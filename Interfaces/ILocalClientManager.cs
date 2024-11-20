using System.IO;

namespace CustomCommander.Interfaces
{
    /// <summary>
    /// Clase encargada de gestionar operaciones locales relacionadas con el sistema de archivos,
    /// como navegar entre directorios, listar archivos y cambiar el directorio actual.
    /// </summary>
    public interface ILocalClientManager
    {
        /// <summary>
        /// Lista todos los archivos y subdirectorios dentro del directorio actual.
        /// </summary>
        /// <returns>Una colección de objetos <see cref="FileSystemInfo"/> que representan archivos y directorios.</returns>
        /// <exception cref="DirectoryNotFoundException">Se lanza si el directorio actual no existe.</exception>
        IEnumerable<FileSystemInfo> ListFiles();

        /// <summary>
        /// Establece un nuevo directorio de trabajo para realizar operaciones locales.
        /// </summary>
        /// <param name="newDir">Ruta del nuevo directorio.</param>
        void SetWorkingDirectory(string newDir);

        /// <summary>
        /// Obtiene la ruta del directorio actual en el que se están realizando operaciones.
        /// </summary>
        /// <returns>Ruta del directorio actual.</returns>
        string GetWorkingDirectory();

        /// <summary>
        /// Navega al directorio padre del directorio actual, si existe.
        /// </summary>
        void NavigateToParentLocalDirectory();
    }
}
