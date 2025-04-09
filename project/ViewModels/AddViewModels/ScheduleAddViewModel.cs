// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ScheduleAddViewModel.cs" company="YourCompany">
//   Copyright (c) YourCompany. All rights reserved.
// </copyright>
// <summary>
//   ViewModel for adding schedules.
// </summary>
// --------------------------------------------------------------------------------------------------------------------


namespace Project.ViewModels.AddViewModels
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows.Input;
    using Project.ClassModels;
    using Project.Models;
    using Project.Utils;

    /// <summary>
    /// ViewModel for adding schedules.
    /// </summary>
    internal class ScheduleAddViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// The model for managing schedules.
        /// </summary>
        private readonly ScheduleModel scheduleModel = new ();

        /// <summary>
        /// The collection of schedules displayed in the view.
        /// </summary>
        public ObservableCollection<Schedule> Schedules { get; set; } = new ObservableCollection<Schedule>();

        /// <summary>
        /// The command to save a schedule.
        /// </summary>
        public int DoctorID
        {
            get => this.doctorID;
            set
            {
                this.doctorID = value;
                this.OnPropertyChanged(nameof(this.DoctorID));
            }
        }

        /// <summary>
        /// The ID of the shift to be added.
        /// </summary>
        public int ShiftID
        {
            get => this.shiftID;
            set
            {
                this.shiftID = value;
                this.OnPropertyChanged(nameof(this.ShiftID));
            }
        }

        /// <summary>
        /// The error message to be displayed.
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
        /// The command to save the schedule.
        /// </summary>
        public ICommand SaveScheduleCommand { get; }

        /// <summary>
        /// The command to delete the schedule.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// The ID of the doctor to be added.
        /// </summary>
        private int doctorID;
        
        /// <summary>
        /// The ID of the shift to be added.
        /// </summary>
        private int shiftID;
        
        /// <summary>
        /// The color of the message displayed in the view.
        /// </summary>
        private string errorMessage = string.Empty;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ScheduleAddViewModel"/> class.
        /// </summary>
        public ScheduleAddViewModel()
        {
            this.SaveScheduleCommand = new RelayCommand(this.SaveSchedule);
            this.LoadSchedules();
        }
        
        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event for the specified property.
        /// </summary>
        /// <param name="propertyName">The name of the property that changed.</param>
        protected void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Loads the schedules from the database and populates the Schedules collection.
        /// </summary>
        private void LoadSchedules()
        {
            this.Schedules.Clear();
            foreach (Schedule schedule in this.scheduleModel.GetSchedules())
            {
                this.Schedules.Add(schedule);
            }
        }
    
        /// <summary>
        /// Saves the schedule to the database.
        /// </summary>
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

        /// <summary>
        /// Validates the schedule before saving it to the database.
        /// </summary>
        /// <param name="schedule">Schedule to be validated.</param>
        /// <returns>True if the schedule is valid, false otherwise.</returns>
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