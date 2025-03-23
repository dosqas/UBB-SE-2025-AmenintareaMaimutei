using Project.Models;
using Project.Utils;
using System;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows;
using EquipmentModel = Project.ClassModels.EquipmentModel;

namespace Project.ViewModel
{
    class EquipmentDeleteViewModel : INotifyPropertyChanged
    {
        private readonly EquipmentModel _equipmentModel = new EquipmentModel();

        private int _equipmentID;
        public int EquipmentID
        {
            get => _equipmentID;
            set
            {
                _equipmentID = value;
                OnPropertyChanged(nameof(EquipmentID));
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

        public ICommand DeleteEquipmentCommand { get; }

        public EquipmentDeleteViewModel()
        {
            DeleteEquipmentCommand = new RelayCommand(RemoveEquipment);
        }

        private void RemoveEquipment()
        {
            //if (EquipmentID == Guid.Empty)
            if (EquipmentID == 0)
            {
                ErrorMessage = "No equipment was selected";
                return;
            }

            if (!_equipmentModel.DoesEquipmentExist(EquipmentID))
            {
                ErrorMessage = "EquipmentID doesn't exist in the Equipment Records";
                return;
            }

            // MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete equipment {EquipmentID}?", 
            //                                           "Confirm Deletion", 
            //                                           MessageBoxButton.YesNo, 
            //                                           MessageBoxImage.Warning);

            // if (result == MessageBoxResult.Yes)
            // {
            //     bool success = _equipmentModel.DeleteEquipment(EquipmentID);
            //     ErrorMessage = success ? "Equipment deleted successfully" : "Failed to delete equipment";
            // }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
