namespace Project.Gui
{
    using System.Collections.ObjectModel;
    using Microsoft.UI.Xaml.Controls;
    using Project.ClassModels;
    using Project.Models;

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    ///
    public sealed partial class ScheduleAndShifts : Page
    {
        private readonly ShiftModel shiftModel = new ();
        private readonly ScheduleModel scheduleModel = new ();

        /// <summary>
        /// Initializes a new instance of the <see cref="ScheduleAndShifts"/> class.
        /// </summary>
        public ScheduleAndShifts()
        {
            this.InitializeComponent();
            this.Load();
        }

        /// <summary>
        /// Gets or Sets the Shifts.
        /// </summary>
        public ObservableCollection<Shift> Shifts { get; set; } = new ();

        /// <summary>
        /// Gets or Sets the Schedules.
        /// </summary>
        public ObservableCollection<Schedule> Schedules { get; set; } = new ();

        private void Load()
        {
            this.Shifts.Clear();
            foreach (Shift shift in this.shiftModel.GetShifts())
            {
                this.Shifts.Add(shift);
            }

            this.Schedules.Clear();
            foreach (Schedule schedule in this.scheduleModel.GetSchedules())
            {
                this.Schedules.Add(schedule);
            }
        }
    }
}
