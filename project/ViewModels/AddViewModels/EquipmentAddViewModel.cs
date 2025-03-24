using Project.ClassModels;
using Project.Models;
using Project.Utils;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace Project.ViewModels.AddViewModels
{
    internal class EquipmentAddViewModel : INotifyPropertyChanged
    {
        private readonly EquipmentModel _equipmentModel = new EquipmentModel();
        public ObservableCollection<Equipment> Equipments { get; set; } = new ObservableCollection<Equipment>();

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

        private string _type = "";
        public string Type
        {
            get => _type;
            set
            {
                _type = value;
                OnPropertyChanged(nameof(Type));
            }
        }

        private string _specification = "";
        public string Specification
        {
            get => _specification;
            set
            {
                _specification = value;
                OnPropertyChanged(nameof(Specification));
            }
        }

        private int _stock;
        public int Stock
        {
            get => _stock;
            set
            {
                _stock = value;
                OnPropertyChanged(nameof(Stock));
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

        public ICommand SaveEquipmentCommand { get; }

        public EquipmentAddViewModel()
        {
            SaveEquipmentCommand = new RelayCommand(SaveEquipment);
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

        private void SaveEquipment()
        {
            var equipment = new Equipment
            {
                EquipmentID = 0,
                Name = Name,
                Type = Type,
                Specification = Specification,
                Stock = Stock
            };

            if (ValidateEquipment(equipment))
            {
                bool success = _equipmentModel.AddEquipment(equipment);
                ErrorMessage = success ? "Equipment added successfully" : "Failed to add equipment";
                if (success)
                {
                    LoadEquipments();
                }
            }
        }

        private bool ValidateEquipment(Equipment equipment)
        {
            if (string.IsNullOrEmpty(equipment.Name))
            {
                ErrorMessage = "Please enter the name of the equipment.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(equipment.Type))
            {
                ErrorMessage = "Please enter the type of the equipment.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(equipment.Specification))
            {
                ErrorMessage = "Please enter the specifications of the equipment.";
                return false;
            }

            if (equipment.Stock <= 0)
            {
                ErrorMessage = "Please enter a number >0.";
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

