using Project.Models;
using Project.Utils;
using System;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows;
using ScheduleModel = Project.ClassModels.ScheduleModel;

namespace Project.ViewModel
{
    class DeleteScheduleViewModel : INotifyPropertyChanged
    {
        private readonly ScheduleModel _scheduleModel = new ScheduleModel();

        private Guid _scheduleID;
        public Guid ScheduleID
        {
            get => _scheduleID;
            set
            {
                _scheduleID = value;
                OnPropertyChanged(nameof(ScheduleID));
            }
        }

        private string? _errorMessage;


        public string ErrorMessage
        {
            get => _errorMessage ?? string.Empty;
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }

        public ICommand DeleteScheduleCommand { get; }

        public DeleteScheduleViewModel()
        {
            DeleteScheduleCommand = new RelayCommand(RemoveSchedule);
        }

        private void RemoveSchedule()
        {
            if (ScheduleID == Guid.Empty)
            {
                ErrorMessage = "No schedule was selected";
                return;
            }

            if (!_scheduleModel.DoesScheduleExist(ScheduleID))
            {
                ErrorMessage = "ScheduleID doesn't exist in the Schedule Records";
                return;
            }

            // MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete schedule {ScheduleID}?", 
            //                                           "Confirm Deletion", 
            //                                           MessageBoxButton.YesNo, 
            //                                           MessageBoxImage.Warning);

            // if (result == MessageBoxResult.Yes)
            // {
            //     bool success = _scheduleModel.DeleteSchedule(ScheduleID);
            //     ErrorMessage = success ? "Schedule deleted successfully" : "Failed to delete schedule";
            // }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
