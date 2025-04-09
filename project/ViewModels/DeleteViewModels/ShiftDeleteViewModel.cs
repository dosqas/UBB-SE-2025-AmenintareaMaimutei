namespace Project.ViewModels.DeleteViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using Project.ClassModels;
    using Project.Models;
    using Project.Utils;

    public class ShiftDeleteViewModel : INotifyPropertyChanged
    {
        private readonly ShiftModel shiftModel = new ShiftModel();
        private ObservableCollection<Shift> shifts;
        private int shiftID;
        private string errorMessage;
        private string messageColor = "Red";

        public ShiftDeleteViewModel()
        {
            this.Shifts = new ObservableCollection<Shift>(this.shiftModel.GetShifts());
            this.DeleteShiftCommand = new RelayCommand(this.RemoveShift);
        }

        public ObservableCollection<Shift> Shifts
        {
            get => this.shifts;
            set => this.SetProperty(ref this.shifts, value);
        }

        public int ShiftID
        {
            get => this.shiftID;
            set
            {
                this.shiftID = value;
                this.OnPropertyChanged(nameof(this.ShiftID));
                this.OnPropertyChanged(nameof(this.CanDeleteShift));
            }
        }

        public string ErrorMessage
        {
            get => this.errorMessage ?? string.Empty;
            set
            {
                this.errorMessage = value;
                this.MessageColor = string.IsNullOrEmpty(value) ? "Red" : value.Contains("successfully") ? "Green" : "Red";
                this.OnPropertyChanged(nameof(this.ErrorMessage));
                this.OnPropertyChanged(nameof(this.MessageColor));
            }
        }

        public string MessageColor
        {
            get => this.messageColor;
            set
            {
                this.messageColor = value;
                this.OnPropertyChanged(nameof(this.MessageColor));
            }
        }

        public ICommand DeleteShiftCommand { get; }

        public bool CanDeleteShift => this.ShiftID > 0;

        private void RemoveShift()
        {
            if (this.ShiftID == 0)
            {
                this.ErrorMessage = "No shift was selected";
                return;
            }

            if (!this.shiftModel.DoesShiftExist(this.ShiftID))
            {
                this.ErrorMessage = "ShiftID doesn't exist in the records";
                return;
            }

            bool succes = this.shiftModel.DeleteShift(this.ShiftID);
            this.ErrorMessage = succes ? "Shift was successfully deleted" : "Shift was not deleted";
            if (succes)
            {
                this.Shifts = new ObservableCollection<Shift>(this.shiftModel.GetShifts());
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void SetProperty<T>(ref T field, T value, [System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            if (!EqualityComparer<T>.Default.Equals(field, value))
            {
                field = value;
                this.OnPropertyChanged(propertyName);
            }
        }
    }
}
