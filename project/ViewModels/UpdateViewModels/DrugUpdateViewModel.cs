// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DrugUpdateViewModel.cs" company="YourCompany">
//   Copyright (c) YourCompany. All rights reserved.
// </copyright>
// <summary>
//   ViewModel responsible for validating and updating drug records.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Project.ViewModels.UpdateViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Numerics;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using Project.ClassModels;
    using Project.Models;
    using Project.Utils;

    /// <summary>
    /// ViewModel used for managing updates to drug records.
    /// </summary>
    public class DrugUpdateViewModel : INotifyPropertyChanged
    {
        private readonly DrugModel drugModel = new DrugModel();

        private string errorMessage = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="DrugUpdateViewModel"/> class.
        /// </summary>
        public DrugUpdateViewModel()
        {
            this.SaveChangesCommand = new RelayCommand(this.SaveChanges);
            this.LoadDrugs();
        }

        /// <summary>
        /// Gets or sets the error message shown in the UI.
        /// </summary>
        public string ErrorMessage
        {
            get => this.errorMessage;
            set
            {
                this.errorMessage = value;
                this.OnPropertyChanged(nameof(this.ErrorMessage));
            }
        }

        /// <summary>
        /// Gets the command that saves changes to drugs.
        /// </summary>
        public ICommand SaveChangesCommand { get; }

        /// <summary>
        /// Gets or sets the collection of drugs to display.
        /// </summary>
        public ObservableCollection<Drug> Drugs { get; set; } = new ObservableCollection<Drug>();

        /// <summary>
        /// Loads the current list of drugs from the data model.
        /// </summary>
        private void LoadDrugs()
        {
            this.Drugs.Clear();

            foreach (Drug drug in this.drugModel.GetDrugs())
            {
                this.Drugs.Add(drug);
            }
        }

        /// <summary>
        /// Saves the modified drug entries after validation.
        /// </summary>
        private void SaveChanges()
        {
            bool hasErrors = false;
            StringBuilder errorMessages = new StringBuilder();

            foreach (Drug drug in this.Drugs)
            {
                if (!this.ValidateDrug(drug))
                {
                    hasErrors = true;
                    errorMessages.AppendLine($"Drug {drug.DrugID}: {this.ErrorMessage}");
                }
                else
                {
                    bool success = this.drugModel.UpdateDrug(drug);

                    if (!success)
                    {
                        errorMessages.AppendLine($"Failed to save changes for drug: {drug.DrugID}");
                        hasErrors = true;
                    }
                }
            }

            this.ErrorMessage = hasErrors ? errorMessages.ToString() : "Changes saved successfully";
        }

        /// <summary>
        /// Validates a single drug record.
        /// </summary>
        /// <param name="drug">The drug to validate.</param>
        /// <returns>True if valid; otherwise, false.</returns>
        private bool ValidateDrug(Drug drug)
        {
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

            if (drug.Supply <= 0)
            {
                this.ErrorMessage = "Supply cannot be negative or zero.";
                return false;
            }

            return true;
        }

        /// <inheritdoc/>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Notifies UI of property value changes.
        /// </summary>
        /// <param name="propertyName">The name of the changed property.</param>
        protected void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
