using CustomCommander.Interfaces;
using Renci.SshNet.Sftp;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;
using System.Windows;

namespace CustomCommander.ViewModels
{
    /// <summary>
    /// ViewModel principal que gestiona la lógica de interacción entre la vista y los servicios de exploración de archivos,
    /// incluyendo navegación, transferencia de archivos y desconexión del servidor.
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// Servicio utilizado para gestionar las operaciones de exploración de archivos en el sistema local y remoto.
        /// </summary>
        private readonly IFileBrowser _fileBrowser;

        /// <summary>
        /// Inicializa una nueva instancia de <see cref="MainViewModel"/> con el servicio de exploración de archivos proporcionado.
        /// </summary>
        /// <param name="fileBrowser">Servicio para gestionar operaciones locales y remotas de archivos.</param>
        public MainViewModel(IFileBrowser fileBrowser)
        {
            _fileBrowser = fileBrowser;

            // Inicializa las listas de archivos local y remoto.
            ActualizarListadoRemoto();
            ActualizarListadoLocal();

            // Configura las rutas actuales para los directorios locales y remotos.
            CurrentRemotePath = _fileBrowser.CurrentRemotePath;
            CurrentLocalPath = _fileBrowser.CurrentLocalPath;

            // Inicializa los comandos para la navegación y desconexión.
            NavigateToRemoteDirectoryCommand = new CustomCommander.Helpers.RelayCommand(NavigateToParentRemoteDirectory, () => true);
            NavigateToLocalDirectoryCommand = new CustomCommander.Helpers.RelayCommand(NavigateToParentLocalDirectory, () => true);
            DisconnectCommand = new CustomCommander.Helpers.RelayCommand(Disconnect, () => true);
        }

        /// <summary>
        /// Comando para navegar al directorio padre en el servidor remoto.
        /// </summary>
        public ICommand NavigateToRemoteDirectoryCommand { get; }

        /// <summary>
        /// Comando para navegar al directorio padre en el sistema local.
        /// </summary>
        public ICommand NavigateToLocalDirectoryCommand { get; }

        /// <summary>
        /// Comando para desconectarse del servidor.
        /// </summary>
        public ICommand DisconnectCommand { get; }

        /// <summary>
        /// Evento que se invoca al desconectar, para abrir la ventana de inicio de sesión.
        /// </summary>
        public event Action DisconnectMain;

        private ObservableCollection<ISftpFile> _remoteDir;
        private ObservableCollection<FileSystemInfo> _localDir;
        private string _currentRemotePath;
        private string _currentLocalPath;

        /// <summary>
        /// Ruta actual del directorio remoto.
        /// </summary>
        public string CurrentRemotePath
        {
            get => _currentRemotePath;
            set
            {
                _currentRemotePath = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Ruta actual del directorio local.
        /// </summary>
        public string CurrentLocalPath
        {
            get => _currentLocalPath;
            set
            {
                _currentLocalPath = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Lista de archivos y directorios en el directorio remoto actual.
        /// </summary>
        public ObservableCollection<ISftpFile> RemoteDir
        {
            get => _remoteDir;
            set
            {
                _remoteDir = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Lista de archivos y directorios en el directorio local actual.
        /// </summary>
        public ObservableCollection<FileSystemInfo> LocalDir
        {
            get => _localDir;
            set
            {
                _localDir = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Actualiza la lista de archivos y directorios del servidor remoto.
        /// </summary>
        public void ActualizarListadoRemoto()
        {
            // Actualiza la lista de archivos remotos sin transformación
            RemoteDir = new ObservableCollection<ISftpFile>(_fileBrowser.ActualizarListadoRemoto());
        }

        /// <summary>
        /// Actualiza la lista de archivos y directorios en el sistema local.
        /// </summary>
        public void ActualizarListadoLocal()
        {
            // Actualiza la lista de archivos local sin transformación
            LocalDir = new ObservableCollection<FileSystemInfo>(_fileBrowser.ActualizarListadoLocal());

        }

        /// <summary>
        /// Cambia el directorio remoto actual a uno nuevo.
        /// </summary>
        /// <param name="newDir">Ruta del nuevo directorio remoto.</param>
        public void NavigateToRemoteDirectory(string newDir)
        {
            //actualizo el directorio actual en el cliente SFTP a travez del FileBrowser
            _fileBrowser.ActualizarRemotePath(newDir);
            //actualizo la lista de archivos remotos
            ActualizarListadoRemoto();
            CurrentRemotePath = _fileBrowser.CurrentRemotePath;

        }

        /// <summary>
        /// Cambia el directorio local actual a uno nuevo.
        /// </summary>
        /// <param name="newDir">Ruta del nuevo directorio local.</param>
        public void NavigateToLocalDirectory(string newDir)
        {
            //actualizo el directorio actual en el cliente Local a travez del FileBrowser
            _fileBrowser.ActualizarLocalPath(newDir);
            //actualizo la lista de archivos locales
            ActualizarListadoLocal();
            CurrentLocalPath = _fileBrowser.CurrentLocalPath;

        }

        /// <summary>
        /// Navega al directorio padre en el servidor remoto.
        /// </summary>
        public void NavigateToParentRemoteDirectory()
        {
            //navego al directorio padre en el cliente SFTP a travez del FileBrowser
            _fileBrowser.NavigateToParentRemoteDirectory();
            //actualizo la lista de archivos remotos
            ActualizarListadoRemoto();
            CurrentRemotePath = _fileBrowser.CurrentRemotePath;
        }

        /// <summary>
        /// Navega al directorio padre en el sistema local.
        /// </summary>
        public void NavigateToParentLocalDirectory()
        {
            //navego al directorio padre en el cliente Local a travez del FileBrowser
            _fileBrowser.NavigateToParentLocalDirectory();
            //actualizo la lista de archivos locales
            ActualizarListadoLocal();
            CurrentLocalPath = _fileBrowser.CurrentLocalPath;
        }

        /// <summary>
        /// Copia un archivo desde el sistema local al servidor remoto.
        /// </summary>
        /// <param name="localFilePath">Ruta del archivo local.</param>
        /// <param name="remoteFilePath">Ruta destino en el servidor remoto.</param>
        public void CopyLocalToRemote(string localFilePath, string remoteFilePath)
        {
            if (_fileBrowser.FileExistsRemote(remoteFilePath))
            {
                if (MessageBox.Show("El archivo ya existe en el servidor remoto, ¿desea sobreescribirlo?", "Archivo existente", MessageBoxButton.YesNo)
                    == MessageBoxResult.No) return;

                _fileBrowser.DeleteRemoteFile(remoteFilePath);
            }
            _fileBrowser.UploadFile(localFilePath, remoteFilePath);
            ActualizarListadoRemoto();
        }

        /// <summary>
        /// Copia un archivo desde el servidor remoto al sistema local.
        /// </summary>
        /// <param name="remoteFilePath">Ruta del archivo remoto.</param>
        /// <param name="localFilePath">Ruta destino en el sistema local.</param>
        public void CopyRemoteToLocal(string remoteFilePath, string localFilePath)
        {
            if (File.Exists(localFilePath))
            {
                if (MessageBox.Show("El archivo ya existe en el sistema local, ¿desea sobreescribirlo?", "Archivo existente", MessageBoxButton.YesNo)
                    == MessageBoxResult.No) return;
                File.Delete(localFilePath);
            }
            _fileBrowser.DownloadFile(remoteFilePath, localFilePath);
            ActualizarListadoLocal();
        }

        /// <summary>
        /// Desconecta del servidor SFTP y lanza el evento para abrir la ventana de inicio de sesión.
        /// </summary>
        private void Disconnect()
        {
            // Llama al método de desconexión del servicio
            _fileBrowser.Disconnect();

            // Cierra la ventana actual y abre LogWindow
            DisconnectMain?.Invoke();
        }
    }
}
