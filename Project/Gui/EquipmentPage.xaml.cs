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
using Project.Models;
using System.Collections.ObjectModel;

using Project.ClassModels;
using System.ComponentModel;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Project.Gui
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EquipmentPage : Page
    {
        public ObservableCollection<Equipment> Equipments { get; set; } = new();

        public EquipmentPage()
        {
            this.InitializeComponent();
            LoadEquiptment();
        }

        private void LoadEquiptment()
        {
          Equipments = new ObservableCollection<Equipment>();
          Equipments.Add(new Equipment(Guid.NewGuid(), "Stethoscope", "Medical", "For checking heart rate", 10));
          Equipments.Add(new Equipment(Guid.NewGuid(), "Thermometer", "Medical", "For checking body temperature", 20));
          Equipments.Add(new Equipment(Guid.NewGuid(), "Sphygmomanometer", "Medical", "For checking blood pressure", 15));
        }
    }
}
