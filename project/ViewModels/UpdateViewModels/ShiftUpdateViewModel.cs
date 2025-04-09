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

    public class ShiftUpdateViewModel : INotifyPropertyChanged
    {
        private readonly ShiftModel shiftModel = new ();

        private string errorMessage;

        public ShiftUpdateViewModel()
        {
            this.errorMessage = string.Empty;
            this.SaveChangesCommand = new RelayCommand(this.SaveChanges);
            this.LoadShifts();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<Shift> Shifts { get; set; } = new ();

        public string ErrorMessage
        {
            get => this.errorMessage;
            set
            {
                this.errorMessage = value;
                this.OnPropertyChanged(nameof(this.ErrorMessage));
            }
        }

        public ICommand SaveChangesCommand { get; }
        
        private void LoadShifts()
        {
            this.Shifts.Clear();
            foreach (Shift shift in this.shiftModel.GetShifts())
            {
                this.Shifts.Add(shift);
            }
        }

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