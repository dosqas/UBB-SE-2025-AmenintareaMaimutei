// <copyright file="RealDispatcherTimer.cs" company="DuoISS">
// Copyright (c) DuoISS. All rights reserved.
// </copyright>

namespace Duo.Helpers.Timers
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Duo.Helpers.Interfaces;
    using Microsoft.UI.Xaml;

    #pragma warning disable IDE0079 // Remove unnecessary suppression
    #pragma warning disable SA1009 // Closing parenthesis should be spaced correctly

    /// <summary>
    /// Wrapper class for the UWP DispatcherTimer that implements our custom IDispatcherTimer interface.
    /// This class is excluded from code coverage since it cannot be properly tested/mocked due to its
    /// tight coupling with the Windows Runtime (WinRT) in UWP, which causes COM exceptions during testing.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the RealDispatcherTimer class.
    /// </remarks>
    /// <param name="dispatcherTimer">
    /// Optional DispatcherTimer instance to wrap. If null, creates a new instance.
    /// This parameter exists primarily to support testing scenarios.
    /// </param>
    [ExcludeFromCodeCoverage]
    public class RealDispatcherTimer(IDispatcherTimer? dispatcherTimer = null) : IDispatcherTimer
    {
        private readonly DispatcherTimer dispatcherTimer = dispatcherTimer as DispatcherTimer ?? new DispatcherTimer();

        /// <summary>
        /// Event that occurs when the timer interval has elapsed
        /// </summary>
        /// <remarks>
        /// The event signature is adapted to match EventHandler&lt;object&gt; to maintain
        /// compatibility with both the UWP DispatcherTimer and our mockable interface
        /// </remarks>
        public event EventHandler<object> Tick
        {
            add => this.dispatcherTimer.Tick += (sender, e) => value(sender, e);
            remove => this.dispatcherTimer.Tick -= (sender, e) => value(sender, e);
        }

        /// <summary>
        /// Gets or sets the amount of time between timer ticks.
        /// </summary>
        public TimeSpan Interval
        {
            get => this.dispatcherTimer.Interval;
            set => this.dispatcherTimer.Interval = value;
        }

        /// <summary>
        /// Starts the timer.
        /// </summary>
        public void Start() => this.dispatcherTimer.Start();

        /// <summary>
        /// Stops the timer.
        /// </summary>
        public void Stop() => this.dispatcherTimer.Stop();
    }
}
