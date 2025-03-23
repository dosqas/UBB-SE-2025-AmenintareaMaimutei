using Project.Models;
using Project.Utils;
using System;
using System.ComponentModel;
using System.Windows.Input;
using ShiftModel = Project.ClassModels.ShiftModel;

namespace Project.ViewModels.AddViewModels
{
    internal class ShiftAddViewModel : INotifyPropertyChanged
    {
        private readonly ShiftModel _shiftModel = new ShiftModel();

        private DateOnly _date;
        public DateOnly Date
        {
            get => _date;
            set
            {
                _date = value;
                OnPropertyChanged(nameof(Date));
            }
        }

        private TimeSpan _startTime;
        public TimeSpan StartTime
        {
            get => _startTime;
            set
            {
                _startTime = value;
                OnPropertyChanged(nameof(StartTime));
            }
        }

        private TimeSpan _endTime;
        public TimeSpan EndTime
        {
            get => _endTime;
            set
            {
                _endTime = value;
                OnPropertyChanged(nameof(EndTime));
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

        public ICommand SaveShiftCommand { get; }

        public ShiftAddViewModel()
        {
            SaveShiftCommand = new RelayCommand(SaveShift);
        }

        private void SaveShift()
        {
            var shift = new Shift
            {
                ShiftID = Guid.NewGuid(),
                Date = Date,
                StartTime = StartTime,
                EndTime = EndTime
            };

            if (ValidateShift(shift))
            {
                bool success = _shiftModel.AddShift(shift);
                ErrorMessage = success ? "Shift added successfully" : "Failed to add shift";
            }
        }

        private bool ValidateShift(Shift shift)
        {
            if (shift.Date == default)
            {
                ErrorMessage = "Please enter a valid date.";
                return false;
            }

            if (shift.StartTime >= shift.EndTime)
            {
                ErrorMessage = "Start time must be before end time.";
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
