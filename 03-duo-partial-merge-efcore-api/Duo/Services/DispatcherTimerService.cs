namespace Duo.Services
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Duo.Services.Helpers;

    /// <summary>
    /// Service implementation for a dispatcher timer that raises events at specified intervals.
    /// Wraps platform-specific timer implementations to provide a testable interface.
    /// </summary>
    /// <remarks>
    /// This service manages timer operations and propagates tick events while properly
    /// handling resource cleanup through IDisposable.
    /// </remarks>
    public partial class DispatcherTimerService : IDispatcherTimerService, IDisposable
    {
        #region Constants

        /// <summary>Default timer interval in milliseconds (1 second)</summary>
        private const int DefaultIntervalMilliseconds = 1000;
        #endregion

        #region Fields

        /// <summary>The underlying timer implementation</summary>
        private IDispatcherTimer timer;
        #endregion

        #region Events

        /// <summary>
        /// Occurs when the timer interval has elapsed
        /// </summary>
        public event EventHandler? Tick;
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the DispatcherTimerService class
        /// </summary>
        /// <param name="timer">
        /// Optional timer implementation to wrap. If null, creates a new RealDispatcherTimer
        /// with default interval. This parameter enables dependency injection for testing.
        /// </param>
        public DispatcherTimerService(IDispatcherTimer? timer = null)
        {
            InitializeTimer(timer);

            this.timer!.Tick += OnTimerTick!;
        }

        [ExcludeFromCodeCoverage]
        private void InitializeTimer(IDispatcherTimer? timer)
        {
            this.timer = timer ?? new RealDispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(DefaultIntervalMilliseconds)
            };
        }
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the time between timer ticks
        /// </summary>
        /// <value>
        /// A TimeSpan representing the interval between ticks.
        /// Set to TimeSpan.Zero to disable periodic ticking.
        /// </value>
        public TimeSpan Interval
        {
            get => timer.Interval;
            set => timer.Interval = value;
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Starts the timer
        /// </summary>
        /// <remarks>
        /// If the timer is already running, has no effect.
        /// The first tick will occur after Interval elapses.
        /// </remarks>
        public void Start() => timer.Start();

        /// <summary>
        /// Stops the timer
        /// </summary>
        /// <remarks>
        /// If the timer is not running, has no effect.
        /// No more ticks will occur until Start is called again.
        /// </remarks>
        public void Stop() => timer.Stop();

        /// <summary>
        /// Simulates a tick. Used for testing purposes.
        /// </summary>
        public void SimulateTick()
        {
            // Manually raise the Tick event as if the timer fired
            OnTimerTick(this, EventArgs.Empty);
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Handles the underlying timer's Tick event and propagates it through our own Tick event
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Event arguments</param>
        private void OnTimerTick(object sender, object e) => Tick!.Invoke(this, EventArgs.Empty);
        #endregion

        #region IDisposable Implementation

        /// <summary>
        /// Disposes the timer by unsubscribing from events and stopping it
        /// </summary>
        /// <remarks>
        /// Always call Dispose when done with the timer to prevent memory leaks
        /// from event handler references.
        /// </remarks>
        public void Dispose()
        {
            timer.Tick -= OnTimerTick!;
            timer.Stop();
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}