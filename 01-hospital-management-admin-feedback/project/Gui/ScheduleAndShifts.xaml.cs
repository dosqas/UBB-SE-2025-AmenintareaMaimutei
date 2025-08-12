using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    /// 

    public sealed partial class ScheduleAndShifts : Page
    {
        public ObservableCollection<Shift> Shifts { get; set; } = new();
        public ObservableCollection<Schedule> Schedules { get; set; } = new();

        private readonly ShiftModel _shiftModel = new();
        private readonly ScheduleModel _scheduleModel = new();
        public ScheduleAndShifts()
        {
            this.InitializeComponent();
            Load();
        }

        private void Load()
        {
            Shifts.Clear();
            foreach (Shift shift in _shiftModel.GetShifts())
            {
                Shifts.Add(shift);
            }
            Schedules.Clear();
            foreach (Schedule schedule in _scheduleModel.GetSchedules())
            {
                Schedules.Add(schedule);
            }
        }
    }
}
