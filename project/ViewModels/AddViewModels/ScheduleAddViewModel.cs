namespace Project.ViewModels.AddViewModels
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows.Input;
    using Project.ClassModels;
    using Project.Models;
    using Project.Utils;

    internal class ScheduleAddViewModel : INotifyPropertyChanged
    {
        private readonly ScheduleModel scheduleModel = new ();

        public ObservableCollection<Schedule> Schedules { get; set; } = new ObservableCollection<Schedule>();

        public int DoctorID
        {
            get => this.doctorID;
            set
            {
                this.doctorID = value;
                this.OnPropertyChanged(nameof(this.DoctorID));
            }
        }

        public int ShiftID
        {
            get => this.shiftID;
            set
            {
                this.shiftID = value;
                this.OnPropertyChanged(nameof(this.ShiftID));
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

        public ICommand SaveScheduleCommand { get; }

        public event PropertyChangedEventHandler? PropertyChanged;

        private int doctorID;
        private int shiftID;
        private string errorMessage = string.Empty;
        
        public ScheduleAddViewModel()
        {
            this.SaveScheduleCommand = new RelayCommand(this.SaveSchedule);
            this.LoadSchedules();
        }

        protected void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void LoadSchedules()
        {
            this.Schedules.Clear();
            foreach (Schedule schedule in this.scheduleModel.GetSchedules())
            {
                this.Schedules.Add(schedule);
            }
        }

        private void SaveSchedule()
        {
            var schedule = new Schedule
            {
                ScheduleID = 0,
                DoctorID = this.DoctorID,
                ShiftID = this.ShiftID,
            };

            if (this.ValidateSchedule(schedule))
            {
                bool success = this.scheduleModel.AddSchedule(schedule);
                this.ErrorMessage = success ? "Schedule added successfully" : "Failed to add schedule";
                if (success)
                {
                    this.LoadSchedules();
                }
            }
        }

        private bool ValidateSchedule(Schedule schedule)
        {
            if (schedule.DoctorID == 0 || !this.scheduleModel.DoesDoctorExist(schedule.DoctorID))
            {
                this.ErrorMessage = "DoctorID doesn’t exist in the Doctors Records.";
                return false;
            }

            if (schedule.ShiftID == 0 || !this.scheduleModel.DoesShiftExist(schedule.ShiftID))
            {
                this.ErrorMessage = "ShiftID doesn’t exist in the Shifts Records.";
                return false;
            }

            return true;
        }
    }
}