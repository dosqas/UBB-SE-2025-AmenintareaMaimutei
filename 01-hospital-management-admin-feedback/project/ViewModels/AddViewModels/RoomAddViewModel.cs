using Project.ClassModels;
using Project.Models;
using Project.Utils;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace Project.ViewModels.AddViewModels
{
    internal class RoomAddViewModel : INotifyPropertyChanged
    {
        private readonly RoomModel _roomModel = new RoomModel();
        public ObservableCollection<Room> Rooms { get; set; } = new ObservableCollection<Room>();

        private int _capacity;
        public int Capacity
        {
            get => _capacity;
            set
            {
                _capacity = value;
                OnPropertyChanged(nameof(Capacity));
            }
        }

        private int _departmentID;
        public int DepartmentID
        {
            get => _departmentID;
            set
            {
                _departmentID = value;
                OnPropertyChanged(nameof(DepartmentID));
            }
        }

        private int _equipmentID;
        public int EquipmentID
        {
            get => _equipmentID;
            set
            {
                _equipmentID = value;
                OnPropertyChanged(nameof(EquipmentID));
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

        public ICommand SaveRoomCommand { get; }

        public RoomAddViewModel()
        {
            SaveRoomCommand = new RelayCommand(SaveRoom);
            LoadRooms();
        }

        private void LoadRooms()
        {
            Rooms.Clear();
            foreach (Room room in _roomModel.GetRooms())
            {
                Rooms.Add(room);
            }
        }

        private void SaveRoom()
        {
            var room = new Room
            {
                RoomID = 0,
                Capacity = Capacity,
                DepartmentID = DepartmentID,
                EquipmentID = EquipmentID
            };

            if (ValidateRoom(room))
            {
                bool success = _roomModel.AddRoom(room);
                ErrorMessage = success ? "Room added successfully" : "Failed to add room";
                if (success)
                {
                    LoadRooms();
                }
            }
        }

        private bool ValidateRoom(Room room)
        {
            if (room.Capacity <= 0)
            {
                ErrorMessage = "Please enter a number >0 for the capacity.";
                return false;
            }

            if (!_roomModel.DoesDepartmentExist(room.DepartmentID))
            {
                ErrorMessage = "DepartmentID doesn’t exist in the Departments Records.";
                return false;
            }

            if (!_roomModel.DoesEquipmentExist(room.EquipmentID))
            {
                ErrorMessage = "EquipmentID doesn’t exist in the Equipments Records.";
                return false;
            }

            return true;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

