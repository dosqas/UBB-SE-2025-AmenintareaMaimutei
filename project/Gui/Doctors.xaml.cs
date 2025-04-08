// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Doctors.xaml.cs" company="YourCompany">
//   Copyright (c) YourCompany. All rights reserved.
// </copyright>
// <summary>
//   This file contains the code-behind for the DoctorsPage in the GUI.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Project.Gui
{
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
    /// A page that displays a list of doctors with sorting and search functionality.
    /// </summary>
    public sealed partial class DoctorsPage : Page
    {
        /// <summary>
        /// Gets or sets the list of doctors.
        /// </summary>
        public ObservableCollection<Doctor> Doctors { get; set; } = new ();

        private DoctorModel doctorModel = new ();

        private Dictionary<string, ListSortDirection> sortingStates = new ()
        {
            { "DoctorID", ListSortDirection.Ascending },
            { "Experience", ListSortDirection.Ascending },
            { "Rating", ListSortDirection.Ascending },
        };

        /// <summary>
        /// Gets or sets the doctor ID.
        /// </summary>
        public int DoctorID { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DoctorsPage"/> class.
        /// </summary>
        public DoctorsPage()
        {
            this.InitializeComponent();
            this.LoadDoctors();
            this.DataContext = this;
        }

        /// <summary>
        /// Loads and sorts the list of doctors.
        /// </summary>
        private void LoadDoctors()
        {
            this.Doctors.Clear();
            List<Doctor> doctorsList = this.doctorModel.GetDoctors();

            foreach (Doctor doctor in doctorsList)
            {
                this.Doctors.Add(doctor);
            }

            var sorted = this.SortDoctors(this.Doctors, "DoctorID", ListSortDirection.Ascending);
            this.Doctors.Clear();

            foreach (var doctor in sorted)
            {
                this.Doctors.Add(doctor);
            }
        }

        /// <summary>
        /// Handles the More Info button click event.
        /// </summary>
        private void MoreInfoClick(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is Doctor doctor)
            {
                this.Frame.Navigate(typeof(DoctorInfoPage), doctor);
            }
        }

        /// <summary>
        /// Handles sorting by doctor ID.
        /// </summary>
        private void SortByDoctorID(object sender, RoutedEventArgs e)
        {
            this.ToggleSort("DoctorID");
        }

        /// <summary>
        /// Handles sorting by rating.
        /// </summary>
        private void SortByRating(object sender, RoutedEventArgs e)
        {
            this.ToggleSort("Rating");
        }

        /// <summary>
        /// Toggles the sort direction and reorders the list.
        /// </summary>
        private void ToggleSort(string field)
        {
            if (this.sortingStates[field] == ListSortDirection.Ascending)
            {
                this.sortingStates[field] = ListSortDirection.Descending;
            }
            else
            {
                this.sortingStates[field] = ListSortDirection.Ascending;
            }

            var sortedDoctors = this.SortDoctors(this.Doctors, field, this.sortingStates[field]);
            this.Doctors.Clear();
            foreach (var doctor in sortedDoctors)
            {
                this.Doctors.Add(doctor);
            }
        }

        /// <summary>
        /// Sorts the doctor list based on a given field and direction.
        /// </summary>
        private ObservableCollection<Doctor> SortDoctors(ObservableCollection<Doctor> doctors, string field, ListSortDirection direction)
        {
            List<Doctor> sortedDoctors = doctors.ToList();

            if (field == "DoctorID")
            {
                sortedDoctors = direction == ListSortDirection.Ascending
                    ? sortedDoctors.OrderBy(x => x.DoctorID).ToList()
                    : sortedDoctors.OrderByDescending(x => x.DoctorID).ToList();
            }
            else if (field == "Rating")
            {
                sortedDoctors = direction == ListSortDirection.Ascending
                    ? sortedDoctors.OrderBy(x => x.Rating).ToList()
                    : sortedDoctors.OrderByDescending(x => x.Rating).ToList();
            }

            return new ObservableCollection<Doctor>(sortedDoctors);
        }

        /// <summary>
        /// Handles the search box text change event.
        /// </summary>
        private async void SearchBox_TextChange(object sender, RoutedEventArgs e)
        {
            string search = this.SearchTextBox.Text.Trim();

            if (string.IsNullOrEmpty(search))
            {
                this.LoadDoctors();
                return;
            }

            var filteredDoctors = this.Doctors.Where(doctor => doctor.UserID.ToString().Contains(search)).ToList();

            if (filteredDoctors.Count == 0)
            {
                var dialog = new ContentDialog
                {
                    Title = "No Results",
                    Content = "No doctors found with this id.",
                    CloseButtonText = "OK",
                    XamlRoot = this.XamlRoot,
                };

                await dialog.ShowAsync();
                return;
            }

            this.Doctors.Clear();
            foreach (var doctor in filteredDoctors)
            {
                this.Doctors.Add(doctor);
            }
        }
    }
}