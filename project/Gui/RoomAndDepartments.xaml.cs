namespace Project.Gui
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Microsoft.UI.Xaml.Controls;
    using Project.ClassModels;
    using Project.Models;

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RoomAndDepartments : Page
    {
        private readonly RoomModel roomModel = new ();
        private readonly DepartmentModel departmentModel = new ();

        /// <summary>
        /// Initializes a new instance of the <see cref="RoomAndDepartments"/> class.
        /// </summary>
        public RoomAndDepartments()
        {
            this.InitializeComponent();
            this.Load();
        }

        /// <summary>
        /// Gets or Sets Rooms.
        /// </summary>
        public ObservableCollection<Room> Rooms { get; set; } = new ();

        /// <summary>
        /// Gets or Sets Departments.
        /// </summary>
        public ObservableCollection<Department> Departments { get; set; } = new ();

        private void Load()
        {
            this.Departments.Clear();
            foreach (Department department in this.departmentModel.GetDepartments())
            {
                this.Departments.Add(department);
            }

            this.Rooms.Clear();

            List<Room>? rooms = this.roomModel.GetRooms();
            if (rooms != null)
            {
                foreach (Room room in rooms)
                {
                    this.Rooms.Add(room);
                }
            }
        }
    }
}
