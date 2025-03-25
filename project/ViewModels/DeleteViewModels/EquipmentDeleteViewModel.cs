using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Project.ClassModels;
using Project.Models;
using Project.Utils;

namespace Project.ViewModels.DeleteViewModels
{
    class EquipmentDeleteViewModel : INotifyPropertyChanged
    {
        private readonly EquipmentModel _equipmentModel = new EquipmentModel();
        private ObservableCollection<Equipment> _equipments;
        private int _equipmentID;
        private string _errorMessage;
        private string _messageColor = "Red";

        public ObservableCollection<Equipment> Equipments
        {
            get { return _equipments; }
            set { SetProperty(ref _equipments, value); }
        }

        public int EquipmentID
        {
            get => _equipmentID;
            set
            {
                _equipmentID = value;
                OnPropertyChanged(nameof(EquipmentID));
                OnPropertyChanged(nameof(CanDeleteEquipment));
            }
        }

        public string ErrorMessage
        {
            get => _errorMessage ?? string.Empty;
            set
            {
                _errorMessage = value;
                MessageColor = string.IsNullOrEmpty(value) ? "Red" : value.Contains("successfully") ? "Green" : "Red";
                OnPropertyChanged(nameof(ErrorMessage));
                OnPropertyChanged(nameof(MessageColor));
            }
        }

        public string MessageColor
        {
            get => _messageColor;
            set
            {
                _messageColor = value;
                OnPropertyChanged(nameof(MessageColor));
            }
        }

        public ICommand DeleteEquipmentCommand { get; }

        public bool CanDeleteEquipment => EquipmentID > 0;

        public EquipmentDeleteViewModel()
        {
            // Load equipments for the DataGrid
            Equipments = new ObservableCollection<Equipment>(_equipmentModel.GetEquipments());

            DeleteEquipmentCommand = new RelayCommand(RemoveEquipment);
        }

        private bool CanExecuteDeleteEquipment()
        {
            return EquipmentID > 0;
        }

        private void RemoveEquipment()
        {
            if (EquipmentID == 0)
            {
                ErrorMessage = "No equipment was selected";
                return;
            }

            if (!_equipmentModel.DoesEquipmentExist(EquipmentID))
            {
                ErrorMessage = "EquipmentID doesn't exist in the records";
                return;
            }

            bool success = _equipmentModel.DeleteEquipment(EquipmentID);
            ErrorMessage = success ? "Equipment deleted successfully" : "Failed to delete equipment";

            if (success)
            {
                Equipments = new ObservableCollection<Equipment>(_equipmentModel.GetEquipments());
            }
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
