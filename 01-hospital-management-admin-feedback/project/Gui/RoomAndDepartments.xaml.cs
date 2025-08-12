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
// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Project.Gui
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RoomAndDepartments : Page
    {
        public ObservableCollection<Room> Rooms { get; set; } = new();
        public ObservableCollection<Department> Departments { get; set; } = new();

        private readonly RoomModel _roomModel = new();
        private readonly DepartmentModel _departmentModel = new();
        public RoomAndDepartments()
        {
            this.InitializeComponent();
            Load();
        }

        private void Load()
        {
            Departments.Clear();
            foreach(Department department in _departmentModel.GetDepartments())
            {
                Departments.Add(department);
            }
            Rooms.Clear();
            foreach (Room room in _roomModel.GetRooms())
            {
                Rooms.Add(room);
            }
        }
    }
}
