using Project.ClassModels;
using Project.Models;
using Project.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
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
            bool hasErrors = false;
            StringBuilder errorMessages = new StringBuilder();

            foreach (Drug drug in Drugs)
            {
                if (!ValidateDrug(drug))
                {
                    hasErrors = true;
                    errorMessages.AppendLine("Drug " + drug.DrugID + ": " + ErrorMessage);
                }
                else
                {
                    bool success = _drugModel.UpdateDrug(drug);
                    if (!success)
                    {
                        errorMessages.AppendLine("Failed to save changes for drug: " + drug.DrugID);
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

        private bool ValidateDrug(Drug drug)
        {
<<<<<<< Updated upstream
            if (!System.Text.RegularExpressions.Regex.IsMatch(drug.Name, @"^[a-zA-Z0-9 ]*$")) { ErrorMessage = "Name should contain only alphanumeric characters"; return false; }
=======

            if (!System.Text.RegularExpressions.Regex.IsMatch(drug.Name, @"^[a-zA-Z0-9 ]*$"))
            {
                this.ErrorMessage = "Name should contain only alphanumeric characters";
                return false;
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(drug.Administration, @"^[a-zA-Z0-9 ]*$"))
            {
                this.ErrorMessage = "Administration should contain only alphanumeric characters";
                return false;
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(drug.Specification, @"^[a-zA-Z0-9 ,.-]*$"))
            {
                this.ErrorMessage = "Specification should contain only alphanumeric characters";
                return false;
            }
>>>>>>> Stashed changes

            if (!System.Text.RegularExpressions.Regex.IsMatch(drug.Administration, @"^[a-zA-Z0-9 ]*$")) { ErrorMessage = "Administration should contain only alphanumeric characters"; return false; }
            
            if (!System.Text.RegularExpressions.Regex.IsMatch(drug.Specification, @"^[a-zA-Z0-9 ,.-]*$")) { ErrorMessage = "Specification should contain only alphanumeric characters"; return false; }
            
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
