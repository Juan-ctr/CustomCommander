using CustomCommander.Interfaces;
using Renci.SshNet.Sftp;
using System.IO;

namespace CustomCommander.Services
{
    /// <summary>
    /// Proporciona funcionalidad para gestionar y navegar por los directorios 
    /// tanto en el sistema de archivos local como en el remoto (SFTP).
    /// Maneja operaciones como listar archivos, cambiar de directorio, 
    /// subir y descargar archivos.
    /// </summary>
    public class FileBrowser : IFileBrowser
    {
        private readonly ISftpClientManager _sftpClientManager; // Gestiona las interacciones con el servidor remoto SFTP.

        private readonly ILocalClientManager _localClientManager; // Gestiona las interacciones con el sistema de archivos local.


        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="FileBrowser"/>.
        /// Configura los directorios actuales para los sistemas de archivos local y remoto.
        /// </summary>
        /// <param name="sftpClientManager">El administrador del cliente SFTP.</param>
        /// <param name="localClientManager">El administrador del sistema de archivos local.</param>
        public FileBrowser(ISftpClientManager sftpClientManager, ILocalClientManager localClientManager)
        {
            _sftpClientManager = sftpClientManager;
            _localClientManager = localClientManager;

            // Inicializa las rutas actuales.
            CurrentRemotePath = _sftpClientManager.GetWorkingDirectory();
            CurrentLocalPath = _localClientManager.GetWorkingDirectory();
        }

        /// <summary>
        /// Obtiene el directorio actual del sistema de archivos remoto.
        /// </summary>
        public string CurrentRemotePath { get; private set; }

        /// <summary>
        /// Obtiene el directorio actual del sistema de archivos local.
        /// </summary>
        public string CurrentLocalPath { get; private set; }

        /// <summary>
        /// Recupera la lista de archivos y directorios del directorio remoto actual.
        /// </summary>
        /// <returns>Una colección enumerable de archivos y directorios remotos.</returns>
        public IEnumerable<ISftpFile> ActualizarListadoRemoto()
        {
            return _sftpClientManager.ListFiles();
        }

        /// <summary>
        /// Recupera la lista de archivos y directorios del directorio local actual.
        /// </summary>
        /// <returns>Una colección enumerable de archivos y directorios locales.</returns>
        public IEnumerable<FileSystemInfo> ActualizarListadoLocal()
        {
            return _localClientManager.ListFiles();
        }

        /// <summary>
        /// Cambia el directorio actual en el sistema de archivos remoto.
        /// Actualiza la propiedad <see cref="CurrentRemotePath"/>.
        /// </summary>
        /// <param name="newDir">La nueva ruta del directorio.</param>
        public void ActualizarRemotePath(string newDir)
        {
            _sftpClientManager.SetWorkingDirectory(newDir);
            CurrentRemotePath = newDir;
        }

        /// <summary>
        /// Cambia el directorio actual en el sistema de archivos local.
        /// Actualiza la propiedad <see cref="CurrentLocalPath"/>.
        /// </summary>
        /// <param name="newDir">La nueva ruta del directorio.</param>
        public void ActualizarLocalPath(string newDir)
        {
            _localClientManager.SetWorkingDirectory(newDir);
            CurrentLocalPath = newDir;
        }

        /// <summary>
        /// Navega al directorio padre en el sistema de archivos remoto.
        /// Actualiza la propiedad <see cref="CurrentRemotePath"/>.
        /// </summary>
        public void NavigateToParentRemoteDirectory()
        {
            _sftpClientManager.NavigateToParentRemoteDirectory();
            CurrentRemotePath = _sftpClientManager.GetWorkingDirectory();
        }

        /// <summary>
        /// Navega al directorio padre en el sistema de archivos local.
        /// Actualiza la propiedad <see cref="CurrentLocalPath"/>.
        /// </summary>
        public void NavigateToParentLocalDirectory()
        {
            _localClientManager.NavigateToParentLocalDirectory();
            CurrentLocalPath = _localClientManager.GetWorkingDirectory();
        }

        /// <summary>
        /// Sube un archivo desde el sistema de archivos local al sistema de archivos remoto.
        /// </summary>
        /// <param name="localPath">La ruta del archivo local.</param>
        /// <param name="remotePath">La ruta de destino en el sistema remoto.</param>
        public void UploadFile(string localPath, string remotePath)
        {
            _sftpClientManager.UploadFile(localPath, remotePath);
        }

        /// <summary>
        /// Descarga un archivo desde el sistema de archivos remoto al sistema de archivos local.
        /// </summary>
        /// <param name="remotePath">La ruta del archivo remoto.</param>
        /// <param name="localPath">La ruta de destino en el sistema local.</param>
        public void DownloadFile(string remotePath, string localPath)
        {
            _sftpClientManager.DownloadFile(remotePath, localPath);
        }

        /// <summary>
        /// Verifica si un archivo existe en el sistema de archivos remoto.
        /// </summary>
        /// <param name="remotePath">La ruta del archivo remoto.</param>
        /// <returns><see langword="true"/> si el archivo existe; de lo contrario, <see langword="false"/>.</returns>
        public bool FileExistsRemote(string remotePath)
        {
            return _sftpClientManager.FileExists(remotePath);
        }

        /// <summary>
        /// Elimina un archivo del sistema de archivos remoto.
        /// </summary>
        /// <param name="remotePath">La ruta del archivo remoto a eliminar.</param>
        public void DeleteRemoteFile(string remotePath)
        {
            _sftpClientManager.DeleteFile(remotePath);
        }

        /// <summary>
        /// Desconecta del sistema de archivos remoto.
        /// </summary>
        public void Disconnect()
        {
            _sftpClientManager.Disconnect();
        }
    }
}
