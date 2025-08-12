using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Project.ClassModels;
using Project.Models;
using Project.Utils;

namespace Project.ViewModels.DeleteViewModels
{
    class DrugDeleteViewModel : INotifyPropertyChanged
    {
        private readonly DrugModel _drugModel = new DrugModel();
        private ObservableCollection<Drug> _drugs;
        private int _drugID;
        private string _errorMessage;
        private string _messageColor = "Red";

        public ObservableCollection<Drug> Drugs
        {
            get { return _drugs; }
            set { SetProperty(ref _drugs, value); }
        }

        public int DrugID
        {
            get => _drugID;
            set
            {
                _drugID = value;
                OnPropertyChanged(nameof(DrugID));
                OnPropertyChanged(nameof(CanDeleteDrug));
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

        public ICommand DeleteDrugCommand { get; }

        public bool CanDeleteDrug => DrugID > 0;

        public DrugDeleteViewModel()
        {
            // Load drugs for the DataGrid
            Drugs = new ObservableCollection<Drug>(_drugModel.GetDrugs());

            DeleteDrugCommand = new RelayCommand(RemoveDrug);
        }

        private bool CanExecuteDeleteDrug()
        {
            return DrugID > 0;
        }

        private void RemoveDrug()
        {
            if (DrugID == 0)
            {
                ErrorMessage = "No drug was selected";
                return;
            }

            if (!_drugModel.DoesDrugExist(DrugID))
            {
                ErrorMessage = "DrugID doesn't exist in the records";
                return;
            }

            bool success = _drugModel.DeleteDrug(DrugID);
            ErrorMessage = success ? "Drug deleted successfully" : "Failed to delete drug";

            if (success)
            {
                Drugs = new ObservableCollection<Drug>(_drugModel.GetDrugs());
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
