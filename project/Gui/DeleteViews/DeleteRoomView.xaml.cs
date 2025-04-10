namespace Project.Gui.DeleteViews
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices.WindowsRuntime;
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using Microsoft.UI.Xaml.Controls.Primitives;
    using Microsoft.UI.Xaml.Data;
    using Microsoft.UI.Xaml.Input;
    using Microsoft.UI.Xaml.Media;
    using Microsoft.UI.Xaml.Navigation;
    using Project.ViewModel;
    using Project.ViewModels.DeleteViewModels;
    using Windows.Foundation;
    using Windows.Foundation.Collections;

    /// <summary>
    /// Represents a page for deleting a room. This page can be used either standalone or within a Frame navigation.
    /// It binds to the <see cref="RoomDeleteViewModel"/> to handle the logic and data for deleting a room.
    /// </summary>
    public sealed partial class DeleteRoomView : Page
    {
        private RoomDeleteViewModel viewmodel;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteRoomView"/> class.
        /// Sets up the view model and data context for the page.
        /// </summary>
        public DeleteRoomView()
        {
            this.InitializeComponent();
            this.viewmodel = new RoomDeleteViewModel();
            this.DataContext = this.viewmodel;
        }
    }
}
