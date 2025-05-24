using System;
using Duo.ViewModels.Base;

namespace Duo.ViewModels
{
    /// <summary>
    /// A ViewModel base class for admin-specific functionality like navigation and error handling.
    /// </summary>
    internal partial class AdminBaseViewModel : ViewModelBase
    {
        /// <summary>
        /// Raised when the view should navigate back.
        /// </summary>
        public event EventHandler? RequestGoBack;

        public AdminBaseViewModel()
        {
        }

        /// <summary>
        /// Triggers the request to navigate back.
        /// </summary>
        public void GoBack()
        {
            RequestGoBack?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Optional override for customized error reporting.
        /// </summary>
        public override void RaiseErrorMessage(string title, string message)
        {
            base.RaiseErrorMessage(title, message);
        }
    }
}
