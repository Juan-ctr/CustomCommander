using CustomCommander.ViewModels;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;

namespace CustomCommander.Views
{
    /// <summary>
    /// Ventana de inicio de sesión que permite al usuario conectarse a un servidor SFTP.
    /// </summary>
    public partial class LogWindow : Window
    {
        private readonly LogViewModel _viewModel;

        /// <summary>
        /// Constructor de la ventana de inicio de sesión.
        /// Inicializa los componentes y establece el contexto de datos para enlazar con el ViewModel.
        /// </summary>
        /// <param name="viewModel">El ViewModel asociado a esta ventana.</param>
        public LogWindow(LogViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;

            // Suscribirse al evento de conexión exitosa
            _viewModel.ConnectionSuccessful += OnConnectionSuccessful;
        }

        /// <summary>
        /// Maneja el evento de cambio de contraseña en el PasswordBox.
        /// Actualiza la contraseña en el ViewModel con el valor ingresado por el usuario.
        /// </summary>
        /// <param name="sender">El PasswordBox que disparó el evento.</param>
        /// <param name="e">Datos del evento.</param>
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is LogViewModel viewModel)
            {
                viewModel.Password = ((PasswordBox)sender).Password;
            }
        }

        /// <summary>
        /// Maneja el evento que ocurre cuando la conexión al servidor SFTP es exitosa.
        /// Abre la ventana principal de la aplicación y cierra la ventana de inicio de sesión.
        /// </summary>
        private void OnConnectionSuccessful()
        {
            // Crear y mostrar la ventana principal
            var mainWindow = new MainWindow(App.ServiceProvider.GetRequiredService<MainViewModel>());
            mainWindow.Show();

            // Cerrar la ventana de inicio de sesión
            this.Close();
        }

        /// <summary>
        /// Sobrescribe el evento OnClosing para realizar acciones personalizadas antes de cerrar la ventana.
        /// </summary>
        /// <param name="e">Argumentos del evento de cierre.</param>
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
        }
    }
}
