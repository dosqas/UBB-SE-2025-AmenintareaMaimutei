namespace Project.ViewModels.DeleteViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows.Input;
    using Project.ClassModels;
    using Project.Models;
    using Project.Utils;

    public class ScheduleDeleteViewModel : INotifyPropertyChanged
    {
        private readonly ScheduleModel scheduleModel = new ();
        private ObservableCollection<Schedule> schedules;
        private int scheduleID;
        private string errorMessage;
        private string messageColor = "Red";

        public ScheduleDeleteViewModel()
        {
            // Load schedules for the DataGrid
            this.Schedules = new ObservableCollection<Schedule>(this.scheduleModel.GetSchedules());

            this.DeleteScheduleCommand = new RelayCommand(this.RemoveSchedule);
        }

        public ObservableCollection<Schedule> Schedules
        {
            get => this.schedules;
            set => this.SetProperty(ref this.schedules, value);
        }

        public int ScheduleID
        {
            get => this.scheduleID;
            set
            {
                this.scheduleID = value;
                this.OnPropertyChanged(nameof(this.ScheduleID));
                this.OnPropertyChanged(nameof(this.CanDeleteSchedule));
            }
        }

        public string ErrorMessage
        {
            get => this.errorMessage ?? string.Empty;
            set
            {
                this.errorMessage = value;

                // Set MessageColor based on success or error
                this.MessageColor = string.IsNullOrEmpty(value) ? "Red" : value.Contains("successfully") ? "Green" : "Red";
                this.OnPropertyChanged(nameof(this.ErrorMessage));
                this.OnPropertyChanged(nameof(this.MessageColor));
            }
        }

        public string MessageColor
        {
            get => this.messageColor;
            set
            {
                this.messageColor = value;
                this.OnPropertyChanged(nameof(this.MessageColor));
            }
        }

        public ICommand DeleteScheduleCommand { get; }

        public bool CanDeleteSchedule => this.ScheduleID > 0;

        private void RemoveSchedule()
        {
            if (this.ScheduleID == 0)
            {
                this.ErrorMessage = "No schedule was selected";
                return;
            }

            if (!this.scheduleModel.DoesScheduleExist(this.ScheduleID))
            {
                this.ErrorMessage = "ScheduleID doesn't exist in the records";
                return;
            }

            bool success = this.scheduleModel.DeleteSchedule(this.ScheduleID);
            this.ErrorMessage = success ? "Schedule deleted successfully" : "Failed to delete schedule";

            if (success)
            {
                this.Schedules = new ObservableCollection<Schedule>(this.scheduleModel.GetSchedules());
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void SetProperty<T>(ref T field, T value, [System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            if (!EqualityComparer<T>.Default.Equals(field, value))
            {
                field = value;
                this.OnPropertyChanged(propertyName);
            }
        }
    }
}
