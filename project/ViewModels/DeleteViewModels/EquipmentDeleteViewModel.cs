namespace Project.ViewModels.DeleteViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows.Input;
    using Project.ClassModels;
    using Project.Models;
    using Project.Utils;

    /// <summary>
    /// ViewModel for deleting equipment.
    /// </summary>
    public class EquipmentDeleteViewModel : INotifyPropertyChanged
    {
        private readonly EquipmentModel equipmentModel = new EquipmentModel();
        private ObservableCollection<Equipment> equipments = new ObservableCollection<Equipment>();
        private int equipmentID;
        private string errorMessage = string.Empty;
        private string messageColor = "Red";

        /// <summary>
        /// Initializes a new instance of the <see cref="EquipmentDeleteViewModel"/> class.
        /// </summary>
        public EquipmentDeleteViewModel()
        {
            // Load equipment for the DataGrid
            this.Equipments = new ObservableCollection<Equipment>(this.equipmentModel.GetEquipments());
            this.DeleteEquipmentCommand = new RelayCommand(this.RemoveEquipment);
        }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Gets or sets the collection of equipment.
        /// </summary>
        public ObservableCollection<Equipment> Equipments
        {
            get => this.equipments;
            set => this.SetProperty(ref this.equipments, value);
        }

        /// <summary>
        /// Gets or sets the equipment ID.
        /// </summary>
        public int EquipmentID
        {
            get => this.equipmentID;
            set
            {
                this.equipmentID = value;
                this.OnPropertyChanged(nameof(this.EquipmentID));
                this.OnPropertyChanged(nameof(this.CanDeleteEquipment));
            }
        }

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        public string ErrorMessage
        {
            get => this.errorMessage;
            set
            {
                this.errorMessage = value;
                this.MessageColor = string.IsNullOrEmpty(value) ? "Red" : value.Contains("successfully") ? "Green" : "Red";
                this.OnPropertyChanged(nameof(this.ErrorMessage));
                this.OnPropertyChanged(nameof(this.MessageColor));
            }
        }

        /// <summary>
        /// Gets or sets the message color.
        /// </summary>
        public string MessageColor
        {
            get => this.messageColor;
            set
            {
                this.messageColor = value;
                this.OnPropertyChanged(nameof(this.MessageColor));
            }
        }

        /// <summary>
        /// Gets the command to delete the equipment.
        /// </summary>
        public ICommand DeleteEquipmentCommand { get; }

        /// <summary>
        /// Gets a value indicating whether the equipment can be deleted.
        /// </summary>
        public bool CanDeleteEquipment => this.EquipmentID > 0;

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="propertyName">The name of the property that changed.</param>
        protected void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Removes the equipment from the database.
        /// </summary>
        private void RemoveEquipment()
        {
            if (this.EquipmentID == 0)
            {
                this.ErrorMessage = "No equipment was selected";
                return;
            }

            if (!this.equipmentModel.DoesEquipmentExist(this.EquipmentID))
            {
                this.ErrorMessage = "EquipmentID doesn't exist in the records";
                return;
            }

            bool success = this.equipmentModel.DeleteEquipment(this.EquipmentID);
            this.ErrorMessage = success ? "Equipment deleted successfully" : "Failed to delete equipment";

            if (success)
            {
                this.Equipments = new ObservableCollection<Equipment>(this.equipmentModel.GetEquipments());
            }
        }

        /// <summary>
        /// Sets the property value and raises the <see cref="PropertyChanged"/> event if the value has changed.
        /// </summary>
        /// <typeparam name="T">The type of the property.</typeparam>
        /// <param name="field">The field to set.</param>
        /// <param name="value">The value to set.</param>
        /// <param name="propertyName">The name of the property. This is optional and will be automatically set by the compiler.</param>
        private void SetProperty<T>(ref T field, T value, [System.Runtime.CompilerServices.CallerMemberName] string propertyName = null!)
        {
            if (!EqualityComparer<T>.Default.Equals(field, value))
            {
                field = value;
                this.OnPropertyChanged(propertyName);
            }
        }
    }
}