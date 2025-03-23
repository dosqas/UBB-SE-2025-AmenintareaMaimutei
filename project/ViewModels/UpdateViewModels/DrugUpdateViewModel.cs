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
    class DrugUpdateViewModel : INotifyPropertyChanged
    {
        private readonly DrugModel _drugModel = new DrugModel();
        public ObservableCollection<Drug> Drugs { get; set; } = new ObservableCollection<Drug>();


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

        public DrugUpdateViewModel()
        {
            _errorMessage = string.Empty;
            SaveChangesCommand = new RelayCommand(SaveChanges);
            LoadDrugs();
        }

        private void LoadDrugs()
        {
            Drugs.Clear();
            foreach (Drug drug in _drugModel.GetDrugs())
            {
                Drugs.Add(drug);
            }
        }

        private void SaveChanges()
        {
            foreach (Drug drug in Drugs)
            {
                if (ValidateDrug(drug))
                {
                    bool success = _drugModel.UpdateDrug(drug);
                    ErrorMessage = success ? "Changes saved successfully!" : "Failed to save changes.";
                }
            }
        }
        private bool ValidateDrug(Drug drug)
        {
            if (string.IsNullOrWhiteSpace(drug.Name))
            {
                ErrorMessage = "Drug name cannot be empty.";
                return false;
            }
            if (string.IsNullOrWhiteSpace(drug.Administration))
            {
                ErrorMessage = "Administration method cannot be empty.";
                return false;
            }
            if (string.IsNullOrWhiteSpace(drug.Specification))
            {
                ErrorMessage = "Specification cannot be empty.";
                return false;
            }
            if (drug.Supply <= 0)
            {
                ErrorMessage = "Supply cannot be negative or zero.";
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
