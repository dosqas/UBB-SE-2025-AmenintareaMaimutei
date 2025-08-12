using Project.ClassModels;
using Project.Models;
using Project.Utils;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace Project.ViewModels.AddViewModels
{
    internal class ShiftAddViewModel : INotifyPropertyChanged
    {
        private readonly ShiftModel _shiftModel = new ShiftModel();
        public ObservableCollection<Shift> Shifts { get; set; } = new ObservableCollection<Shift>();

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

        private void SaveShift()
        {
            var shift = new Shift
            {
                ShiftID = 0,
                Date = Date,
                StartTime = StartTime,
                EndTime = EndTime
            };

            if (ValidateShift(shift))
            {
                bool success = _shiftModel.AddShift(shift);
                ErrorMessage = success ? "Shift added successfully" : "Failed to add shift";
                if (success)
                {
                    LoadShifts();
                }
            }
        }

        private bool ValidateShift(Shift shift)
        {
            if (shift.StartTime != new TimeSpan(8, 0, 0) && shift.StartTime != new TimeSpan(20, 0, 0))
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
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}


