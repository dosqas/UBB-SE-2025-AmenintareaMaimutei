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
    class ShiftUpdateViewModel : INotifyPropertyChanged
    {
        private readonly ShiftModel _shiftModel = new ShiftModel();
        public ObservableCollection<Shift> Shifts { get; set; } = new ObservableCollection<Shift>();

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
        public ShiftUpdateViewModel()
        {
            _errorMessage = string.Empty;
            SaveChangesCommand = new RelayCommand(SaveChanges);
            LoadShifts();
        }
        private void LoadShifts()
        {
            Shifts.Clear();
            foreach (Shift shift in _shiftModel.GetShifts())
            {
                Shifts.Add(shift);
            }
        }
        private void SaveChanges()
        {
            bool hasErrors = false;
            StringBuilder errorMessages = new StringBuilder();

            foreach (Shift shift in Shifts)
            {
                if (!ValidateShift(shift))
                {
                    hasErrors = true;
                    errorMessages.AppendLine("Shift " + shift.ShiftID + ": " + ErrorMessage);
                }
                else
                {
                    bool success = _shiftModel.UpdateShift(shift);
                    if (!success)
                    {
                        errorMessages.AppendLine("Failed to save changes for shift: " + shift.ShiftID);
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

        private bool ValidateShift(Shift shift)
        {
            if(shift.StartTime != new TimeSpan(8, 0, 0) && shift.StartTime != new TimeSpan(20, 0, 0))
            {
                ErrorMessage = "Start time should be either 8:00 AM or 8:00 PM";
                return false;
            }
            if (shift.EndTime != new TimeSpan(8, 0, 0) && shift.EndTime != new TimeSpan(20, 0, 0))
            {
                ErrorMessage = "End time should be either 8:00 AM or 8:00 PM";
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
