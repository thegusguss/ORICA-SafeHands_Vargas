# Guía: llevar ORICA SafeHands a otra PC

## Lo que tienes que llevar (git NO lo sube)

| Archivo | Dónde está en esta PC | Para qué |
|---|---|---|
| `conexion.config` | Carpeta del proyecto, junto a `App.config` | Cadena de Azure con la contraseña |

Llévalo por USB, por tu correo o por OneDrive. **No lo subas a GitHub** (el repositorio es público).

Todo lo demás (código, scripts, `conexion.ejemplo.config`) ya está en GitHub.

---

## 1. Programas que necesita la otra PC

- **Visual Studio** con la carga de trabajo **"Desarrollo de escritorio de .NET"** (.NET Framework 4.7.2).
- **SQL Server LocalDB**: viene con Visual Studio si marcas **"Almacenamiento y procesamiento de datos"**.
  Para comprobarlo, abre una terminal y escribe:
  ```
  sqllocaldb info MSSQLLocalDB
  ```
  Si responde con datos, está instalada.
- **SSMS** (SQL Server Management Studio), para ejecutar el script de la base local.
- **Git**.

## 2. Clonar el proyecto

```
git clone -b migracion-3-capas https://github.com/thegusguss/ORICA-SafeHands_Vargas.git
```

Se crea la carpeta `ORICA-SafeHands_Vargas` con la solución `PROYECTO_ORICA_SAFEHANDS_VARGAS.sln`.

## 3. Copiar la conexión a Azure

Copia `conexion.config` dentro de la carpeta `ORICA-SafeHands_Vargas`, junto a `App.config`.

Si no lo tienes a mano: copia `conexion.ejemplo.config`, renómbralo a `conexion.config` y completa estos datos:
servidor `upttutorserver.database.windows.net`, usuario `adminupt` y la contraseña.

## 4. Crear la base local (copia sin internet)

1. Abre SSMS y conéctate a `(localdb)\MSSQLLocalDB` con autenticación de Windows.
2. Ve a **Archivo → Abrir → Archivo** y elige `Scripts\ORICA_SafeHandsDB_Completo.sql`. No copies y pegues el texto.
3. Presiona F5. Debe decir *"ORICA_SafeHandsDB creada correctamente."*

## 5. Permitir la PC en Azure (si estás en otra red)

En el portal de Azure, ve a **upttutorserver → Redes**, pulsa **"+ Agregar la dirección IPv4 del cliente"** y luego **Guardar**.
Si no lo haces, la app trabaja solo en local y Sincronización muestra el error de firewall.

## 6. Compilar y la primera sincronización

1. Abre `PROYECTO_ORICA_SAFEHANDS_VARGAS.sln` en Visual Studio.
2. Ve a **Compilar → Recompilar solución**. Así se copia `conexion.config` a `bin\Debug`.
3. Ejecuta (F5) y entra como **ADMIN**. La primera vez, la app usa los datos base del script.
4. Abre **Sincronización** y pulsa **"Descargar de Azure"**.
   **No uses "Subir esta PC a Azure"**: reemplazaría todo Azure con los datos base y perderías lo que subiste.
5. Debe quedar **Azure SQL: Conectado** y la cabecera en **"● Azure conectado"**.

---

## Lista para verificar

- [ ] Compila sin errores.
- [ ] Sincronización muestra **Conectado** y la copia local tiene fecha.
- [ ] Aparecen los mismos operadores, comandos y eventos que en la otra PC.
- [ ] Un cambio hecho en una PC (por ejemplo, editar un comando) aparece en la otra al recargar.
- [ ] Modo sin conexión: **Simular sin conexión** (o desconectar el Wi-Fi), entrar como operador, dar órdenes y luego **Volver a conectar**. Pendientes debe volver a 0.

## Si algo falla

| Mensaje | Qué hacer |
|---|---|
| Azure SQL: **No configurado** | Falta `conexion.config` o no se recompiló. Revisa que esté junto a `App.config` y usa **Recompilar solución**. |
| "Azure no respondió: ... IP address ... not allowed" | Agrega la IP en **Redes** del servidor (paso 5). |
| "Azure no respondió: Login failed" | Contraseña incorrecta en `conexion.config`. |
| "Azure no respondió: ... tiempo de espera" | La base gratuita estaba pausada. Espera un minuto y vuelve a intentar. |
| "La base local (LocalDB) no respondió" | Ejecuta `sqllocaldb start MSSQLLocalDB` y revisa el paso 4. |
| "Esta PC nunca se sincronizó" | Normal la primera vez: pulsa **Descargar de Azure**. |
| El diseñador no abre un formulario | Usa **Recompilar solución**, cierra la pestaña y ábrela de nuevo. |
