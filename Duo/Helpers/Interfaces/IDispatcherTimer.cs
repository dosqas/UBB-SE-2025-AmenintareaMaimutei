// <copyright file="IDispatcherTimer.cs" company="DuoISS">
// Copyright (c) DuoISS. All rights reserved.
// </copyright>

namespace Duo.Helpers.Interfaces
{
    using System;

    /// <summary>
    /// Interface representing a dispatcher timer service, providing timer functionality
    /// that can raise events on the UI thread. This abstraction allows for testability
    /// by mocking the timer behavior in unit tests.
    /// </summary>
    public interface IDispatcherTimer
    {
        /// <summary>
        /// Event that occurs when the timer interval has elapsed.
        /// </summary>
        /// <remarks>
        /// The event uses EventHandler&lt;object&gt; as its signature to maintain
        /// compatibility with various timer implementations while providing flexibility
        /// for different use cases.
        /// </remarks>
        event EventHandler<object> Tick;

        /// <summary>
        /// Gets or sets the time interval between timer ticks.
        /// </summary>
        /// <value>
        /// A TimeSpan representing the duration between timer ticks.
        /// The minimum allowed interval may vary by platform implementation.
        /// </value>
        TimeSpan Interval { get; set; }

        /// <summary>
        /// Starts the timer.
        /// </summary>
        /// <remarks>
        /// If the timer is already running, calling Start() should have no effect.
        /// The first tick will occur after the Interval has elapsed.
        /// </remarks>
        void Start();

        /// <summary>
        /// Stops the timer.
        /// </summary>
        /// <remarks>
        /// If the timer is not running, calling Stop() should have no effect.
        /// No more ticks will occur until Start() is called again.
        /// </remarks>
        void Stop();
    }
}