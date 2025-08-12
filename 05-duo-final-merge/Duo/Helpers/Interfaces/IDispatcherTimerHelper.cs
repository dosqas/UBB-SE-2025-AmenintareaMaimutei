// <copyright file="IDispatcherTimerHelper.cs" company="DuoISS">
// Copyright (c) DuoISS. All rights reserved.
// </copyright>

namespace Duo.Helpers.Interfaces
{
    using System;

    /// <summary>
    /// Defines a service contract for a dispatcher timer that raises events at specified intervals.
    /// Provides platform-agnostic timer functionality suitable for UI and background operations.
    /// </summary>
    /// <remarks>
    /// This interface abstracts timer implementations to enable:
    /// - Testability through mocking
    /// - Dependency injection
    /// - Consistent behavior across platforms.
    /// </remarks>
    public interface IDispatcherTimerHelper
    {
        /// <summary>
        /// Occurs when the timer interval has elapsed.
        /// </summary>
        /// <remarks>
        /// Event handlers will be invoked on the thread/context appropriate for the implementation.
        /// For UI timers, this typically means the UI thread.
        /// </remarks>
        event EventHandler Tick;

        /// <summary>
        /// Gets or sets the time interval between timer ticks.
        /// </summary>
        /// <value>
        /// A <see cref="TimeSpan"/> representing the duration between ticks.
        /// Set to <see cref="TimeSpan.Zero"/> to disable periodic ticking.
        /// </value>
        TimeSpan Interval { get; set; }

        /// <summary>
        /// Starts the timer.
        /// </summary>
        /// <remarks>
        /// - If the timer is already running, has no effect.
        /// - The first tick will occur after <see cref="Interval"/> elapses.
        /// - Subsequent ticks occur at each <see cref="Interval"/> period.
        /// </remarks>
        void Start();

        /// <summary>
        /// Stops the timer.
        /// </summary>
        /// <remarks>
        /// - If the timer is not running, has no effect.
        /// - No more ticks will occur until <see cref="Start"/> is called again.
        /// - Does not reset the <see cref="Interval"/> value.
        /// </remarks>
        void Stop();

        /// <summary>
        /// Manually triggers a timer tick event for testing purposes, bypassing the actual timer interval.
        /// </summary>
        /// <remarks>
        /// **Purpose:**
        /// - Enables deterministic testing of timer-dependent logic without waiting for real-time intervals.
        /// - Useful for unit/integration tests to verify behavior when the timer elapses.
        ///
        /// **Testing Scenarios:**
        /// - Verify side effects of timer ticks (e.g., UI updates, state changes).
        /// </remarks>
        void SimulateTick();
    }
}