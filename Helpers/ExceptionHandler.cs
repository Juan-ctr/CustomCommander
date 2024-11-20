using System.Diagnostics;
using System.IO;
using System.Security.Authentication;
using System.Windows;

namespace CustomCommander.Helpers
{
    /// <summary>
    /// Clase encargada de manejar excepciones en la aplicación.
    /// </summary>
    public class ExceptionHandler
    {
        /// <summary>
        /// Maneja una excepción específica, mostrando un mensaje amigable al usuario.
        /// </summary>
        /// <param name="ex">Excepción a manejar.</param>
        public static void HandleException(Exception ex)
        {
            // Log the exception
            LogException(ex);

            string UserMessage = ex switch
            {
                System.UnauthorizedAccessException => "No tiene permisos para realizar esta acción.",
                System.InvalidOperationException => "Operación no válida, verifique los datos y si el problema persiste, avise a soporte.",
                System.TimeoutException => "El servidor no respondió correctamente, avise a soporte.",
                NullReferenceException => "Ocurrió un error interno. Contacta al soporte técnico.",
                DirectoryNotFoundException => "El directorio no existe.",
                AuthenticationException => "Error de autenticación, verifique los datos de usuario y contraseña",
                Renci.SshNet.Common.SshAuthenticationException => "Error de autenticación, verifique los datos de usuario y contraseña",
                InvalidDataException => "Error de configuración, verifique los datos de configuración",
                _ => "Ocurrió un error inesperado, avise a soporte."
            };

            MessageBox.Show(UserMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            if (ex is InvalidDataException)
            {
                Application.Current.Shutdown();
            }
        }

        private static void LogException(Exception exception)
        {
            // Registrar la excepción en un log o sistema de monitoreo
            Debug.WriteLine($"Excepción: {exception.Message}\nStackTrace: {exception.StackTrace}");
        }
    }
}
