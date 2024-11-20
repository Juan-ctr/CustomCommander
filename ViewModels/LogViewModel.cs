using CustomCommander.Helpers;
using CustomCommander.Interfaces;
using CustomCommander.Models;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.IO;
using System.Security.Authentication;
using System.Windows.Input;

namespace CustomCommander.ViewModels
{
    /// <summary>
    /// ViewModel para la ventana de inicio de sesión, gestiona la configuración de conexiones, autenticación
    /// y eventos relacionados a la conexión con el servidor SFTP.
    /// </summary>
    public class LogViewModel : ViewModelBase
    {
        /// <summary>
        /// Cliente SFTP utilizado para gestionar la conexión con el servidor.
        /// </summary>
        private readonly ISftpClientManager _sftpClientManager;

        /// <summary>
        /// Lista de conexiones disponibles obtenidas desde la configuración.
        /// </summary>
        public ObservableCollection<MyConnectionInfo> Connections { get; set; }

        /// <summary>
        /// Conexión seleccionada por el usuario.
        /// </summary>
        private MyConnectionInfo _selectedConnection;

        /// <summary>
        /// Constructor del ViewModel. Carga la configuración de conexiones desde un archivo JSON
        /// y configura los comandos y propiedades necesarias.
        /// </summary>
        /// <param name="sftpClientManager">Cliente SFTP utilizado para gestionar las conexiones.</param>
        public LogViewModel(ISftpClientManager sftpClientManager)
        {
            _sftpClientManager = sftpClientManager;

            // Ruta del archivo de configuración
            string configPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "CustomCommander", "config.json")/*?.Replace("\\","/")*/;

            // Carga y deserializa el archivo de configuración
            string json = File.Exists(configPath) ? File.ReadAllText(configPath) : throw new InvalidDataException($"El archivo de configuración no se encontró en la ruta: {configPath}");
            Config config = JsonConvert.DeserializeObject<Config>(json);

            // Inicializa la lista de conexiones
            Connections = new ObservableCollection<MyConnectionInfo>();
            foreach (var server in config.Servers)
            {
                //Console.WriteLine($"Servidor: {server.Description}, Host: {server.HostName}");
                var con = new MyConnectionInfo(server.HostName, server.Description);
                Connections.Add(con);
            }

            // Selecciona la primera conexión como predeterminada
            SelectedConnection = Connections.FirstOrDefault();

            // Inicializa el comando de conexión
            ConnectCommand = new RelayCommand(Connect, CanConnect);
        }

        /// <summary>
        /// Comando para establecer una conexión con el servidor SFTP.
        /// </summary>
        public ICommand ConnectCommand { get; }

        /// <summary>
        /// Evento que se lanza cuando la conexión es exitosa.
        /// </summary>
        public event Action ConnectionSuccessful;

        /// <summary>
        /// Propiedad que representa la conexión seleccionada por el usuario.
        /// </summary>
        public MyConnectionInfo SelectedConnection
        {
            get => _selectedConnection;
            set
            {
                _selectedConnection = value;
                OnPropertyChanged();
                (ConnectCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// Nombre de usuario ingresado por el usuario.
        /// </summary>
        private string _username = "";

        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged();
                (ConnectCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// Contraseña ingresada por el usuario.
        /// </summary>
        private string _password = "";

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
                (ConnectCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// Valida si es posible realizar la conexión, verificando que se hayan ingresado usuario, contraseña y se haya seleccionado una conexión.
        /// </summary>
        /// <returns>True si es posible conectar, de lo contrario False.</returns>
        private bool CanConnect() =>
            SelectedConnection != null && !string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password);

        /// <summary>
        /// Establece la conexión con el servidor SFTP utilizando los datos proporcionados por el usuario.
        /// </summary>
        /// <exception cref="AuthenticationException">Se lanza si no se ingresaron usuario y contraseña.</exception>
        public void Connect()
        {
            if (SelectedConnection == null || string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
            {
                throw new AuthenticationException("Falta usuario y contraseña");
            }
            else
            {
                // Configura la sesión SFTP
                var session = new MySession(SelectedConnection.HostName,
                SelectedConnection.Description,
                Username,
                Password,
                $"/home/{Username}"
            );
                // Conecta al servidor
                _sftpClientManager.Connect(session);

                // Si es exitoso, lanza el evento
                ConnectionSuccessful?.Invoke();

            }
        }
    }
}
