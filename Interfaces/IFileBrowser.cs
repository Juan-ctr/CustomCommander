using Renci.SshNet.Sftp;
using System.IO;

namespace CustomCommander.Interfaces
{
    /// <summary>
    /// Proporciona funcionalidad para gestionar y navegar por los directorios 
    /// tanto en el sistema de archivos local como en el remoto (SFTP).
    /// Maneja operaciones como listar archivos, cambiar de directorio, 
    /// subir y descargar archivos.
    /// </summary>
    public interface IFileBrowser
    {
        /// <summary>
        /// Obtiene el directorio actual del sistema de archivos remoto.
        /// </summary>
        string CurrentRemotePath { get; }

        /// <summary>
        /// Obtiene el directorio actual del sistema de archivos local.
        /// </summary>
        string CurrentLocalPath { get; }

        /// <summary>
        /// Recupera la lista de archivos y directorios del directorio remoto actual.
        /// </summary>
        /// <returns>Una colección enumerable de archivos y directorios remotos.</returns>
        IEnumerable<ISftpFile> ActualizarListadoRemoto();

        /// <summary>
        /// Recupera la lista de archivos y directorios del directorio local actual.
        /// </summary>
        /// <returns>Una colección enumerable de archivos y directorios locales.</returns>
        IEnumerable<FileSystemInfo> ActualizarListadoLocal();

        /// <summary>
        /// Cambia el directorio actual en el sistema de archivos remoto.
        /// Actualiza la propiedad <see cref="CurrentRemotePath"/>.
        /// </summary>
        /// <param name="newDir">La nueva ruta del directorio.</param>
        void ActualizarRemotePath(string newDir);

        /// <summary>
        /// Cambia el directorio actual en el sistema de archivos local.
        /// Actualiza la propiedad <see cref="CurrentLocalPath"/>.
        /// </summary>
        /// <param name="newDir">La nueva ruta del directorio.</param>
        void ActualizarLocalPath(string newDir);

        /// <summary>
        /// Navega al directorio padre en el sistema de archivos remoto.
        /// Actualiza la propiedad <see cref="CurrentRemotePath"/>.
        /// </summary>
        void NavigateToParentRemoteDirectory();

        /// <summary>
        /// Navega al directorio padre en el sistema de archivos local.
        /// Actualiza la propiedad <see cref="CurrentLocalPath"/>.
        /// </summary>
        void NavigateToParentLocalDirectory();

        /// <summary>
        /// Sube un archivo desde el sistema de archivos local al sistema de archivos remoto.
        /// </summary>
        /// <param name="localPath">La ruta del archivo local.</param>
        /// <param name="remotePath">La ruta de destino en el sistema remoto.</param>
        void UploadFile(string localPath, string remotePath);

        /// <summary>
        /// Descarga un archivo desde el sistema de archivos remoto al sistema de archivos local.
        /// </summary>
        /// <param name="remotePath">La ruta del archivo remoto.</param>
        /// <param name="localPath">La ruta de destino en el sistema local.</param>
        void DownloadFile(string remotePath, string localPath);

        /// <summary>
        /// Verifica si un archivo existe en el sistema de archivos remoto.
        /// </summary>
        /// <param name="remotePath">La ruta del archivo remoto.</param>
        /// <returns><see langword="true"/> si el archivo existe; de lo contrario, <see langword="false"/>.</returns>
        bool FileExistsRemote(string remotePath);

        /// <summary>
        /// Elimina un archivo del sistema de archivos remoto.
        /// </summary>
        /// <param name="remotePath">La ruta del archivo remoto a eliminar.</param>
        void DeleteRemoteFile(string remotePath);

        /// <summary>
        /// Desconecta del sistema de archivos remoto.
        /// </summary>
        void Disconnect();

    }
}
