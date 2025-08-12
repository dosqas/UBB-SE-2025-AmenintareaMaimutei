using Microsoft.UI.Xaml.Controls;

namespace Duo.Views.Components
{
    public sealed partial class DialogContent : UserControl
    {
        public string ContentText { get; set; }

        public DialogContent()
        {
            this.InitializeComponent();
        }
    }
}
