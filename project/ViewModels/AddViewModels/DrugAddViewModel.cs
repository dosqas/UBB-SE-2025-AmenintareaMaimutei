using Project.ClassModels;
using Project.Models;
using Project.Utils;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace Project.ViewModels.AddViewModels
{
<<<<<<< Updated upstream
    internal class DrugAddViewModel : INotifyPropertyChanged
=======
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows.Input;
    using Project.ClassModels;
    using Project.Models;
    using Project.Utils;

    /// <summary>
    /// ViewModel for adding a new drug.
    /// </summary>
    public class DrugAddViewModel : INotifyPropertyChanged
>>>>>>> Stashed changes
    {
        private readonly DrugModel _drugModel = new DrugModel();
        public ObservableCollection<Drug> Drugs { get; set; } = new ObservableCollection<Drug>();

        private string _name = "";
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        private string _administration = "";
        public string Administration
        {
            get => _administration;
            set
            {
                _administration = value;
                OnPropertyChanged(nameof(Administration));
            }
        }

        private string _specification = "";
        public string Specification
        {
            get => _specification;
            set
            {
                _specification = value;
                OnPropertyChanged(nameof(Specification));
            }
        }

        private int _supply;
        public int Supply
        {
            get => _supply;
            set
            {
                _supply = value;
                OnPropertyChanged(nameof(Supply));
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

        public ICommand SaveDrugCommand { get; }

        public DrugAddViewModel()
        {
            SaveDrugCommand = new RelayCommand(SaveDrug);
            LoadDrugs();
        }

        private void LoadDrugs()
        {
            Drugs.Clear();
            foreach (Drug drug in _drugModel.GetDrugs())
            {
                Drugs.Add(drug);
            }
        }

        private void SaveDrug()
        {
            var drug = new Drug
            {
                DrugID = 0,
                Name = Name,
                Administration = Administration,
                Specification = Specification,
                Supply = Supply
            };

            if (ValidateDrug(drug))
            {
                bool success = _drugModel.AddDrug(drug);
                ErrorMessage = success ? "Drug added successfully" : "Failed to add drug";
                if (success)
                {
                    LoadDrugs();
                }
            }
        }

        private bool ValidateDrug(Drug drug)
        {
            if (string.IsNullOrWhiteSpace(drug.Name))
            {
                ErrorMessage = "Please enter the name of the drug.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(drug.Administration))
            {
                ErrorMessage = "Please enter the administration of the drug.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(drug.Specification))
            {
                ErrorMessage = "Please enter the specifications of the drug.";
                return false;
            }

            if (drug.Supply <= 0)
            {
                ErrorMessage = "Please enter a number >0 for the supply.";
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
