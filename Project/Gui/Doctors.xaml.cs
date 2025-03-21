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
        public DoctorsPage()
        {
            this.InitializeComponent();
            loadDoctors();
        }
        public static int Guid2Int(Guid value)
        {
            byte[] b = value.ToByteArray();
            int bint = BitConverter.ToInt32(b, 0);
            return bint;
        }
        private void loadDoctors()
        {
            //Doctors.Clear();
            //List<Doctor> doctors = _doctorModel.GetDoctors();
            //foreach (Doctor doctor in doctors)
            //{
            //    Doctors.Add(doctor);
            //}
            Doctors.Clear();
            Doctors.Add(new Doctor
            {
                DoctorID = Guid.NewGuid(),
                UserID = Guid.NewGuid(),
                DepartmentID = Guid.NewGuid(),
                Experience = 5,
                LicenseNumber = "123456"
            });
            Doctors.Add(new Doctor
            {
                DoctorID = Guid.NewGuid(),
                UserID = Guid.NewGuid(),
                DepartmentID = Guid.NewGuid(),
                Experience = 10,
                LicenseNumber = "654321"
            });
        }
        private void MoreInfoClick(object sender, RoutedEventArgs e)
        {

        }
    }
}
