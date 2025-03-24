using Project.ClassModels;
using Project.Models;
using Project.Utils;
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
    class ShiftUpdateViewModel : INotifyPropertyChanged
    {
        private readonly ShiftModel _shiftModel = new ShiftModel();
        public ObservableCollection<Shift> Shifts { get; set; } = new ObservableCollection<Shift>();

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
        public ShiftUpdateViewModel()
        {
            _errorMessage = string.Empty;
            SaveChangesCommand = new RelayCommand(SaveChanges);
            LoadShifts();
        }
        private void LoadShifts()
        {
            Shifts.Clear();
            foreach (Shift shift in _shiftModel.GetShifts())
            {
                Shifts.Add(shift);
            }
        }
        private void SaveChanges()
        {
            foreach (Shift shift in Shifts)
            {
                if (ValidateShift(shift))
                {
                    bool success = _shiftModel.UpdateShift(shift);
                    ErrorMessage = success ? "Changes saved successfully!" : "Failed to save changes.";
                }
            }
        }

        private bool ValidateShift(Shift shift)
        {
            if (shift.ShiftID <= 0)
            {
                ErrorMessage = "Shift name cannot be empty.";
                return false;
            }
            
            return true;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
