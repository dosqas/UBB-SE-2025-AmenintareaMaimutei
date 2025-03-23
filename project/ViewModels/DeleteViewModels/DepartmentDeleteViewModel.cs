
using System;
using System.Collections.Generic;
using Project.Utils;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows;
using DepartmentModel = Project.ClassModels.DepartmentModel;

namespace Project.ViewModel
{
    class DepartmentDeleteViewModel : INotifyPropertyChanged
    {
        private readonly DepartmentModel _departmentModel = new DepartmentModel();

        private int _departmentID;
        public int DepartmentID
        {
            get => _departmentID;
            set
            {
                _departmentID = value;
                OnPropertyChanged(nameof(DepartmentID));
            }
        }

        private string? _errorMessage;


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

        public DepartmentDeleteViewModel()
        {
            DeleteDepartmentCommand = new RelayCommand(RemoveDepartment);
        }

        private void RemoveDepartment()
        {
            //if (DepartmentID == Guid.Empty)
            if (DepartmentID == 0)
            {
                ErrorMessage = "No department was selected";
                return;
            }

            if (!_departmentModel.DoesDepartmentExist(DepartmentID))
            {
                ErrorMessage = "DepartmentID doesn't exist in the Department Records";
                return;
            }


            // MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete department {DepartmentID}?",
            //                                           "Confirm Deletion",
            //                                           MessageBoxButton.YesNo,
            //                                           MessageBoxImage.Warning);

            // if (result == MessageBoxResult.Yes)
            // {
            //     bool success = _departmentModel.DeleteDepartment(DepartmentID);
            //     ErrorMessage = success ? "Department deleted successfully" : "Failed to delete department";
            // }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
