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
    using Project.ViewModel;

    public class ScheduleUpdateViewModel : INotifyPropertyChanged
    {
        private readonly ScheduleModel scheduleModel = new ();

        private string errorMessage;

        public ScheduleUpdateViewModel()
        {
            this.errorMessage = string.Empty;
            this.SaveChangesCommand = new RelayCommand(this.SaveChanges);
            this.LoadSchedules();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

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

        public ObservableCollection<Schedule> Schedules { get; set; } = new ();

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

        private void SaveChanges()
        {
            bool hasErrors = false;
            StringBuilder errorMessages = new StringBuilder();

            foreach (Schedule schedule in this.Schedules)
            {
                if (!this.ValidateSchedule(schedule))
                {
                    hasErrors = true;
                    errorMessages.AppendLine("Schedule " + schedule.ScheduleID + ": " + this.ErrorMessage);
                }
                else
                {
                    bool success = this.scheduleModel.UpdateSchedule(schedule);

                    if (!success)
                    {
                        errorMessages.AppendLine("Failed to save changes for schedule: " + schedule.DoctorID);
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

        private bool ValidateSchedule(Schedule schedule)
        {
            bool doctorExists = this.scheduleModel.DoesDoctorExist(schedule.DoctorID);

            if (!doctorExists)
            {
                this.ErrorMessage = "DoctorID doesn’t exist in the Doctors Records";
                return false;
            }

            bool shiftExists = this.scheduleModel.DoesShiftExist(schedule.ShiftID);

            if (!shiftExists)
            {
                this.ErrorMessage = "ShiftID doesn’t exist in the Shifts Records";
                return false;
            }

            return true;
        }
    }
}
