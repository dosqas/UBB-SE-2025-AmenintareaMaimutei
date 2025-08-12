using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Project.Models;
using System.Collections.ObjectModel;
using Project.ClassModels;
using System.ComponentModel;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Project.Gui
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DoctorsPage : Page
    {
        public ObservableCollection<Doctor> Doctors { get; set; } = new();
        private readonly DoctorModel _doctorModel = new();

        private Dictionary<string, ListSortDirection> _sortingStates = new Dictionary<string, ListSortDirection>
        {
            { "DoctorID", ListSortDirection.Ascending },
            { "Experience", ListSortDirection.Ascending },
            { "Rating", ListSortDirection.Ascending }
        };

        public int DoctorID { get; set; }  
        public DoctorsPage()
        {
            this.InitializeComponent();
            loadDoctors();
            DataContext = this;
        }

        private void loadDoctors()
        {
            Doctors.Clear();
            List<Doctor> doctors = _doctorModel.GetDoctors();
            foreach (Doctor doctor in doctors)
            {
                Doctors.Add(doctor);
            }
    
            var Sorted = SortDoctors(Doctors, "DoctorId", ListSortDirection.Ascending);
            Doctors.Clear();

            foreach (var Doctor in Sorted)
            {
                Doctors.Add(Doctor);
            }
        }
        private void MoreInfoClick(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is Doctor doctor)
            {
                Frame.Navigate(typeof(DoctorInfoPage), doctor);
            }
        }
        private void SortByDoctorID(object sender, RoutedEventArgs e)
        {
            ToggleSort("DoctorID");
        }
        private void SortByRating(object sender, RoutedEventArgs e)
        {
            ToggleSort("Rating");
        }
        private void ToggleSort(string field)
        {
            if (_sortingStates[field] == ListSortDirection.Ascending)
            {
                _sortingStates[field] = ListSortDirection.Descending;
            }
            else
            {
                _sortingStates[field] = ListSortDirection.Ascending;
            }
            var sortedDoctors = SortDoctors(Doctors, field, _sortingStates[field]);
            Doctors.Clear();
            foreach (var doctor in sortedDoctors)
            {
                Doctors.Add(doctor);
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
            string search = SearchTextBox.Text.Trim();

            if (string.IsNullOrEmpty(search))
            {
                loadDoctors();
                return;
            }

            var filteredDoctors = Doctors.Where(doctor => doctor.UserID.ToString().Contains(search)).ToList();

            if (filteredDoctors.Count == 0)
            {
                var dialog = new ContentDialog
                {
                    Title = "No Results",
                    Content = "No doctors found with this id.",
                    CloseButtonText = "OK",
                    XamlRoot = this.XamlRoot 
                };

                await dialog.ShowAsync();
                return;
            }

            Doctors.Clear();
            foreach (var doctor in filteredDoctors)
            {
                Doctors.Add(doctor);
            }
        }

    }
}
