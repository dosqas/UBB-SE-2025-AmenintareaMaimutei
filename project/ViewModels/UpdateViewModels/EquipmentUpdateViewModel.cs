using Project.ClassModels;
using Project.Models;
using Project.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
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
            bool hasErrors = false;
            StringBuilder errorMessages = new StringBuilder();

            foreach (Equipment equipment in Equipments)
            {
                if (!ValidateEquipment(equipment))
                {
                    hasErrors = true;
                    errorMessages.AppendLine("Equipment " + equipment.EquipmentID + ": " + ErrorMessage);
                }
                else
                {
                    bool success = _equipmentModel.UpdateEquipment(equipment);
                    if (!success)
                    {
                        errorMessages.AppendLine("Failed to save changes for equipment: " + equipment.EquipmentID);
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

        private bool ValidateEquipment(Equipment equipment)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(equipment.Name, @"^[a-zA-Z0-9 ]*$")) { ErrorMessage = "Please enter the name of the equipment"; return false; }

            if (!System.Text.RegularExpressions.Regex.IsMatch(equipment.Specification, @"^[a-zA-Z0-9 ,.-]*$")) { ErrorMessage = "Please enter the specifications of the equipment"; return false; }

            if (!System.Text.RegularExpressions.Regex.IsMatch(equipment.Type, @"^[a-zA-Z0-9 ]*$")) { ErrorMessage = "Please enter the type of the equipment"; return false; }
            
            if (equipment.Stock <= 0)
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
