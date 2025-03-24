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
            bool hasErrors = false;
            StringBuilder errorMessages = new StringBuilder();

            foreach (Room room in Rooms)
            {
                if (!ValidateRoom(room))
                {
                    hasErrors = true;
                    errorMessages.AppendLine("Room " + room.RoomID + ": " + ErrorMessage);
                }
                else
                {
                    bool success = _roomModel.UpdateRoom(room);
                    if (!success)
                    {
                        errorMessages.AppendLine("Failed to save changes for room: " + room.RoomID);
                        hasErrors = true;
                    }
                }
            }
            if (hasErrors)
            {
                ErrorMessage = errorMessages.ToString();
            }
            else
            {
                ErrorMessage = "Changes saved successfully";
            }
        }

        private bool ValidateRoom(Room room)
        {
            if (room.Capacity <= 0)
            {
                ErrorMessage = "Please enter a number >0.";
                return false;
            }
            
            bool departmentExists = _roomModel.DoesDepartmentExist(room.DepartmentID);
            if (!departmentExists)
            {
                ErrorMessage = "Department ID doesn’t exist in the Departments Records";
                return false;
            }
            
            bool equipmentExists = _roomModel.DoesEquipmentExist(room.EquipmentID);
            if(!equipmentExists)
            {
                ErrorMessage = "Equipment ID doesn’t exist in the Equipment Records";
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
