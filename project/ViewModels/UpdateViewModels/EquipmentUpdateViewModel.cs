namespace Project.ViewModels.UpdateViewModels
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Text;
    using System.Windows.Input;
    using Project.ClassModels;
    using Project.Models;
    using Project.Utils;

    /// <summary>
    /// ViewModel for updating equipment.
    /// </summary>
    public class EquipmentUpdateViewModel : INotifyPropertyChanged
    {
        private readonly EquipmentModel equipmentModel = new EquipmentModel();
        private string errorMessage = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="EquipmentUpdateViewModel"/> class.
        /// </summary>
        public EquipmentUpdateViewModel()
        {
            this.SaveChangesCommand = new RelayCommand(this.SaveChanges);
            this.LoadEquipments();
        }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Gets or sets the collection of equipment.
        /// </summary>
        public ObservableCollection<Equipment> Equipments { get; set; } = new ObservableCollection<Equipment>();

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        public string ErrorMessage
        {
            get => this.errorMessage;
            set
            {
                this.errorMessage = value;
                this.OnPropertyChanged(nameof(this.ErrorMessage));
            }
        }

        /// <summary>
        /// Gets the command to save changes to the equipment.
        /// </summary>
        public ICommand SaveChangesCommand { get; }

        /// <summary>
        /// Loads the equipment from the database.
        /// </summary>
        private void LoadEquipments()
        {
            this.Equipments.Clear();
            foreach (Equipment equipment in this.equipmentModel.GetEquipments())
            {
                this.Equipments.Add(equipment);
            }
        }

        /// <summary>
        /// Saves the changes to the equipment in the database.
        /// </summary>
        private void SaveChanges()
        {
            bool hasErrors = false;
            StringBuilder errorMessages = new StringBuilder();

            foreach (Equipment equipment in this.Equipments)
            {
                if (!this.ValidateEquipment(equipment))
                {
                    hasErrors = true;
                    errorMessages.AppendLine("Equipment " + equipment.EquipmentID + ": " + this.ErrorMessage);
                }
                else
                {
                    bool success = this.equipmentModel.UpdateEquipment(equipment);
                    if (!success)
                    {
                        errorMessages.AppendLine("Failed to save changes for equipment: " + equipment.EquipmentID);
                        hasErrors = true;
                    }
                }
            }

            this.ErrorMessage = hasErrors ? errorMessages.ToString() : "Changes saved successfully";
        }

        /// <summary>
        /// Validates the equipment.
        /// </summary>
        /// <param name="equipment">The equipment to validate.</param>
        /// <returns>True if the equipment is valid, otherwise false.</returns>
        private bool ValidateEquipment(Equipment equipment)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(equipment.Name, @"^[a-zA-Z0-9 ]*$"))
            {
                this.ErrorMessage = "Please enter the name of the equipment";
                return false;
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(equipment.Specification, @"^[a-zA-Z0-9 ,.-]*$"))
            {
                this.ErrorMessage = "Please enter the specifications of the equipment";
                return false;
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(equipment.Type, @"^[a-zA-Z0-9 ]*$"))
            {
                this.ErrorMessage = "Please enter the type of the equipment";
                return false;
            }

            if (equipment.Stock <= 0)
            {
                this.ErrorMessage = "Equipment stock must be greater than 0!";
                return false;
            }

            return true;
        }

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="propertyName">The name of the property that changed.</param>
        private void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}