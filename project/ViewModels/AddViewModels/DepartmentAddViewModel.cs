using Project.Models;
using Project.Utils;
using System;
using System.ComponentModel;
using System.Windows.Input;
using DepartmentModel = Project.ClassModels.DepartmentModel;

namespace Project.ViewModels
{
    class DepartmentAddViewModel : INotifyPropertyChanged
    {
        private readonly DepartmentModel _departmentModel = new DepartmentModel();

        private string _name = "";
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        private string _errorMessage = "";
        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }

        public ICommand SaveDepartmentCommand { get; }

        public DepartmentAddViewModel()
        {
            SaveDepartmentCommand = new RelayCommand(SaveDepartment);
        }

        private void SaveDepartment()
        {
            var department = new Department
            {
                //DepartmentID = Guid.NewGuid(),
                DepartmentID = 0,
                Name = Name
            };

            if (ValidateDepartment(department))
            {
                bool success = _departmentModel.AddDepartment(department);
                ErrorMessage = success ? "Department added successfully" : "Failed to add department";
            }
        }

        private bool ValidateDepartment(Department department)
        {
            if (string.IsNullOrWhiteSpace(department.Name))
            {
                ErrorMessage = "Please enter the name of the department.";
                return false;
            }
            return true;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
