namespace Project.Gui
{
    using System.Collections.ObjectModel;
    using Microsoft.UI.Xaml.Controls;
    using Project.ClassModels;
    using Project.Models;

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Drugs : Page
    {
        private readonly DrugModel drugModel = new ();

        /// <summary>
        /// Initializes a new instance of the <see cref="Drugs"/> class.
        /// </summary>
        public Drugs()
        {
            this.InitializeComponent();
            this.Load();
        }

        /// <summary>
        /// Gets or Sets the DrugList.
        /// </summary>
        public ObservableCollection<Drug> DrugsList { get; set; } = new ();

        private void Load()
        {
            this.DrugsList.Clear();
            foreach (Drug drug in this.drugModel.GetDrugs())
            {
                this.DrugsList.Add(drug);
            }
        }
    }
}
