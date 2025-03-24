using Project.ClassModels;
using Project.Models;
using Project.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Project.ViewModels.UpdateViewModels
{
    class EquipmentUpdateViewModel : INotifyPropertyChanged
    {
        private readonly EquipmentModel _equipmentModel = new EquipmentModel();
        public ObservableCollection<Equipment> Equipments { get; set; } = new ObservableCollection<Equipment>();

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
        public EquipmentUpdateViewModel()
        {
            _errorMessage = string.Empty;
            SaveChangesCommand = new RelayCommand(SaveChanges);
            LoadEquipments();
        }

       private void LoadEquipments()
        {
            Equipments.Clear();
            foreach (Equipment equipment in _equipmentModel.GetEquipments())
            {
                Equipments.Add(equipment);
            }
        }

        private void SaveChanges()
        {
            foreach (Equipment equipment in Equipments)
            {
                if (ValidateEquipment(equipment))
                {
                    bool success = _equipmentModel.UpdateEquipment(equipment);
                    ErrorMessage = success ? "Changes saved successfully!" : "Failed to save changes.";
                }
            }
        }

        private bool ValidateEquipment(Equipment equipment)
        {
            if (string.IsNullOrWhiteSpace(equipment.Name))
            {
                ErrorMessage = "Equipment name cannot be empty!";
                return false;
            }
            if (string.IsNullOrWhiteSpace(equipment.Specification))
            {
                ErrorMessage = "Equipment specification cannot be empty!";
                return false;
            }
            if(string.IsNullOrWhiteSpace(equipment.Type))
            {
                ErrorMessage = "Equipment type cannot be empty!";
                return false;
            }
            if(equipment.Stock <= 0)
            {
                ErrorMessage = "Equipment stock must be greater than 0!";
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
