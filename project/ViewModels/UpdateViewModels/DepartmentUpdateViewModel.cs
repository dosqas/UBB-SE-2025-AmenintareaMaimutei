using Project.ClassModels;
using Project.Models;
using Project.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
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
            foreach (Department department in Departments)
            {
                if (ValidateDepartment(department))
                {
                    bool success = _departmentModel.UpdateDepartment(department);
                    ErrorMessage = success ? "Changes saved successfully!" : "Failed to save changes.";
                }
            }
        }
        private bool ValidateDepartment(Department department)
        {
            if (string.IsNullOrWhiteSpace(department.Name))
            {
                ErrorMessage = "Department name cannot be empty.";
                return false;
            }
            return true;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
