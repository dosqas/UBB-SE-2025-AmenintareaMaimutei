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
        private string _messageColor = "Red";

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
                MessageColor = string.IsNullOrEmpty(value) ? "Red" : value.Contains("successfully") ? "Green" : "Red";
                OnPropertyChanged(nameof(ErrorMessage));
                OnPropertyChanged(nameof(MessageColor));
            }
        }

        public string MessageColor
        {
            get => _messageColor;
            set
            {
                _messageColor = value;
                OnPropertyChanged(nameof(MessageColor));
            }
        }

        public ICommand DeleteRoomCommand { get; }

        public bool CanDeleteRoom => RoomID > 0;

        public RoomDeleteViewModel()
        {
            var rooms = _roomModel.GetRooms();
            Rooms = rooms != null ? new ObservableCollection<Room>(rooms) : new ObservableCollection<Room>();

            DeleteRoomCommand = new RelayCommand(RemoveRoom);
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

            if (success)
            {
                Rooms = new ObservableCollection<Room>(_roomModel.GetRooms());
            }
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
