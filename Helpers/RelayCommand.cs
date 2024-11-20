using System.Windows.Input;

namespace CustomCommander.Helpers
{
    /// <summary>
    /// Implementación genérica de la interfaz ICommand para manejar comandos en la aplicación.
    /// Permite definir acciones y condiciones para ejecutar comandos en un entorno MVVM.
    /// </summary>
    public class RelayCommand : ICommand
    {
        /// <summary>
        /// Acción que se ejecutará cuando el comando sea invocado.
        /// </summary>
        private readonly Action _execute;

        /// <summary>
        /// Función que determina si el comando puede ejecutarse.
        /// </summary>
        private readonly Func<bool> _canExecute;

        /// <summary>
        /// Evento que se dispara cuando cambia la capacidad del comando de ejecutarse.
        /// </summary>
        public event EventHandler? CanExecuteChanged;

        /// <summary>
        /// Constructor que inicializa el comando con una acción a ejecutar
        /// y una función opcional para validar si puede ejecutarse.
        /// </summary>
        /// <param name="execute">Acción que se ejecutará al invocar el comando.</param>
        /// <param name="canExecute">
        /// Función que determina si el comando puede ejecutarse. Si es null, el comando siempre estará habilitado.
        /// </param>
        public RelayCommand(Action execute, Func<bool> canExecute = null)
        {

            _execute = execute ?? throw new ArgumentNullException(nameof(execute));


            _canExecute = canExecute;
        }

        /// <summary>
        /// Determina si el comando puede ejecutarse.
        /// </summary>
        /// <param name="parameter">Parámetro opcional que no se utiliza en esta implementación.</param>
        /// <returns>True si el comando puede ejecutarse, False en caso contrario.</returns>
        public bool CanExecute(object parameter) => _canExecute == null || _canExecute();

        /// <summary>
        /// Ejecuta la acción asociada al comando.
        /// </summary>
        /// <param name="parameter">Parámetro opcional que no se utiliza en esta implementación.</param>
        public void Execute(object parameter) => _execute();

        /// <summary>
        /// Notifica a los suscriptores que el estado del comando ha cambiado,
        /// lo que obliga a reevaluar si el comando puede ejecutarse.
        /// </summary>
        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
