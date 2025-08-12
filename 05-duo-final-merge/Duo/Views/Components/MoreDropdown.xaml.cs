using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Threading.Tasks;

namespace Duo.Views.Components
{
    public sealed partial class MoreDropdown : UserControl
    {

        public event EventHandler<RoutedEventArgs> EditClicked;
        public event EventHandler<RoutedEventArgs> DeleteClicked;

        public MoreDropdown()
        {
            this.InitializeComponent();
        }

        private void EditMenuItem_Click(object sender, RoutedEventArgs e)
        {
            EditClicked?.Invoke(this, e);
        }

        private void DeleteMenuItem_Click(object sender, RoutedEventArgs e)
        {
            DeleteClicked?.Invoke(this, e);
        }
    }
}
