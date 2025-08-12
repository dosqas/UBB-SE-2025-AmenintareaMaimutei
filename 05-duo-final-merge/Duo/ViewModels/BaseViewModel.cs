using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Duo.ViewModels
{
    /// <summary>
    /// A base class for view models that supports property change notifications.
    /// </summary>
    public partial class BaseViewModel : IBaseViewModel
    {
        /// <summary>
        /// Notifies the UI when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Raised when the View should display an error message.
        /// </summary>
        public event EventHandler<(string Title, string Message)>? ShowErrorMessageRequested;

        /// <summary>
        /// Triggers an error message to be displayed in the UI.
        /// </summary>
        public virtual void RaiseErrorMessage(string title, string message)
        {
            ShowErrorMessageRequested?.Invoke(this, (title, message));
        }

        /// <summary>
        /// Triggers a notification that a property has changed.
        /// </summary>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Updates a field and notifies the UI only if the value has changed.
        /// </summary>
        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
            {
                return false;
            }

            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        /// <summary>
        /// Triggers a property changed event. Used by interface implementation.
        /// </summary>
        void IBaseViewModel.OnPropertyChanged(string propertyName)
        {
            OnPropertyChanged(propertyName);
        }

        /// <summary>
        /// Placeholder for setting a property from the interface. Not implemented yet.
        /// </summary>
        bool IBaseViewModel.SetProperty<T>(ref T field, T value, string propertyName)
        {
            throw new System.NotImplementedException();
        }
    }
}
