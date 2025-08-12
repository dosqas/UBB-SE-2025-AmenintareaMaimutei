// <copyright file="DispatcherTimerHelper.cs" company="DuoISS">
// Copyright (c) DuoISS. All rights reserved.
// </copyright>

namespace Duo.Helpers.Timers
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Duo.Helpers.Interfaces;

    /// <summary>
    /// Service implementation for a dispatcher timer that raises events at specified intervals.
    /// Wraps platform-specific timer implementations to provide a testable interface.
    /// </summary>
    /// <remarks>
    /// This service manages timer operations and propagates tick events while properly
    /// handling resource cleanup through IDisposable.
    /// </remarks>
    public partial class DispatcherTimerHelper : IDispatcherTimerHelper, IDisposable
    {
        /// <summary>Default timer interval in milliseconds (1 second).</summary>
        private const int DefaultIntervalMilliseconds = 1000;

        /// <summary>The underlying timer implementation.</summary>
        private IDispatcherTimer timer;

        /// <summary>
        /// Initializes a new instance of the <see cref="DispatcherTimerHelper"/> class.
        /// </summary>
        /// <param name="timer">
        /// Optional timer implementation to wrap. If null, creates a new RealDispatcherTimer
        /// with default interval. This parameter enables dependency injection for testing.
        /// </param>
        public DispatcherTimerHelper(IDispatcherTimer? timer = null)
        {
            this.InitializeTimer(timer);
            this.timer!.Tick += this.OnTimerTick!;
        }

        /// <summary>
        /// Occurs when the timer interval has elapsed
        /// </summary>
        public event EventHandler? Tick;

        /// <summary>
        /// Gets or sets the time between timer ticks.
        /// </summary>
        /// <value>
        /// A TimeSpan representing the interval between ticks.
        /// Set to TimeSpan.Zero to disable periodic ticking.
        /// </value>
        public TimeSpan Interval
        {
            get => this.timer.Interval;
            set => this.timer.Interval = value;
        }

        /// <summary>
        /// Starts the timer.
        /// </summary>
        /// <remarks>
        /// If the timer is already running, has no effect.
        /// The first tick will occur after Interval elapses.
        /// </remarks>
        public void Start() => this.timer.Start();

        /// <summary>
        /// Stops the timer.
        /// </summary>
        /// <remarks>
        /// If the timer is not running, has no effect.
        /// No more ticks will occur until Start is called again.
        /// </remarks>
        public void Stop() => this.timer.Stop();

        /// <summary>
        /// Simulates a tick. Used for testing purposes.
        /// </summary>
        public void SimulateTick()
        {
            this.OnTimerTick(this, EventArgs.Empty);
        }

        /// <summary>
        /// Disposes the timer by unsubscribing from events and stopping it.
        /// </summary>
        /// <remarks>
        /// Always call Dispose when done with the timer to prevent memory leaks
        /// from event handler references.
        /// </remarks>
        public void Dispose()
        {
            this.timer.Tick -= this.OnTimerTick!;
            this.timer.Stop();
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Handles the underlying timer's Tick event and propagates it through our own Tick event.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event arguments.</param>
        private void OnTimerTick(object sender, object e) => this.Tick!.Invoke(this, EventArgs.Empty);

        [ExcludeFromCodeCoverage]
        private void InitializeTimer(IDispatcherTimer? timer)
        {
            this.timer = timer ?? new RealDispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(DefaultIntervalMilliseconds),
            };
        }
    }
}
