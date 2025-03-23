using Project.Models;
using Project.Utils;
using System;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows;
using DrugModel = Project.ClassModels.DrugModel;

namespace Project.ViewModel
{
    class DrugDeleteViewModel : INotifyPropertyChanged
    {
        private readonly DrugModel _drugModel = new DrugModel();

        private int _drugID;
        public int DrugID
        {
            get => _drugID;
            set
            {
                _drugID = value;
                OnPropertyChanged(nameof(DrugID));
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

        public ICommand DeleteDrugCommand { get; }

        public DrugDeleteViewModel()
        {
            DeleteDrugCommand = new RelayCommand(RemoveDrug);
        }

        private void RemoveDrug()
        {
            if (DrugID == Guid.Empty)
            {
                ErrorMessage = "No drug was selected";
                return;
            }

            if (!_drugModel.DoesDrugExist(DrugID))
            {
                ErrorMessage = "DrugID doesn't exist in the Drug Records";
                return;
            }

            // MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete drug {DrugID}?", 
            //                                           "Confirm Deletion", 
            //                                           MessageBoxButton.YesNo, 
            //                                           MessageBoxImage.Warning);

            // if (result == MessageBoxResult.Yes)
            // {
            //     bool success = _drugModel.DeleteDrug(DrugID);
            //     ErrorMessage = success ? "Drug deleted successfully" : "Failed to delete drug";
            // }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
