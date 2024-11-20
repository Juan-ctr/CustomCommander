using CustomCommander.Interfaces;
using System.IO;

namespace CustomCommander.Services
{
    /// <summary>
    /// Clase encargada de gestionar operaciones locales relacionadas con el sistema de archivos,
    /// como navegar entre directorios, listar archivos y cambiar el directorio actual.
    /// </summary>
    public class LocalClientManager : ILocalClientManager
    {
        /// <summary>
        /// Almacena el directorio actual en el que se están realizando operaciones.
        /// </summary>
        private string _actualDirectory;

        /// <summary>
        /// Constructor de la clase. Inicializa el directorio actual al directorio de documentos del usuario.
        /// </summary>
        public LocalClientManager()
        {
            _actualDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Documents");
        }

        /// <summary>
        /// Establece un nuevo directorio de trabajo para realizar operaciones locales.
        /// </summary>
        /// <param name="newDir">Ruta del nuevo directorio.</param>
        public void SetWorkingDirectory(string newDir)
        {
            _actualDirectory = newDir;
        }

        /// <summary>
        /// Obtiene la ruta del directorio actual en el que se están realizando operaciones.
        /// </summary>
        /// <returns>Ruta del directorio actual.</returns>
        public string GetWorkingDirectory()
        {
            return _actualDirectory;
        }

        /// <summary>
        /// Lista todos los archivos y subdirectorios dentro del directorio actual.
        /// </summary>
        /// <returns>Una colección de objetos <see cref="FileSystemInfo"/> que representan archivos y directorios.</returns>
        /// <exception cref="DirectoryNotFoundException">Se lanza si el directorio actual no existe.</exception>
        public IEnumerable<FileSystemInfo> ListFiles()
        {
            if (!Directory.Exists(_actualDirectory))
            {
                throw new DirectoryNotFoundException($"El directorio {_actualDirectory} no existe!");
            }

            var items = new List<FileSystemInfo>();
            //Debo agregar carpetas y archivos por separado, ya que no existe método que devuelva ambos a la vez.
            // Agrega subdirectorios
            foreach (var directoryPath in Directory.GetDirectories(_actualDirectory))
            {
                var directoryInfo = new DirectoryInfo(directoryPath);
                items.Add(directoryInfo);
            }

            // Agrega archivos
            foreach (var filePath in Directory.GetFiles(_actualDirectory))
            {
                var fileInfo = new FileInfo(filePath);
                items.Add(fileInfo);
            }

            return items;
        }

        /// <summary>
        /// Navega al directorio padre del directorio actual, si existe.
        /// </summary>
        public void NavigateToParentLocalDirectory()
        {
            var currentDir = new DirectoryInfo(_actualDirectory);
            if (currentDir.Parent != null)
            {
                _actualDirectory = currentDir.Parent.FullName;
            }
        }
    }
}
