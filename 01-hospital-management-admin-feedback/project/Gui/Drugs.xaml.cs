using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Project.Models;
using System.Collections.ObjectModel;
using Project.ClassModels;
// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Project.Gui
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Drugs : Page
    {
        public ObservableCollection<Drug> DrugsList { get; set; } = new();
        private readonly DrugModel _drugModel = new();
        public Drugs()
        {
            this.InitializeComponent();
            Load();
        }
        private void Load()
        {
            DrugsList.Clear();
            foreach (Drug drug in _drugModel.GetDrugs())
            {
                DrugsList.Add(drug);
            }
        }
    }
}
