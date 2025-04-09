namespace Project.ViewModels.AddViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows.Input;
    using Project.ClassModels;
    using Project.Models;
    using Project.Utils;

    internal class ShiftAddViewModel : INotifyPropertyChanged
    {
        public ShiftAddViewModel()
        {
            this.SaveShiftCommand = new RelayCommand(this.SaveShift);
            this.LoadShifts();
        }

        public ObservableCollection<Shift> Shifts { get; set; } = new ();

        public DateOnly Date
        {
            get => this.date;
            set
            {
                this.date = value;
                this.OnPropertyChanged(nameof(this.Date));
            }
        }
        
        public TimeSpan StartTime
        {
            get => this.startTime;
            set
            {
                this.startTime = value;
                this.OnPropertyChanged(nameof(this.StartTime));
            }
        }
        
        public TimeSpan EndTime
        {
            get => this.endTime;
            set
            {
                this.endTime = value;
                this.OnPropertyChanged(nameof(this.EndTime));
            }
        }
        
        public string ErrorMessage
        {
            get => this.errorMessage;
            set
            {
                this.errorMessage = value;
                this.OnPropertyChanged(nameof(this.ErrorMessage));
            }
        }

        public ICommand SaveShiftCommand { get; }

        private readonly ShiftModel shiftModel = new ();
        private DateOnly date;
        private TimeSpan startTime;
        private TimeSpan endTime;
        private string errorMessage = string.Empty;

        private void LoadShifts()
        {
            this.Shifts.Clear();
            foreach (Shift shift in this.shiftModel.GetShifts())
            {
                this.Shifts.Add(shift);
            }
        }

        private void SaveShift()
        {
            var shift = new Shift
            {
                ShiftID = 0,
                Date = this.Date,
                StartTime = this.StartTime,
                EndTime = this.EndTime,
            };

            if (this.ValidateShift(shift))
            {
                bool success = this.shiftModel.AddShift(shift);
                this.ErrorMessage = success ? "Shift added successfully" : "Failed to add shift";
                if (success)
                {
                    this.LoadShifts();
                }
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

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}