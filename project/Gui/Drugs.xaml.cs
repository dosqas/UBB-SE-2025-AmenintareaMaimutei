// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Drugs.xaml.cs" company="YourCompany">
//   Copyright (c) YourCompany. All rights reserved.
// </copyright>
// <summary>
//   This file contains the code-behind for the Drugs page in the GUI.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Project.Gui
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices.WindowsRuntime;
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using Microsoft.UI.Xaml.Controls.Primitives;
    using Microsoft.UI.Xaml.Data;
    using Microsoft.UI.Xaml.Input;
    using Microsoft.UI.Xaml.Media;
    using Microsoft.UI.Xaml.Navigation;
    using Project.ClassModels;
    using Project.Models;
    using Windows.Foundation;
    using Windows.Foundation.Collections;

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Drugs : Page
    {
        /// <summary>
        /// Gets the collection of drugs to be displayed.
        /// </summary>
        public ObservableCollection<Drug> DrugsList { get; set; } = new ();
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
        /// Loads the drugs from the model and populates the observable collection.
        /// </summary>
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
