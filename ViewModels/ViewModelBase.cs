using System.ComponentModel;
using System.Runtime.CompilerServices;
namespace CustomCommander.ViewModels
{
    /// <summary>
    /// Clase base para los ViewModels que implementa el patrón MVVM (Model-View-ViewModel).
    /// Proporciona soporte para notificar a la vista sobre cambios en las propiedades del modelo de vista.
    /// </summary>
    public class ViewModelBase : INotifyPropertyChanged
    {
        /// <summary>
        /// Evento que se dispara cuando cambia el valor de una propiedad.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Método para invocar el evento <see cref="PropertyChanged"/>.
        /// Esto notifica a los suscriptores (normalmente la vista) que una propiedad ha cambiado.
        /// </summary>
        /// <param name="propertyName">
        /// Nombre de la propiedad que cambió. Se establece automáticamente si se llama dentro de la propiedad,
        /// gracias al atributo <see cref="CallerMemberName"/>.
        /// </param>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
