namespace Project.ViewModels.AddViewModels
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows.Input;
    using Project.ClassModels;
    using Project.Models;
    using Project.Utils;

    /// <summary>
    /// ViewModel for adding departments.
    /// </summary>
    public class DepartmentAddViewModel : INotifyPropertyChanged
    {
        private readonly DepartmentModel departmentModel = new DepartmentModel();
        private string name = string.Empty;
        private string errorMessage = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="DepartmentAddViewModel"/> class.
        /// </summary>
        public DepartmentAddViewModel()
        {
            this.SaveDepartmentCommand = new RelayCommand(this.SaveDepartment);
            this.LoadDepartments();
        }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Gets or sets the collection of departments.
        /// </summary>
        public ObservableCollection<Department> Departments { get; set; } = new ObservableCollection<Department>();

        /// <summary>
        /// Gets or sets the name of the department.
        /// </summary>
        public string Name
        {
            get => this.name;
            set
            {
                this.name = value;
                this.OnPropertyChanged(nameof(this.Name));
            }
        }

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        public string ErrorMessage
        {
            get => this.errorMessage;
            set
            {
                this.errorMessage = value;
                this.OnPropertyChanged(nameof(this.ErrorMessage));
            }
        }

        /// <summary>
        /// Gets the command to save the department.
        /// </summary>
        public ICommand SaveDepartmentCommand { get; }

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="propertyName">The name of the property that changed.</param>
        protected void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Loads the departments from the database.
        /// </summary>
        private void LoadDepartments()
        {
            this.Departments.Clear();
            foreach (Department department in this.departmentModel.GetDepartments())
            {
                this.Departments.Add(department);
            }
        }

        /// <summary>
        /// Saves the department to the database.
        /// </summary>
        private void SaveDepartment()
        {
            var department = new Department
            {
                DepartmentID = 0,
                Name = this.Name,
            };

            if (this.ValidateDepartment(department))
            {
                bool success = this.departmentModel.AddDepartment(department);
                this.ErrorMessage = success ? "Department added successfully" : "Failed to add department";
                if (success)
                {
                    this.LoadDepartments();
                }
            }
        }

        /// <summary>
        /// Validates the department.
        /// </summary>
        /// <param name="department">The department to validate.</param>
        /// <returns>True if the department is valid, otherwise false.</returns>
        private bool ValidateDepartment(Department department)
        {
            if (string.IsNullOrWhiteSpace(department.Name))
            {
                this.ErrorMessage = "Please enter the name of the department.";
                return false;
            }

            return true;
        }
    }
}