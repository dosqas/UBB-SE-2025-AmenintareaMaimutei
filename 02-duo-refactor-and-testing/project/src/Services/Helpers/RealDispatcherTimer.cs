#pragma warning disable IDE0290

namespace CourseApp.Services.Helpers
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.UI.Xaml;

    /// <summary>
    /// Wrapper class for the UWP DispatcherTimer that implements our custom IDispatcherTimer interface.
    /// This class is excluded from code coverage since it cannot be properly tested/mocked due to its
    /// tight coupling with the Windows Runtime (WinRT) in UWP, which causes COM exceptions during testing.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class RealDispatcherTimer : IDispatcherTimer
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the RealDispatcherTimer class
        /// </summary>
        /// <param name="dispatcherTimer">
        /// Optional DispatcherTimer instance to wrap. If null, creates a new instance.
        /// This parameter exists primarily to support testing scenarios.
        /// </param>
        public RealDispatcherTimer(IDispatcherTimer? dispatcherTimer = null) =>
            this.dispatcherTimer = dispatcherTimer as DispatcherTimer ?? new DispatcherTimer();

        #endregion

        #region Fields
        private readonly DispatcherTimer dispatcherTimer;
        #endregion

        #region Events

        /// <summary>
        /// Event that occurs when the timer interval has elapsed
        /// </summary>
        /// <remarks>
        /// The event signature is adapted to match EventHandler&lt;object&gt; to maintain
        /// compatibility with both the UWP DispatcherTimer and our mockable interface
        /// </remarks>
        public event EventHandler<object> Tick
        {
            add => dispatcherTimer.Tick += (sender, e) => value(sender, e);
            remove => dispatcherTimer.Tick -= (sender, e) => value(sender, e);
        }
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the amount of time between timer ticks
        /// </summary>
        public TimeSpan Interval
        {
            get => dispatcherTimer.Interval;
            set => dispatcherTimer.Interval = value;
        }
        #endregion

        #region Methods

        /// <summary>
        /// Starts the timer
        /// </summary>
        public void Start() => dispatcherTimer.Start();

        /// <summary>
        /// Stops the timer
        /// </summary>
        public void Stop() => dispatcherTimer.Stop();
        #endregion
    }
}
