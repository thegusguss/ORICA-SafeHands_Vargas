// ============================================================
//  ORICA SafeHands - Programa del Arduino Uno
//  Recibe órdenes de la PC por USB (puerto serie) y maneja las salidas.
//
//  Trama (texto, una por línea):   $<secuencia>,<tipo>[,<dato>...]*<CRC>\n
//    CRC = XOR de todos los caracteres entre '$' y '*', en 2 dígitos hex.
//    Ejemplo:  $5,OPERAR,SIM-001*65
//
//  PC -> Arduino: PING, CONFIG, ESTADO, INDICADOR, OPERAR, CRITICA, DETENER, PARADA, REARME
//  Arduino -> PC: PONG, ACK, NAK y avisos EVT (LISTO, PARADA_FISICA, WATCHDOG)
//
//  Conexiones (cambiar los números si se arma distinto):
//    Pin 8  -> LED verde      (INDICADOR)          con resistencia de 220 ohm a GND
//    Pin 9  -> LED amarillo   (SIMULADOR / motor)  o el IN de un módulo relé
//    Pin 10 -> LED rojo       (operación CRÍTICA)
//    Pin 11 -> Buzzer activo  (suena en parada y watchdog)
//    Pin 2  -> Pulsador de PARADA física, el otro lado a GND (usa INPUT_PULLUP)
//    Pin 13 -> LED de la placa: parpadea con cada mensaje de la PC (enlace vivo)
// ============================================================

const char* EQUIPO_ID = "SIM-001";   // = "Identificador de hardware" del equipo en la app

const int PIN_INDICADOR = 8;
const int PIN_SIMULADOR = 9;
const int PIN_CRITICA   = 10;
const int PIN_BUZZER    = 11;
const int PIN_PARADA    = 2;
const int PIN_ENLACE    = LED_BUILTIN;

// Configuración (la PC la envía con CONFIG al conectarse; estos son los valores por defecto)
unsigned long watchdogMs = 1000;   // si no llega nada en este tiempo -> estado seguro
bool watchdogActivo = true;
bool crcObligatorio = true;
bool secuenciaObligatoria = true;
bool rearmeRequerido = true;

// Estado
enum Modo { NORMAL, PARADA, WATCHDOG };
Modo modo = NORMAL;
bool indicador = false, simulador = false, critica = false;
long ultimaSecuencia = 0;
unsigned long ultimoMensaje = 0;
bool pcConectada = false;          // el watchdog solo vigila después de que la PC habló
unsigned long finBuzzer = 0;
bool botonAnterior = HIGH;

String buffer = "";

// ---------------- Inicio ----------------

void setup() {
  pinMode(PIN_INDICADOR, OUTPUT);
  pinMode(PIN_SIMULADOR, OUTPUT);
  pinMode(PIN_CRITICA, OUTPUT);
  pinMode(PIN_BUZZER, OUTPUT);
  pinMode(PIN_ENLACE, OUTPUT);
  pinMode(PIN_PARADA, INPUT_PULLUP);

  apagarTodo();
  Serial.begin(115200);              // debe ser igual a BAUD_RATE en Configuración de la app
  ultimoMensaje = millis();
  enviar(0, "EVT,LISTO," + String(EQUIPO_ID));
}

void loop() {
  leerSerial();
  revisarBotonParada();
  revisarWatchdog();
  actualizarSalidas();
}

// ---------------- Lectura de tramas ----------------

void leerSerial() {
  while (Serial.available() > 0) {
    char c = (char)Serial.read();
    if (c == '\n') {
      buffer.trim();
      if (buffer.length() > 0) procesar(buffer);
      buffer = "";
    } else if (buffer.length() < 80) {
      buffer += c;
    } else {
      buffer = "";                   // línea demasiado larga: se descarta
    }
  }
}

void procesar(String linea) {
  int asterisco = linea.lastIndexOf('*');
  if (!linea.startsWith("$") || asterisco < 0) return;

  String cuerpo = linea.substring(1, asterisco);
  String crcRecibido = linea.substring(asterisco + 1);

  // Separar: secuencia, tipo, dato1, dato2...
  String partes[7];
  int n = separar(cuerpo, partes, 7);
  if (n < 2) return;

  long secuencia = partes[0].toInt();
  String tipo = partes[1];

  ultimoMensaje = millis();          // cualquier trama mantiene vivo el watchdog
  pcConectada = true;
  digitalWrite(PIN_ENLACE, !digitalRead(PIN_ENLACE));

  if (crcObligatorio && !crcRecibido.equalsIgnoreCase(crc(cuerpo))) { nak(secuencia, tipo, "CRC"); return; }

  if (tipo == "PING") { enviar(secuencia, "PONG," + textoModo()); return; }

  // La secuencia debe avanzar (evita órdenes repetidas o viejas). Vuelve a empezar en 1 al reconectar.
  if (secuenciaObligatoria && secuencia != 1 && secuencia <= ultimaSecuencia) { nak(secuencia, tipo, "SECUENCIA"); return; }
  ultimaSecuencia = secuencia;

  if (tipo == "CONFIG") {
    // CONFIG,<watchdogMs>,<watchdog 0/1>,<crc 0/1>,<secuencia 0/1>,<rearme 0/1>
    watchdogMs = max(500L, partes[2].toInt());
    watchdogActivo = partes[3] == "1";
    crcObligatorio = partes[4] == "1";
    secuenciaObligatoria = partes[5] == "1";
    rearmeRequerido = partes[6] == "1";
    ack(secuencia, tipo);
    return;
  }

  if (tipo == "REARME") {
    modo = NORMAL;
    ack(secuencia, tipo);
    return;
  }

  // Desde aquí las órdenes llevan el equipo: partes[2] = EQUIPO_ID (o "*" para todos).
  String equipo = n > 2 ? partes[2] : "";
  if (equipo != "*" && equipo != EQUIPO_ID) { nak(secuencia, tipo, "EQUIPO"); return; }

  if (tipo == "PARADA") {            // la parada siempre se acepta
    pararTodo(PARADA);
    ack(secuencia, tipo);
    return;
  }

  if (tipo == "DETENER") {           // detiene la operación, sin entrar en parada
    apagarTodo();
    ack(secuencia, tipo);
    return;
  }

  if (tipo == "ESTADO") { ack(secuencia, tipo); return; }

  // Las órdenes que encienden algo no se aceptan en parada ni tras un watchdog (hace falta REARME).
  if (modo != NORMAL) { nak(secuencia, tipo, textoModo()); return; }

  if (tipo == "INDICADOR")     indicador = true;
  else if (tipo == "OPERAR")   simulador = true;
  else if (tipo == "CRITICA")  { critica = true; simulador = true; }
  else { nak(secuencia, tipo, "DESCONOCIDO"); return; }

  ack(secuencia, tipo);
}

// Separa el texto por comas. Devuelve cuántas partes encontró.
int separar(String texto, String partes[], int maximo) {
  int n = 0, inicio = 0;
  while (n < maximo) {
    int coma = texto.indexOf(',', inicio);
    if (coma < 0) { partes[n++] = texto.substring(inicio); break; }
    partes[n++] = texto.substring(inicio, coma);
    inicio = coma + 1;
  }
  return n;
}

// ---------------- Seguridad ----------------

void revisarBotonParada() {
  bool boton = digitalRead(PIN_PARADA);
  if (botonAnterior == HIGH && boton == LOW) {   // se acaba de presionar
    pararTodo(PARADA);
    enviar(0, "EVT,PARADA_FISICA," + String(EQUIPO_ID));
  }
  botonAnterior = boton;
}

void revisarWatchdog() {
  if (!watchdogActivo || !pcConectada) return;
  if (millis() - ultimoMensaje > watchdogMs) {
    // Se perdió la PC: todo a estado seguro.
    if (rearmeRequerido) pararTodo(WATCHDOG);
    else apagarTodo();
    enviar(0, "EVT,WATCHDOG," + String(EQUIPO_ID));
    pcConectada = false;             // no se repite el aviso hasta que la PC vuelva a hablar
  }
}

void pararTodo(Modo nuevoModo) {
  apagarTodo();
  if (rearmeRequerido || nuevoModo == WATCHDOG) modo = nuevoModo;
  finBuzzer = millis() + 1500;       // suena 1,5 s
}

void apagarTodo() {
  indicador = false;
  simulador = false;
  critica = false;
}

void actualizarSalidas() {
  digitalWrite(PIN_INDICADOR, indicador);
  digitalWrite(PIN_SIMULADOR, simulador);
  // La operación crítica parpadea para que se note.
  digitalWrite(PIN_CRITICA, critica && (millis() / 250) % 2 == 0);
  digitalWrite(PIN_BUZZER, millis() < finBuzzer);
}

// ---------------- Respuestas ----------------

String textoModo() {
  if (modo == PARADA) return "PARADA";
  if (modo == WATCHDOG) return "WATCHDOG";
  return "NORMAL";
}

// ACK con el estado de las salidas: ACK,<tipo>,<modo>,<indicador>,<simulador>,<critica>
void ack(long secuencia, String tipo) {
  enviar(secuencia, "ACK," + tipo + "," + textoModo() + "," +
                    String(indicador) + "," + String(simulador) + "," + String(critica));
}

void nak(long secuencia, String tipo, String motivo) {
  enviar(secuencia, "NAK," + tipo + "," + motivo);
}

void enviar(long secuencia, String datos) {
  String cuerpo = String(secuencia) + "," + datos;
  Serial.print("$");
  Serial.print(cuerpo);
  Serial.print("*");
  Serial.println(crc(cuerpo));       // println termina con \r\n; la PC ignora el \r
}

String crc(String cuerpo) {
  byte x = 0;
  for (unsigned int i = 0; i < cuerpo.length(); i++) x ^= (byte)cuerpo[i];
  String hex = String(x, HEX);
  hex.toUpperCase();
  return x < 16 ? "0" + hex : hex;
}
