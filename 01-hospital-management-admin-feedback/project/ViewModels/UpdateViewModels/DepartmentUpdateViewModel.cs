using Project.ClassModels;
using Project.Models;
using Project.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Project.ViewModels.UpdateViewModels
{
    class DepartmentUpdateViewModel : INotifyPropertyChanged
    {
        private readonly DepartmentModel _departmentModel = new DepartmentModel();
        public ObservableCollection<Department> Departments { get; set; } = new ObservableCollection<Department>();

        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }
        public ICommand SaveChangesCommand { get; }
        public DepartmentUpdateViewModel()
        {
            _errorMessage = string.Empty;
            SaveChangesCommand = new RelayCommand(SaveChanges);
            LoadDepartments();
        }

        private void LoadDepartments()
        {
            Departments.Clear();
            foreach (Department department in _departmentModel.GetDepartments())
            {
                Departments.Add(department);
            }
        }
        private void SaveChanges()
        {
            bool hasErrors = false;
            StringBuilder errorMessages = new StringBuilder();

            foreach (Department department in Departments)
            {
                if (!ValidateDepartment(department))
                {
                    hasErrors = true;
                    errorMessages.AppendLine("Department " + department.DepartmentID + ": " + ErrorMessage);
                }
                else
                {
                    bool success = _departmentModel.UpdateDepartment(department);
                    if (!success)
                    {
                        errorMessages.AppendLine("Failed to save changes for department: " + department.DepartmentID);
                        hasErrors = true;
                    }
                }
            }
            if (hasErrors)
            {
                ErrorMessage = errorMessages.ToString();
            }
            else
            {
                ErrorMessage = "Changes saved successfully";
            }
        }
        private bool ValidateDepartment(Department department)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(department.Name, @"^[a-zA-Z0-9 ]*$")) { ErrorMessage = "Department Name should contain only alphanumeric characters"; return false; }
            return true;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
