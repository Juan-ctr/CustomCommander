using CustomCommander.Models;
using Renci.SshNet.Sftp;

namespace CustomCommander.Interfaces
{
    /// <summary>
    /// Gestiona la conexión SFTP y las operaciones relacionadas, como listar, transferir y eliminar archivos en el servidor.
    /// </summary>
    public interface ISftpClientManager
    {
        /// <summary>
        /// Establece una conexión con el servidor SFTP usando los datos de sesión proporcionados.
        /// </summary>
        /// <param name="session">Objeto <see cref="MySession"/> que contiene los datos de la sesión (host, usuario, contraseña, directorio).</param>
        void Connect(MySession session);

        /// <summary>
        /// Cambia el directorio actual en el servidor SFTP.
        /// </summary>
        /// <param name="newDir">Ruta del nuevo directorio.</param>
        void SetWorkingDirectory(string newDir);

        /// <summary>
        /// Obtiene la ruta del directorio actual en el servidor SFTP.
        /// </summary>
        /// <returns>Ruta del directorio actual.</returns>
        string GetWorkingDirectory();

        /// <summary>
        /// Cierra la conexión con el servidor SFTP.
        /// </summary>
        void Disconnect();

        /// <summary>
        /// Lista los archivos y directorios en el directorio actual del servidor SFTP, excluyendo elementos ocultos.
        /// </summary>
        /// <returns>Colección de archivos y directorios disponibles.</returns>
        /// <exception cref="InvalidOperationException">Se lanza si no hay una conexión activa al servidor SFTP.</exception>
        IEnumerable<ISftpFile> ListFiles();

        /// <summary>
        /// Descarga un archivo del servidor SFTP al sistema local.
        /// </summary>
        /// <param name="remotePath">Ruta del archivo en el servidor.</param>
        /// <param name="localPath">Ruta de destino en el sistema local.</param>
        void DownloadFile(string remotePath, string localPath);

        /// <summary>
        /// Navega al directorio padre del directorio actual en el servidor SFTP.
        /// </summary>
        void NavigateToParentRemoteDirectory();

        /// <summary>
        /// Sube un archivo del sistema local al servidor SFTP.
        /// </summary>
        /// <param name="localPath">Ruta del archivo en el sistema local.</param>
        /// <param name="remotePath">Ruta de destino en el servidor.</param>
        void UploadFile(string localPath, string remotePath);

        /// <summary>
        /// Elimina un archivo en el servidor SFTP.
        /// </summary>
        /// <param name="remotePath">Ruta del archivo a eliminar.</param>
        void DeleteFile(string remotePath);

        /// <summary>
        /// Verifica si un archivo existe en el servidor SFTP.
        /// </summary>
        /// <param name="remotePath">Ruta del archivo a verificar.</param>
        /// <returns>True si el archivo existe, de lo contrario false.</returns>
        bool FileExists(string remotePath);
    }
}
