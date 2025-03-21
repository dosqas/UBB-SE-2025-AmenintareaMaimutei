using Project.Models;
using Project.Utils;
using System;
using System.ComponentModel;
using System.Windows.Input;
using DrugModel = Project.ClassModels.DrugModel;

namespace Project.ViewModels.AddViewModels
{
    internal class DrugAddViewModel : INotifyPropertyChanged
    {
        private readonly DrugModel _drugModel = new DrugModel();

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

        private string _administration = "";
        public string Administration
        {
            get => _administration;
            set
            {
                _administration = value;
                OnPropertyChanged(nameof(Administration));
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

        private int _supply;
        public int Supply
        {
            get => _supply;
            set
            {
                _supply = value;
                OnPropertyChanged(nameof(Supply));
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

        public ICommand SaveDrugCommand { get; }

        public DrugAddViewModel()
        {
            SaveDrugCommand = new RelayCommand(SaveDrug);
        }

        private void SaveDrug()
        {
            var drug = new Drug
            {
                DrugID = Guid.NewGuid(),
                Name = Name,
                Administration = Administration,
                Specification = Specification,
                Supply = Supply
            };

            if (ValidateDrug(drug))
            {
                bool success = _drugModel.AddDrug(drug);
                ErrorMessage = success ? "Drug added successfully" : "Failed to add drug";
            }
        }

        private bool ValidateDrug(Drug drug)
        {
            if (string.IsNullOrWhiteSpace(drug.Name))
            {
                ErrorMessage = "Please enter the name of the drug.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(drug.Administration))
            {
                ErrorMessage = "Please enter the administration of the drug.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(drug.Specification))
            {
                ErrorMessage = "Please enter the specifications of the drug.";
                return false;
            }

            if (drug.Supply <= 0)
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
    