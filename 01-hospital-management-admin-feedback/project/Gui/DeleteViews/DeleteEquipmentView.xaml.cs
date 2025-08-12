using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Project.ViewModel;
using Project.ViewModels.DeleteViewModels;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Project.Gui.DeleteViews
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DeleteEquipmentView : Page
    {
        private EquipmentDeleteViewModel _viewModel = new EquipmentDeleteViewModel();
        public DeleteEquipmentView()
        {
            this.InitializeComponent();
            _viewModel = new EquipmentDeleteViewModel();
            this.DataContext = _viewModel;


        }
    }
}
