# CustomCommander

**CustomCommander** es una herramienta de escritorio diseñada para gestionar archivos de manera eficiente entre un sistema local y un servidor remoto mediante conexiones SFTP.

---

## Idea del Proyecto

El objetivo principal de **CustomCommander** es proporcionar una herramienta que permita copiar archivos entre un sistema local y un servidor remoto en entornos donde no es posible utilizar protocolos como **Samba**. Además, se busca tener un control estricto sobre los usuarios, limitando su navegación al directorio **home** asignado y habilitando únicamente la copia de archivos, cumpliendo con los requerimientos específicos del proyecto.

---

## Características

- **Transferencia de archivos**: Arrastra y suelta archivos entre el sistema local y el servidor remoto.
- **Navegación restringida**: Los usuarios solo pueden explorar su directorio **home**, garantizando un entorno seguro y controlado.
- **Gestión de permisos**: Validación segura de accesos antes de realizar cualquier operación.
- **Conexión SFTP**: Configuración y conexión simple a servidores remotos.
- **Restricción de operaciones**: Solo está habilitada la copia de archivos, deshabilitando la edición o modificación directa de archivos en el servidor. No se pueden crear, ni modificar directorios.
- **Interfaz amigable**: Diseño claro y accesible para usuarios.

---

## Requisitos

- **Sistema operativo**: Windows 10 o superior.
- **Framework**: .NET 8.
- **Permisos de administrador**: Para realizar operaciones en carpetas restringidas.

---

## Instalación

1. Descarga el archivo instalador desde el repositorio o del enlace proporcionado.
2. Ejecuta el instalador y sigue los pasos en pantalla.
3. Una vez instalado, encontrarás un acceso directo en el menú de inicio.
4. En C:/ProgramData/CustomCommander, hay un archivo config.json. En dicho archivo se deben agregar el host y su descripción, que luego se utilizarán para realizar la conexión sftp (Se pueden agregar todos los host que se deseen)

---

## Uso

1. Abre **CustomCommander** desde el acceso directo.
2. Configura las credenciales del servidor remoto en la pantalla de inicio.
3. Navega por los directorios locales y remotos para gestionar archivos.
4. Arrastra y suelta archivos para transferirlos entre sistemas, respetando las restricciones de navegación y permisos.

---


## Consideraciones sobre el instalador

- Si eres usuario final, puedes descargar solo el instalador desde la sección **Releases**.

¡Gracias por usar CustomCommander!
