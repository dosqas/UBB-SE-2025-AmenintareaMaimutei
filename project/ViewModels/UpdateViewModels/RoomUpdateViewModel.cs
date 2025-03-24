using Project.ClassModels;
using Project.Models;
using Project.Utils;
using Project.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Project.ViewModels.UpdateViewModels
{
    class RoomUpdateViewModel : INotifyPropertyChanged
    {
        private readonly RoomModel _roomModel = new RoomModel();
        public ObservableCollection<Room> Rooms { get; set; } = new ObservableCollection<Room>();

        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }

        public ICommand SaveChangesCommand { get; }

        public RoomUpdateViewModel()
        {
            _errorMessage = string.Empty;
            SaveChangesCommand = new RelayCommand(SaveChanges);
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

        private void SaveChanges()
        {
            foreach (Room room in Rooms)
            {
                if (ValidateRoom(room))
                {
                    bool success = _roomModel.UpdateRoom(room);
                    ErrorMessage = success ? "Changes saved successfully!" : "Failed to save changes.";
                }
            }
        }

        private bool ValidateRoom(Room room)
        {
            if (room.Capacity < 0)
            {
                ErrorMessage = "Capacity cannot be negative.";
                return false;
            }
            if (room.DepartmentID < 0)
            {
                ErrorMessage = "Department ID cannot be negative.";
                return false;
            }
            if (room.EquipmentID < 0)
            {
                ErrorMessage = "Equipment ID cannot be negative.";
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
