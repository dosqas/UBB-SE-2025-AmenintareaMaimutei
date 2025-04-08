namespace Project.Gui
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using Project.ClassModels;
    using Project.Models;

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DoctorsPage : Page
    {
        private readonly DoctorModel doctorModel = new ();

        private Dictionary<string, ListSortDirection> sortingStates = new Dictionary<string, ListSortDirection>
        {
            { "DoctorID", ListSortDirection.Ascending },
            { "Experience", ListSortDirection.Ascending },
            { "Rating", ListSortDirection.Ascending },
        };

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
        /// Gets or Sets the Doctors.
        /// </summary>
        public ObservableCollection<Doctor> Doctors { get; set; } = new ();

        /// <summary>
        /// Gets or Sets the doctorID.
        /// </summary>
        public int DoctorID { get; set; }

        private void LoadDoctors()
        {
            this.Doctors.Clear();
            List<Doctor> doctors = this.doctorModel.GetDoctors();
            foreach (Doctor doctor in doctors)
            {
                this.Doctors.Add(doctor);
            }

            ObservableCollection<Doctor> sorted = this.SortDoctors(this.Doctors, "DoctorId", ListSortDirection.Ascending);
            this.Doctors.Clear();

            foreach (Doctor doctor in sorted)
            {
                this.Doctors.Add(doctor);
            }
        }

        private void MoreInfoClick(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is Doctor doctor)
            {
                this.Frame.Navigate(typeof(DoctorInfoPage), doctor);
            }
        }

        private void SortByDoctorID(object sender, RoutedEventArgs e)
        {
            this.ToggleSort("DoctorID");
        }

        private void SortByRating(object sender, RoutedEventArgs e)
        {
            this.ToggleSort("Rating");
        }

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

        private ObservableCollection<Doctor> SortDoctors(ObservableCollection<Doctor> doctors, string field, ListSortDirection direction)
        {
            List<Doctor> sortedDoctors = doctors.ToList();
            if (field == "DoctorID")
            {
                if (direction == ListSortDirection.Ascending)
                {
                    sortedDoctors.Sort((x, y) => x.DoctorID.CompareTo(y.DoctorID));
                }
                else
                {
                    sortedDoctors.Sort((x, y) => y.DoctorID.CompareTo(x.DoctorID));
                }
            }
            else if (field == "Rating")
            {
                if (direction == ListSortDirection.Ascending)
                {
                    sortedDoctors.Sort((x, y) => x.Rating.CompareTo(y.Rating));
                }
                else
                {
                    sortedDoctors.Sort((x, y) => y.Rating.CompareTo(x.Rating));
                }
            }

            return new ObservableCollection<Doctor>(sortedDoctors);
        }

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
