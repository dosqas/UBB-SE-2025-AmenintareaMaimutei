// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ScheduleDeleteViewModel.cs" company="YourCompany">
//   Copyright (c) YourCompany. All rights reserved.
// </copyright>
// <summary>
//   ViewModel for deleting schedules.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Project.ViewModels.DeleteViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows.Input;
    using Project.ClassModels;
    using Project.Models;
    using Project.Utils;

    /// <summary>
    /// ViewModel for deleting schedules.
    /// </summary>
    public class ScheduleDeleteViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// The model for managing schedules.
        /// </summary>
        private readonly ScheduleModel scheduleModel = new ();
        
        /// <summary>
        /// The collection of schedules displayed in the view.
        /// </summary>
        private ObservableCollection<Schedule> schedules;
        
        /// <summary>
        /// The ID of the schedule to be deleted.
        /// </summary>
        private int scheduleID;
        
        /// <summary>
        /// The error message to be displayed.
        /// </summary>
        private string errorMessage;
        
        /// <summary>
        /// The color of the message to be displayed.
        /// </summary>
        private string messageColor = "Red";

        /// <summary>
        /// Initializes a new instance of the <see cref="ScheduleDeleteViewModel"/> class.
        /// </summary>
        public ScheduleDeleteViewModel()
        {
            // Load schedules for the DataGrid
            this.Schedules = new ObservableCollection<Schedule>(this.scheduleModel.GetSchedules());

            this.DeleteScheduleCommand = new RelayCommand(this.RemoveSchedule);
        }

        /// <summary>
        /// Gets or sets the collection of schedules displayed in the view.
        /// </summary>
        public ObservableCollection<Schedule> Schedules
        {
            get => this.schedules;
            set => this.SetProperty(ref this.schedules, value);
        }

        /// <summary>
        /// Gets or sets the ID of the schedule to be deleted.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the error message to display in the view.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the color of the message displayed in the view.
        /// </summary>
        public string MessageColor
        {
            get => this.messageColor;
            set
            {
                this.messageColor = value;
                this.OnPropertyChanged(nameof(this.MessageColor));
            }
        }

        /// <summary>
        /// Gets the command to delete a schedule.
        /// </summary>
        public ICommand DeleteScheduleCommand { get; }

        /// <summary>
        /// Gets a value indicating whether the schedule can be deleted.
        /// </summary>
        public bool CanDeleteSchedule => this.ScheduleID > 0;

        /// <summary>
        /// Removes the selected schedule from the database.
        /// </summary>
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

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises the PropertyChanged event for the specified property.
        /// </summary>
        /// <param name="propertyName"></param>
        protected void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Sets the property value and raises the PropertyChanged event if the value has changed.
        /// </summary>
        /// <param name="field"> The field to set.</param>
        /// <param name="value"> The new value.</param>
        /// <param name="propertyName"> The name of the property.</param>
        /// <typeparam name="T"> The type of the property.</typeparam>
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
