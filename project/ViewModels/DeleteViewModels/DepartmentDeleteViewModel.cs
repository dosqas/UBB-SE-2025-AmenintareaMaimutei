using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Project.ClassModels;
using Project.Models;
using Project.Utils;

namespace Project.ViewModels.DeleteViewModels
{
    class DepartmentDeleteViewModel : INotifyPropertyChanged
    {
        private readonly DepartmentModel _departmentModel = new DepartmentModel();
        private ObservableCollection<Department> _departments;
        private int _departmentID;
        private string _errorMessage;

        public ObservableCollection<Department> Departments
        {
            get { return _departments; }
            set { SetProperty(ref _departments, value); }
        }

        public int DepartmentID
        {
            get => _departmentID;
            set
            {
                _departmentID = value;
                OnPropertyChanged(nameof(DepartmentID));
                OnPropertyChanged(nameof(CanDeleteDepartment)); 
            }
        }

        public string ErrorMessage
        {
            get => _errorMessage ?? string.Empty;
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }

        public ICommand DeleteDepartmentCommand { get; }

        public bool CanDeleteDepartment => DepartmentID > 0;

        public DepartmentDeleteViewModel()
        {
            // Load departments for the DataGrid
            Departments = new ObservableCollection<Department>(_departmentModel.GetDepartments());

            DeleteDepartmentCommand = new RelayCommand(RemoveDepartment, CanExecuteDeleteDepartment);
        }

        private bool CanExecuteDeleteDepartment()
        {
            return DepartmentID > 0; 
        }

        private void RemoveDepartment()
        {
            if (DepartmentID == 0)
            {
                ErrorMessage = "No department was selected";
                return;
            }

            if (!_departmentModel.DoesDepartmentExist(DepartmentID))
            {
                ErrorMessage = "DepartmentID doesn't exist in the records";
                return;
            }

            bool success = _departmentModel.DeleteDepartment(DepartmentID);
            ErrorMessage = success ? "Department deleted successfully" : "Failed to delete department";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void SetProperty<T>(ref T field, T value, [System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            if (!EqualityComparer<T>.Default.Equals(field, value))
            {
                field = value;
                OnPropertyChanged(propertyName);
            }
        }
    }
}
