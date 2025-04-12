namespace Project.Gui.AddViews
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
    using Project.ViewModels.AddViewModels;
    using Windows.Foundation;
    using Windows.Foundation.Collections;

    /// <summary>
    /// Represents a page for adding a room. This page can be used either standalone or within a Frame navigation.
    /// It binds to the <see cref="RoomAddViewModel"/> to handle the logic and data for adding a new room.
    /// </summary>
    public sealed partial class AddRoomView : Page
    {
        private RoomAddViewModel viewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddRoomView"/> class.
        /// Sets up the view model and data context for the page.
        /// </summary>
        public AddRoomView()
        {
            this.InitializeComponent();
            this.viewModel = new RoomAddViewModel();
            this.DataContext = this.viewModel;
        }
    }
}
