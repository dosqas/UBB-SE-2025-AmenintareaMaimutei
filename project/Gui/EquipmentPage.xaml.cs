namespace Project.Gui
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Microsoft.UI.Xaml.Controls;
    using Project.ClassModels;
    using Project.Models;

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EquipmentPage : Page
    {
        private readonly EquipmentModel equipmentModel = new ();

        /// <summary>
        /// Initializes a new instance of the <see cref="EquipmentPage"/> class.
        /// </summary>
        public EquipmentPage()
        {
            this.InitializeComponent();
            this.LoadEquiptment();
        }

        /// <summary>
        /// Gets or Sets the Equipments.
        /// </summary>
        public ObservableCollection<Equipment> Equipments { get; set; } = new ();

        private void LoadEquiptment()
        {
            this.Equipments.Clear();
            List<Equipment> equipments = this.equipmentModel.GetEquipments();
            foreach (Equipment equipment in equipments)
            {
                this.Equipments.Add(equipment);
            }
        }
    }
}
