using CustomCommander.Helpers;
using CustomCommander.Interfaces;
using CustomCommander.Services;
using CustomCommander.ViewModels;
using CustomCommander.Views;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.Windows;

namespace CustomCommander
{
    /// <summary>
    /// Clase principal de la aplicación que configura los servicios y maneja los eventos de inicio y excepciones globales.
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Proveedor de servicios utilizado para inyección de dependencias.
        /// </summary>
        public static IServiceProvider ServiceProvider { get; private set; }

        /// <summary>
        /// Método ejecutado al iniciar la aplicación.
        /// </summary>
        /// <param name="e">Argumentos de inicio.</param>
        protected override void OnStartup(StartupEventArgs e)
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            // Construir el proveedor de servicios
            ServiceProvider = serviceCollection.BuildServiceProvider();

            // Manejo de excepciones del Dispatcher (Hilo principal de la UI)
            DispatcherUnhandledException += App_DispatcherUnhandledException;

            // Manejo de excepciones no controladas a nivel de dominio de la aplicación
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            // Crear y mostrar la ventana inicial (LogWindow)
            var mainWindow = ServiceProvider.GetRequiredService<LogWindow>();
            mainWindow.Show();
        }

        /// <summary>
        /// Configura los servicios necesarios para la aplicación.
        /// </summary>
        /// <param name="services">Colección de servicios para la configuración.</param>
        private void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ISftpClientManager, SftpClientManager>();  // Gestión de SFTP como singleton
            services.AddSingleton<ILocalClientManager, LocalClientManager>(); // Gestión de archivos locales como singleton
            services.AddSingleton<IFileBrowser, FileBrowser>(); // Servicio de navegación de archivos
            services.AddSingleton<LogViewModel>(); // ViewModel de la ventana de login
            services.AddSingleton<MainViewModel>(); // ViewModel de la ventana principal
            services.AddTransient<MainWindow>(); // Ventana principal
            services.AddTransient<LogWindow>(); // Ventana de login
        }

        /// <summary>
        /// Maneja las excepciones no controladas del Dispatcher (hilo principal de la UI).
        /// </summary>
        /// <param name="sender">Objeto que genera el evento.</param>
        /// <param name="e">Detalles del evento de excepción.</param>
        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            // Manejar la excepción con un controlador específico
            ExceptionHandler.HandleException(e.Exception);

            // Marcar como manejada para evitar cierre de la aplicación
            e.Handled = true;
        }

        /// <summary>
        /// Maneja las excepciones no controladas a nivel de dominio de la aplicación.
        /// </summary>
        /// <param name="sender">Objeto que genera el evento.</param>
        /// <param name="e">Detalles del evento de excepción.</param>
        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception ex)
            {
                // Manejar la excepción con un controlador específico
                ExceptionHandler.HandleException(ex);
            }
        }
    }

}
