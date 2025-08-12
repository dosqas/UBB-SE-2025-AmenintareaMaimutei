using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;

namespace Duo.Views.Components
{
    public sealed partial class CreatePostButton : UserControl
    {
        // Define a public event that other components can subscribe to
        public event EventHandler<RoutedEventArgs> CreatePostRequested;

        public CreatePostButton()
        {
            this.InitializeComponent();
        }

        private void PostButton_Click(object sender, RoutedEventArgs e)
        {
            // Raise the event to notify subscribers that the button was clicked
            CreatePostRequested?.Invoke(this, e);
        }
    }
}