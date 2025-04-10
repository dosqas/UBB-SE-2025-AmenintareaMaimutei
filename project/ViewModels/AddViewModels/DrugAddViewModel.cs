// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DrugAddViewModel.cs" company="YourCompany">
//   Copyright (c) YourCompany. All rights reserved.
// </copyright>
// <summary>
//   ViewModel responsible for handling drug addition logic, validation, and data binding.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Project.ViewModels.AddViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows.Input;
    using Project.ClassModels;
    using Project.Models;
    using Project.Utils;

    /// <summary>
    /// ViewModel for adding a new drug.
    /// </summary>
    internal class DrugAddViewModel : INotifyPropertyChanged
    {
        private readonly DrugModel drugModel = new DrugModel();
        private string name = string.Empty;
        private string administration = string.Empty;
        private string specification = string.Empty;
        private int supply;
        private string errorMessage = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="DrugAddViewModel"/> class.
        /// </summary>
        public DrugAddViewModel()
        {
            this.SaveDrugCommand = new RelayCommand(this.SaveDrug);
            this.LoadDrugs();
        }

        /// <summary>
        /// Gets the list of all drugs.
        /// </summary>
        public ObservableCollection<Drug> Drugs { get; set; } = new ObservableCollection<Drug>();

        /// <summary>
        /// Gets or sets the name of the drug.
        /// </summary>
        public string Name
        {
            get => this.name;
            set
            {
                this.name = value;
                this.OnPropertyChanged(nameof(this.Name));
            }
        }

        /// <summary>
        /// Gets or sets the administration method of the drug.
        /// </summary>
        public string Administration
        {
            get => this.administration;
            set
            {
                this.administration = value;
                this.OnPropertyChanged(nameof(this.Administration));
            }
        }

        /// <summary>
        /// Gets or sets the specifications of the drug.
        /// </summary>
        public string Specification
        {
            get => this.specification;
            set
            {
                this.specification = value;
                this.OnPropertyChanged(nameof(this.Specification));
            }
        }

        /// <summary>
        /// Gets or sets the supply amount of the drug.
        /// </summary>
        public int Supply
        {
            get => this.supply;
            set
            {
                this.supply = value;
                this.OnPropertyChanged(nameof(this.Supply));
            }
        }

        /// <summary>
        /// Gets or sets the error message to be displayed.
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
        /// Gets the command that triggers the SaveDrug method.
        /// </summary>
        public ICommand SaveDrugCommand { get; }

        /// <summary>
        /// Loads all drugs into the ObservableCollection.
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
        /// Creates and saves a new drug if validation passes.
        /// </summary>
        private void SaveDrug()
        {
            var drug = new Drug
            {
                DrugID = 0,
                Name = this.Name,
                Administration = this.Administration,
                Specification = this.Specification,
                Supply = this.Supply,
            };

            if (this.ValidateDrug(drug))
            {
                bool success = this.drugModel.AddDrug(drug);
                this.ErrorMessage = success ? "Drug added successfully" : "Failed to add drug";

                if (success)
                {
                    this.LoadDrugs();
                }
            }
        }

        /// <summary>
        /// Validates the drug information before saving.
        /// </summary>
        /// <param name="drug">The drug to validate.</param>
        /// <returns><c>true</c> if valid; otherwise, <c>false</c>.</returns>
        private bool ValidateDrug(Drug drug)
        {
            if (string.IsNullOrWhiteSpace(drug.Name))
            {
                this.ErrorMessage = "Please enter the name of the drug.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(drug.Administration))
            {
                this.ErrorMessage = "Please enter the administration of the drug.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(drug.Specification))
            {
                this.ErrorMessage = "Please enter the specifications of the drug.";
                return false;
            }

            if (drug.Supply <= 0)
            {
                this.ErrorMessage = "Please enter a number > 0 for the supply.";
                return false;
            }

            return true;
        }

        /// <inheritdoc/>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Triggers the PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">The name of the changed property.</param>
        protected void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
