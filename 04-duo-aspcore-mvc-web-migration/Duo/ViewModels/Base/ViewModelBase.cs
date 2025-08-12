using System.Collections.Generic;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Duo.ViewModels.Base
{
    /// <summary>
    /// A base class for all ViewModels that provides property change notifications and error messaging.
    /// </summary>
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        /// <summary>
        /// Raised when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Raised when the view should display an error message.
        /// </summary>
        public event EventHandler<(string Title, string Message)>? ShowErrorMessageRequested;

        /// <summary>
        /// Raises an error message event with a title and message to be handled by the view.
        /// </summary>
        public virtual void RaiseErrorMessage(string title, string message)
        {
            ShowErrorMessageRequested?.Invoke(this, (title, message));
        }

        /// <summary>
        /// Triggers a property changed notification.
        /// </summary>
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Updates a property and notifies the UI only if the value has changed.
        /// </summary>
        protected virtual bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(storage, value))
            {
                return false;
            }

            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        /// <summary>
        /// Updates a property and runs a callback if the value has changed.
        /// </summary>
        protected virtual bool SetProperty<T>(ref T storage, T value, Action onChanged, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(storage, value))
            {
                return false;
            }

            storage = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
