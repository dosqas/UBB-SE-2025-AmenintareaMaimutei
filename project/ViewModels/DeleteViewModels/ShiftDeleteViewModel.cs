using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Project.ClassModels;
using Project.Models;
using Project.Utils;

namespace Project.ViewModels.DeleteViewModels
{
    class ShiftDeleteViewModel : INotifyPropertyChanged
    {
        private readonly ShiftModel _shiftModel = new ShiftModel();
        private ObservableCollection<Shift> _shifts;
        private int _shiftID;
        private string _errorMessage;
        private string _messageColor = "Red";

        public ObservableCollection<Shift> Shifts
        {
            get { return _shifts; }
            set { SetProperty(ref _shifts, value); }
        }

        public int ShiftID
        {
            get => _shiftID;
            set
            {
                _shiftID = value;
                OnPropertyChanged(nameof(ShiftID));
                OnPropertyChanged(nameof(CanDeleteShift));
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

        public ICommand DeleteShiftCommand { get; }

        public bool CanDeleteShift => ShiftID > 0;

        public ShiftDeleteViewModel()
        {
            Shifts = new ObservableCollection<Shift>(_shiftModel.GetShifts());
            DeleteShiftCommand = new RelayCommand(RemoveShift);
        }

        private void RemoveShift()
        {
            if (ShiftID == 0)
            {
                ErrorMessage = "No shift was selected";
                return;
            }
            if (!_shiftModel.DoesShiftExist(ShiftID))
            {
                ErrorMessage = "ShiftID doesn't exist in the records";
                return;
            }
            bool succes = _shiftModel.DeleteShift(ShiftID);
            ErrorMessage = succes ? "Shift was successfully deleted" : "Shift was not deleted";
            if (succes)
            {
                Shifts = new ObservableCollection<Shift>(_shiftModel.GetShifts());
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
