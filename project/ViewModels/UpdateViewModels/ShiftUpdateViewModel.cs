// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ShiftUpdateViewModel.cs" company="YourCompany">
//   Copyright (c) YourCompany. All rights reserved.
// </copyright>
// <summary>
//   ViewModel for updating shifts.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Project.ViewModels.UpdateViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using Project.ClassModels;
    using Project.Models;
    using Project.Utils;

    /// <summary>
    /// ViewModel for updating shifts.
    /// </summary>
    public class ShiftUpdateViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// The model for managing shifts.
        /// </summary>
        private readonly ShiftModel shiftModel = new ();

        /// <summary>
        /// The collection of shifts displayed in the view.
        /// </summary>
        private string errorMessage;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShiftUpdateViewModel"/> class.
        /// </summary>
        public ShiftUpdateViewModel()
        {
            this.errorMessage = string.Empty;
            this.SaveChangesCommand = new RelayCommand(this.SaveChanges);
            this.LoadShifts();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        
        /// <summary>
        /// Gets or sets the ID of the shifts to be updated.
        /// </summary>
        public ObservableCollection<Shift> Shifts { get; set; } = new ();

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
        /// Gets the command for saving changes.
        /// </summary>
        public ICommand SaveChangesCommand { get; }
        
        /// <summary>
        /// Loads the shifts from the model.
        /// </summary>
        private void LoadShifts()
        {
            this.Shifts.Clear();
            foreach (Shift shift in this.shiftModel.GetShifts())
            {
                this.Shifts.Add(shift);
            }
        }

        /// <summary>
        /// Saves the changes made to the shifts.
        /// </summary>
        private void SaveChanges()
        {
            bool hasErrors = false;

            StringBuilder errorMessages = new StringBuilder();

            foreach (Shift shift in this.Shifts)
            {
                if (!this.ValidateShift(shift))
                {
                    hasErrors = true;
                    errorMessages.AppendLine("Shift " + shift.ShiftID + ": " + this.ErrorMessage);
                }
                else
                {
                    bool success = this.shiftModel.UpdateShift(shift);
                    if (!success)
                    {
                        errorMessages.AppendLine("Failed to save changes for shift: " + shift.ShiftID);
                        hasErrors = true;
                    }
                }
            }

            if (hasErrors)
            {
                this.ErrorMessage = errorMessages.ToString();
            }
            else
            {
                this.ErrorMessage = "Changes saved successfully";
            }
        }

        /// <summary>
        /// Validates the shift before saving it to the database.
        /// </summary>
        /// <param name="shift">Shift to be validated.</param>
        /// <returns>True if the shift is valid, false otherwise.</returns>
        private bool ValidateShift(Shift shift)
        {
            if (shift.StartTime != new TimeSpan(8, 0, 0) && shift.StartTime != new TimeSpan(20, 0, 0))
            {
                this.ErrorMessage = "Start time should be either 8:00 AM or 8:00 PM";
                return false;
            }

            if (shift.EndTime != new TimeSpan(8, 0, 0) && shift.EndTime != new TimeSpan(20, 0, 0))
            {
                this.ErrorMessage = "End time should be either 8:00 AM or 8:00 PM";
                return false;
            }

            return true;
        }

        private void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}