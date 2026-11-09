# Guía: Arduino Uno con ORICA SafeHands

## 1. Conexiones

| Pin del Arduino | Componente | Qué hace |
|---|---|---|
| 8  | LED verde + resistencia 220 Ω a GND | **INDICADOR** ("ENCENDER INDICADOR") |
| 9  | LED amarillo + 220 Ω (o IN de un módulo relé) | **SIMULADOR / motor** ("INICIAR SIMULADOR") |
| 10 | LED rojo + 220 Ω | **Operación CRÍTICA**: parpadea |
| 11 | Buzzer activo (+ al pin, − a GND) | Suena 1,5 s en una parada o un watchdog |
| 2  | Pulsador: una pata al pin 2, la otra a GND | **PARADA física** (no lleva resistencia: usa INPUT_PULLUP) |
| 13 | LED de la placa | Parpadea con cada mensaje de la PC (enlace vivo) |

Lo mínimo para probar son los LEDs de los pines 8, 9 y 10. El buzzer y el pulsador son opcionales.
Si conectas otra cosa, cambia los números al inicio de `ORICA_SafeHands.ino`.

## 2. Cargar el programa

1. Abre `Arduino/ORICA_SafeHands/ORICA_SafeHands.ino` en el Arduino IDE.
2. Elige **Herramientas → Placa → Arduino Uno** y en **Puerto** el COM donde aparece (por ejemplo COM5).
3. Pulsa **Subir** (→).
4. Para comprobarlo (opcional): abre el **Monitor serie** a **115200** baudios con "Nueva línea". Debe aparecer:
   `$0,EVT,LISTO,SIM-001*..`
   **Cierra el Monitor serie antes de abrir la app**: el puerto solo lo puede usar un programa a la vez.

## 3. Configurar la app

1. En **Configuración**, en **Puerto** elige el COM del Arduino y deja la velocidad en **115200**. Guarda.
2. En **Equipos**, el equipo debe tener el **Identificador de hardware = `SIM-001`**, igual que `EQUIPO_ID` del .ino. El equipo de ejemplo ya lo tiene.
3. Revisa que cada comando tenga su **tipo de acción**:

| Tipo de acción del comando | Orden al Arduino | Resultado |
|---|---|---|
| Informativa (ESTADO DEL EQUIPO) | ESTADO | Responde cómo están las salidas |
| Indicador (ENCENDER INDICADOR) | INDICADOR | Enciende el LED verde |
| Operar (INICIAR SIMULADOR) | OPERAR | Enciende el LED amarillo o el relé |
| Detener (DETENER SIMULADOR) | DETENER | Apaga todo, sin entrar en parada |
| Crítica (OPERACION CRITICA SIMULADA) | CRITICA | LED rojo parpadeando + amarillo |
| Estado seguro | PARADA | Apaga todo y queda en PARADA |

## 4. Pruebas (en este orden)

- [ ] **Conexión**: abre **Diagnóstico**. La tarjeta Arduino debe decir **Conectado**, Heartbeat **0 / 3**, y el monitor serial debe mostrar `· Arduino listo`.
- [ ] **Pedir ESTADO** (botón del monitor): responde con todo apagado y modo NORMAL.
- [ ] **Voz**: como operador di "ENCENDER INDICADOR" y luego "INICIAR SIMULADOR" (con sus confirmaciones). Se encienden los LEDs verde y amarillo.
- [ ] **ESTADO DEL EQUIPO** por voz: el mensaje dice qué está encendido.
- [ ] **DETENER SIMULADOR**: se apagan los LEDs.
- [ ] **Botón DETENER** del operador: apaga todo, suena el buzzer y el equipo queda en **PARADA**. Cualquier orden que encienda algo se rechaza con "El equipo está en PARADA...".
- [ ] **Rearmar**: en Diagnóstico → **Rearmar sistema**. Vuelve a aceptar órdenes.
- [ ] **Pulsador físico**: con algo encendido, presiona el pulsador. Se apaga todo y aparecen un evento y una alerta de "Parada de equipo".
- [ ] **Watchdog**: enciende un LED y en Diagnóstico pulsa **Perder heartbeat**. En aproximadamente 1 s el Arduino apaga todo (WATCHDOG) y sale la alerta. Pulsa **Reanudar heartbeat** y luego **Rearmar sistema**.
- [ ] **Cable USB**: desconéctalo. Sale la alerta "Arduino desconectado". Al volver a conectarlo, la app se reconecta sola en unos segundos.
- [ ] **Historial**: en Eventos, los comandos ejecutados dicen "Ejecutado en el Arduino" y guardan la secuencia y el CRC.

## 5. Si algo falla

| Síntoma | Qué revisar |
|---|---|
| Diagnóstico: **Desconectado** | El COM de Configuración no es el del Arduino, o el Monitor serie del IDE está abierto. |
| Se queda en **Conectando...** y el monitor dice "no respondió a CONFIG" | El programa cargado no es `ORICA_SafeHands.ino`, o la velocidad no es 115200 en los dos lados. |
| "El Arduino no corresponde a este equipo" | El Identificador de hardware del equipo no es `SIM-001`. |
| "El equipo está en PARADA / WATCHDOG" | Es lo esperado después de una parada: **Rearmar sistema** en Diagnóstico. |
| Heartbeat perdido seguido | Sube **Intervalo de heartbeat** en Configuración (por ejemplo a 500 ms). |
| Quiero probar la app **sin** Arduino | En Diagnóstico marca **Simular comandos si no hay Arduino**. |

## Protocolo (resumen técnico)

Cada mensaje es una línea de texto: `$<secuencia>,<tipo>[,<datos>]*<CRC>`.
El CRC es el XOR de todos los caracteres entre `$` y `*`, en 2 dígitos hexadecimales. Ejemplo: `$5,OPERAR,SIM-001*65`.

- **PC → Arduino**: `PING` (cada HEARTBEAT_MS), `CONFIG`, `ESTADO`, `INDICADOR`, `OPERAR`, `CRITICA`, `DETENER`, `PARADA` y `REARME`.
- **Arduino → PC**:
  - `PONG,<modo>`.
  - `ACK,<orden>,<modo>,<indicador>,<simulador>,<critica>` cuando acepta una orden.
  - `NAK,<orden>,<motivo>` cuando la rechaza. Motivos: CRC, SECUENCIA, EQUIPO, PARADA, WATCHDOG o DESCONOCIDO.
  - `EVT,LISTO|PARADA_FISICA|WATCHDOG,<id>` para avisar por su cuenta.
- **Watchdog**: si el Arduino no recibe nada en (heartbeat × máximo de perdidos), con un mínimo de 1 s, apaga todo y queda en WATCHDOG hasta recibir un REARME.
