using CustomCommander.Interfaces;
using CustomCommander.Models;
using Renci.SshNet.Sftp;
using Renci.SshNet;
using System.IO;

namespace CustomCommander.Services
{
    /// <summary>
    /// Gestiona la conexión SFTP y las operaciones relacionadas, como listar, transferir y eliminar archivos en el servidor.
    /// </summary>
    public class SftpClientManager : ISftpClientManager
    {
        /// <summary>
        /// Instancia del cliente SFTP para manejar la conexión.
        /// </summary>
        private SftpClient _sftpClient;

        /// <summary>
        /// Almacena la ruta del directorio inicial (home) para validar cambios de directorio.
        /// </summary>
        private string _home = string.Empty;

        /// <summary>
        /// Constructor por defecto.
        /// </summary>
        public SftpClientManager()
        {

        }

        /// <summary>
        /// Establece una conexión con el servidor SFTP usando los datos de sesión proporcionados.
        /// </summary>
        /// <param name="session">Objeto <see cref="MySession"/> que contiene los datos de la sesión (host, usuario, contraseña, directorio).</param>
        public void Connect(MySession session)
        {
            _sftpClient?.Dispose();
            // Crear una nueva instancia del cliente SFTP
            _sftpClient = new SftpClient(session.HostName, session.UserName, session.Password);

            // Conecta al servidor SFTP
            _sftpClient.Connect();
            _sftpClient.ChangeDirectory(session.CurrentDir);
            _home = session.CurrentDir;
            //quitar lo siguiente
            //string pathh = _sftpClient.WorkingDirectory;
            //foreach (SftpFile file in _sftpClient.ListDirectory(pathh)) Debug.WriteLine(file.FullName);

            //Debug.WriteLine($"Conectando a {session.HostName}, {session.Description},{session.UserName},{session.Password},{session.CurrentDir}");
        }

        /// <summary>
        /// Cambia el directorio actual en el servidor SFTP.
        /// </summary>
        /// <param name="newDir">Ruta del nuevo directorio.</param>
        public void SetWorkingDirectory(string newDir)
        {
            _sftpClient.ChangeDirectory(newDir);
        }

        /// <summary>
        /// Obtiene la ruta del directorio actual en el servidor SFTP.
        /// </summary>
        /// <returns>Ruta del directorio actual.</returns>
        public string GetWorkingDirectory()
        {
            return _sftpClient.WorkingDirectory;
        }

        /// <summary>
        /// Cierra la conexión con el servidor SFTP.
        /// </summary>
        public void Disconnect()
        {
            // Desconectar del servidor SFTP
            _sftpClient?.Dispose();
        }

        /// <summary>
        /// Lista los archivos y directorios en el directorio actual del servidor SFTP, excluyendo elementos ocultos.
        /// </summary>
        /// <returns>Colección de archivos y directorios disponibles.</returns>
        /// <exception cref="InvalidOperationException">Se lanza si no hay una conexión activa al servidor SFTP.</exception>
        public IEnumerable<ISftpFile> ListFiles()
        {

            return _sftpClient?.IsConnected == true ? _sftpClient.ListDirectory(_sftpClient.WorkingDirectory).Where(file => !file.Name.StartsWith(".")) :
                throw new InvalidOperationException("No hay una conexión activa al servidor SFTP.");
        }

        /// <summary>
        /// Navega al directorio padre del directorio actual en el servidor SFTP.
        /// </summary>
        public void NavigateToParentRemoteDirectory()
        {
            string currentDir = _sftpClient.WorkingDirectory;

            string? parentPath = Path.GetDirectoryName(currentDir)?.Replace('\\', '/'); // Obtén el directorio padre
            if (!string.IsNullOrEmpty(parentPath) && parentPath != "/home") //Valido que el usuario no pueda ir mas atras de su home.
            {
                _sftpClient.ChangeDirectory(parentPath);
            }
        }

        /// <summary>
        /// Descarga un archivo del servidor SFTP al sistema local.
        /// </summary>
        /// <param name="remotePath">Ruta del archivo en el servidor.</param>
        /// <param name="localPath">Ruta de destino en el sistema local.</param>
        public void DownloadFile(string remotePath, string localPath)
        {
            // Descargar archivo del servidor SFTP
            using (var fileStream = new FileStream(localPath, FileMode.Create))
            {
                _sftpClient.DownloadFile(remotePath, fileStream);
            }
        }

        /// <summary>
        /// Sube un archivo del sistema local al servidor SFTP.
        /// </summary>
        /// <param name="localPath">Ruta del archivo en el sistema local.</param>
        /// <param name="remotePath">Ruta de destino en el servidor.</param>
        public void UploadFile(string localPath, string remotePath)
        {
            // Subir archivo al servidor SFTP
            using (var fileStream = new FileStream(localPath, FileMode.Open))
            {
                _sftpClient.UploadFile(fileStream, remotePath, true); // Sobrescribe si ya existe
                _sftpClient.ChangePermissions(remotePath, 777);
            }
        }

        /// <summary>
        /// Elimina un archivo en el servidor SFTP.
        /// </summary>
        /// <param name="remotePath">Ruta del archivo a eliminar.</param>
        public void DeleteFile(string remotePath)
        {
            // Eliminar archivo del servidor SFTP
            _sftpClient.DeleteFile(remotePath);
        }

        /// <summary>
        /// Verifica si un archivo existe en el servidor SFTP.
        /// </summary>
        /// <param name="remotePath">Ruta del archivo a verificar.</param>
        /// <returns>True si el archivo existe, de lo contrario false.</returns>
        public bool FileExists(string remotePath)
        {
            // Verifico si existe el archivo
            return _sftpClient.Exists(remotePath);
        }

    }
}
