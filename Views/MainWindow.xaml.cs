using System.Windows;
using CustomCommander.ViewModels;
using Renci.SshNet.Sftp;
using System.IO;
using System.Windows.Controls;
using System.Windows.Input;
using System.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using CustomCommander.Views;

namespace CustomCommander.Views
{
    /// <summary>
    /// Ventana principal de la aplicación que permite gestionar directorios locales y remotos.
    /// Incluye funcionalidades como navegar entre carpetas, copiar archivos mediante drag-and-drop y desconexión.
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainViewModel _viewModel;

        /// <summary>
        /// Constructor de la ventana principal. 
        /// Inicializa los componentes y establece el contexto de datos para enlazar con el ViewModel.
        /// </summary>
        /// <param name="viewModel">El ViewModel asociado a esta ventana.</param>
        public MainWindow(MainViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;

            // Suscribirse al evento de desconexión
            _viewModel.DisconnectMain += OnDisconnectMain;
        }

        /// <summary>
        /// Maneja el evento de desconexión, cerrando la ventana principal y mostrando la ventana de inicio de sesión (LogWindow).
        /// </summary>
        private void OnDisconnectMain()
        {
            // Crear y mostrar la ventana de inicio de sesión
            var logWindow = App.ServiceProvider.GetRequiredService<LogWindow>();

            // Configura LogWindow como la ventana principal
            Application.Current.MainWindow = logWindow;

            // Muestra LogWindow
            logWindow.Show();

            // Cierra MainWindow
            this.Close();
        }

        /// <summary>
        /// Método para manejar el evento de doble clic en la lista de directorios remotos, si es una carpeta, navega a ella.
        /// Si es un archivo, no hace nada, se desea que la aplicación solo pueda copiar archivos, no abrirlos ni modificarlos.
        /// </summary>
        /// <param name="sender">El control que disparó el evento.</param>
        /// <param name="e">Datos del evento.</param>
        private void RemoteDirectoryList_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (RemoteDirectoryList.SelectedItem is ISftpFile selectedItem && selectedItem.IsDirectory)
            {
                // Llama a la función de navegación en el ViewModel usando la ruta completa
                _viewModel.NavigateToRemoteDirectory(selectedItem.FullName);
            }
        }

        /// <summary>
        /// Método para manejar el evento de doble clic en la lista de directorios locales, si es una carpeta, navega a ella.
        /// Si es un archivo, no hace nada, se desea que la aplicación solo pueda copiar archivos, no abrirlos ni modificarlos.
        /// </summary>
        /// <param name="sender">El control que disparó el evento.</param>
        /// <param name="e">Datos del evento.</param>
        private void LocalDirectoryList_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (LocalDirectoryList.SelectedItem is FileSystemInfo selectedItem && (selectedItem.Attributes & FileAttributes.Directory) == FileAttributes.Directory)
            {
                // Llama a la función de navegación en el ViewModel usando la ruta completa
                _viewModel.NavigateToLocalDirectory(selectedItem.FullName);
            }
        }

        /// <summary>
        /// Maneja el evento de arrastrar archivos desde la lista de directorios remotos.
        /// Solo permite arrastrar archivos, no carpetas.
        /// </summary>
        private void RemoteDirectoryList_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed &&
                sender is ListView listView && listView.SelectedItem is ISftpFile selectedItem &&
                !selectedItem.IsDirectory)
            {
                DragDrop.DoDragDrop(listView, selectedItem.FullName, DragDropEffects.Copy);
            }
        }

        /// <summary>
        /// Maneja el evento de arrastrar archivos desde la lista de directorios locales.
        /// Solo permite arrastrar archivos, no carpetas.
        /// </summary>
        private void LocalDirectoryList_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed &&
                sender is ListView listView && listView.SelectedItem is FileSystemInfo selectedItem &&
                (selectedItem.Attributes & FileAttributes.Directory) != FileAttributes.Directory)
            {
                string fullName = selectedItem.FullName?.Replace('\\', '/');
                DragDrop.DoDragDrop(listView, fullName, DragDropEffects.Copy);

            }
        }

        /// <summary>
        /// Maneja el evento de soltar un archivo en la lista de directorios remotos.
        /// Copia el archivo desde el sistema local al servidor remoto.
        /// </summary>
        private void RemoteDirectoryList_Drop(object sender, DragEventArgs e)
        {
            //borrar luego las siguientes dos líneas
            //var formats = e.Data.GetFormats();
            //Debug.WriteLine("Formatos disponibles: " + string.Join(", ", formats));

            if (e.Data.GetData(typeof(string)) is string localFilePath)
            {

                //copia archivo del sistema local al servidor remoto
                var destinationPath = Path.Combine(_viewModel.CurrentRemotePath, Path.GetFileName(localFilePath));
                destinationPath = destinationPath?.Replace('\\', '/');

                // Verifica si el archivo ya está en el mismo directorio
                if (string.Equals(localFilePath, destinationPath, StringComparison.OrdinalIgnoreCase))
                {
                    return;
                }

                _viewModel.CopyLocalToRemote(localFilePath, destinationPath);
            }
        }

        /// <summary>
        /// Maneja el evento de soltar un archivo en la lista de directorios locales.
        /// Copia el archivo desde el servidor remoto al sistema local.
        /// </summary>
        private void LocalDirectoryList_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetData(typeof(string)) is string remoteFilePath)
            {
                // Construye la ruta local de destino
                var destinationPath = Path.Combine(_viewModel.CurrentLocalPath, Path.GetFileName(remoteFilePath));
                //Para la validación de la ruta de destino, se reemplaza el caracter '\' por '/'
                var controlDestinationPath = destinationPath?.Replace('\\', '/');
                // Verifica si el archivo ya está en el mismo directorio
                if (string.Equals(remoteFilePath, controlDestinationPath, StringComparison.OrdinalIgnoreCase))
                {
                    return;
                }

                _viewModel.CopyRemoteToLocal(remoteFilePath, destinationPath);
            }
        }

        /// <summary>
        /// Sobrescribe el evento OnClosing para realizar acciones personalizadas al cerrar la ventana.
        /// </summary>
        /// <param name="e">Argumentos del evento de cierre.</param>
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            // Liberar recursos si es necesario
        }
    }
}
