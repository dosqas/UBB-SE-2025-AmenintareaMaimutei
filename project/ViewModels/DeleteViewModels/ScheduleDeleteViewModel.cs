using Project.ClassModels;
using Project.Models;
using Project.Utils;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Microsoft.UI.Xaml.Controls; // Ensure you're using WinUI for ContentDialog

namespace Project.ViewModels.DeleteViewModels
{
    class ScheduleDeleteViewModel : INotifyPropertyChanged
    {
        private readonly ScheduleModel _scheduleModel = new ScheduleModel();

        public ObservableCollection<Schedule> Schedules { get; } = new ObservableCollection<Schedule>();

        private Schedule? _selectedSchedule;
        public Schedule? SelectedSchedule
        {
            get => _selectedSchedule;
            set
            {
                _selectedSchedule = value;
                ScheduleID = value?.ScheduleID ?? 0;
                OnPropertyChanged(nameof(SelectedSchedule));
                OnPropertyChanged(nameof(CanDeleteSchedule));
            }
        }

        private int _scheduleID;
        public int ScheduleID
        {
            get => _scheduleID;
            set
            {
                _scheduleID = value;
                OnPropertyChanged(nameof(ScheduleID));
                OnPropertyChanged(nameof(CanDeleteSchedule));
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

        public bool CanDeleteSchedule => ScheduleID != 0;

        public ICommand DeleteScheduleCommand { get; }

        public ScheduleDeleteViewModel()
        {
            DeleteScheduleCommand = new RelayCommand(RemoveSchedule, () => CanDeleteSchedule);
            LoadSchedules();
        }

        private void LoadSchedules()
        {
            Schedules.Clear();
            foreach (var schedule in _scheduleModel.GetSchedules())
            {
                Schedules.Add(schedule);
            }
        }

        private async void RemoveSchedule()
        {
            if (ScheduleID == 0)
            {
                ErrorMessage = "No schedule was selected.";
                return;
            }

            if (!_scheduleModel.DoesScheduleExist(ScheduleID))
            {
                ErrorMessage = "ScheduleID doesn't exist in the records.";
                return;
            }

            // Confirmation Dialog
            var dialog = new ContentDialog
            {
                Title = "Confirm Deletion",
                Content = $"Are you sure you want to delete schedule {ScheduleID}?",
                PrimaryButtonText = "Yes",
                CloseButtonText = "No",
                DefaultButton = ContentDialogButton.Close,
               // XamlRoot = App.MainWindow.Content.XamlRoot // Ensure App.xaml.cs has MainWindow reference
            };

            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                bool success = _scheduleModel.DeleteSchedule(ScheduleID);
                if (success)
                {
                    ErrorMessage = "Schedule deleted successfully.";
                    LoadSchedules(); // Refresh list
                }
                else
                {
                    ErrorMessage = "Failed to delete schedule.";
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
