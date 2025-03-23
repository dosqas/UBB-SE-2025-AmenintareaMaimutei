using Project.Models;
using Project.Utils;
using System;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows;
using RoomModel = Project.ClassModels.RoomModel;

namespace Project.ViewModel
{
    class RoomDeleteViewModel : INotifyPropertyChanged
    {
        private readonly RoomModel _roomModel = new RoomModel();

        private int _roomID;
        public int RoomID
        {
            get => _roomID;
            set
            {
                _roomID = value;
                OnPropertyChanged(nameof(RoomID));
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

        public ICommand DeleteRoomCommand { get; }

        public RoomDeleteViewModel()
        {
            DeleteRoomCommand = new RelayCommand(RemoveRoom);
        }

        private void RemoveRoom()
        {
            //if (RoomID == Guid.Empty)
            if ( RoomID == 0)  
            {
                ErrorMessage = "No room was selected";
                return;
            }

            if (!_roomModel.DoesRoomExist(RoomID))
            {
                ErrorMessage = "RoomID doesn't exist in the Room Records";
                return;
            }

            // MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete room {RoomID}?", 
            //                                           "Confirm Deletion", 
            //                                           MessageBoxButton.YesNo, 
            //                                           MessageBoxImage.Warning);

            // if (result == MessageBoxResult.Yes)
            // {
            //     bool success = _roomModel.DeleteRoom(RoomID);
            //     ErrorMessage = success ? "Room deleted successfully" : "Failed to delete room";
            // }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
