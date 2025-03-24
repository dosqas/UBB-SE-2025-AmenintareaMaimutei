using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Project.ClassModels;
using Project.Models;
using Project.Utils;

namespace Project.ViewModels.DeleteViewModels
{
    class RoomDeleteViewModel : INotifyPropertyChanged
    {
        private readonly RoomModel _roomModel = new RoomModel();
        private ObservableCollection<Room> _rooms;
        private int _roomID;
        private string _errorMessage;

        public ObservableCollection<Room> Rooms
        {
            get { return _rooms; }
            set { SetProperty(ref _rooms, value); }
        }

        public int RoomID
        {
            get => _roomID;
            set
            {
                _roomID = value;
                OnPropertyChanged(nameof(RoomID));
                OnPropertyChanged(nameof(CanDeleteRoom));
            }
        }

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

        public bool CanDeleteRoom => RoomID > 0;

        public RoomDeleteViewModel()
        {
            // Load rooms for the DataGrid
            Rooms = new ObservableCollection<Room>(_roomModel.GetRooms());

            DeleteRoomCommand = new RelayCommand(RemoveRoom, CanExecuteDeleteRoom);
        }

        private bool CanExecuteDeleteRoom()
        {
            return RoomID > 0;
        }

        private void RemoveRoom()
        {
            if (RoomID == 0)
            {
                ErrorMessage = "No room was selected";
                return;
            }

            if (!_roomModel.DoesRoomExist(RoomID))
            {
                ErrorMessage = "RoomID doesn't exist in the records";
                return;
            }

            bool success = _roomModel.DeleteRoom(RoomID);
            ErrorMessage = success ? "Room deleted successfully" : "Failed to delete room";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void SetProperty<T>(ref T field, T value, [System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            if (!EqualityComparer<T>.Default.Equals(field, value))
            {
                field = value;
                OnPropertyChanged(propertyName);
            }
        }
    }
}