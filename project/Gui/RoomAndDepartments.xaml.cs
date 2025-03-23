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

        public RoomAndDepartments()
        {
            this.InitializeComponent();
            Load();
        }

        private void Load()
        {
            Rooms = new ObservableCollection<Room>();
            Departments = new ObservableCollection<Department>();
            //Departments.Add(new Department(Guid.NewGuid(), "Cardiology"));
            //Departments.Add(new Department(Guid.NewGuid(), "Neurology"));

            Departments.Add(new Department(1, "Cardiology"));
            Departments.Add(new Department(2, "Neurology"));

            //Rooms.Add(new Room(Guid.NewGuid(), 30, Departments[0].DepartmentID, Guid.NewGuid()));
            //Rooms.Add(new Room(Guid.NewGuid(), 25, Departments[1].DepartmentID, Guid.NewGuid()));

            Rooms.Add(new Room(1, 30, Departments[0].DepartmentID, 1));
            Rooms.Add(new Room(2, 25, Departments[1].DepartmentID, 2));
        }
    }
}
