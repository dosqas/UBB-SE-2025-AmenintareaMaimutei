﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Duo.ViewModels.Base;

namespace Duo.ViewModels
{
    internal partial class AdminBaseViewModel : ViewModelBase
    {
        public event EventHandler? RequestGoBack;
        public event EventHandler<(string Title, string Message)>? ShowErrorMessageRequested;

        public AdminBaseViewModel()
        {
        }

        public void GoBack()
        {
            RequestGoBack?.Invoke(this, EventArgs.Empty);
        }
        public virtual void RaiseErrorMessage(string title, string message)
        {
            ShowErrorMessageRequested?.Invoke(this, (title, message));
        }
    }
}
