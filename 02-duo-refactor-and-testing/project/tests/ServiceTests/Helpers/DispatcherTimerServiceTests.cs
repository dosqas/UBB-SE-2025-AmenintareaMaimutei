namespace Tests.ServiceTests.Helpers
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using CourseApp.Services;
    using CourseApp.Services.Helpers;
    using Moq;
    using Xunit;

    /// <summary>
    /// Unit tests for <see cref="DispatcherTimerService"/>.
    /// Verifies timer functionality including start/stop operations, interval setting,
    /// event propagation, and proper cleanup.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class DispatcherTimerServiceTests
    {
        private readonly Mock<IDispatcherTimer> mockTimer;
        private readonly DispatcherTimerService timerService;
        private bool eventRaised;

        /// <summary>
        /// Initializes a new instance of the <see cref="DispatcherTimerServiceTests"/> class.
        /// Initializes test dependencies and creates the service under test.
        /// Configures a mock timer with settable Interval property.
        /// </summary>
        public DispatcherTimerServiceTests()
        {
            this.mockTimer = new Mock<IDispatcherTimer>();
            this.mockTimer.SetupProperty(t => t.Interval);
            this.timerService = new DispatcherTimerService(this.mockTimer.Object);
            this.eventRaised = false;
        }

        /// <summary>
        /// Verifies that the Interval property correctly proxies to the underlying timer.
        /// </summary>
        [Fact]
        public void Interval_Property_SetsAndGetsCorrectly()
        {
            // Arrange
            var newInterval = TimeSpan.FromMilliseconds(500);

            // Act
            this.timerService.Interval = newInterval;

            // Assert
            Assert.Equal(newInterval, this.timerService.Interval);
            this.mockTimer.VerifySet(t => t.Interval = newInterval);
        }

        /// <summary>
        /// Verifies that Start() properly initiates the underlying timer.
        /// Ensures the start method is called exactly once.
        /// </summary>
        [Fact]
        public void Start_Method_InitiatesTimer()
        {
            // Act
            this.timerService.Start();

            // Assert
            this.mockTimer.Verify(t => t.Start(), Times.Once);
        }

        /// <summary>
        /// Verifies that Stop() properly halts the underlying timer.
        /// Ensures the stop method is called exactly once.
        /// </summary>
        [Fact]
        public void Stop_Method_HaltsTimer()
        {
            // Act
            this.timerService.Stop();

            // Assert
            this.mockTimer.Verify(t => t.Stop(), Times.Once);
        }

        /// <summary>
        /// Verifies that the service correctly propagates tick events from the underlying timer.
        /// Confirms event subscribers are notified when the timer ticks.
        /// </summary>
        [Fact]
        public void OnTimerTick_RaisesTickEvent()
        {
            // Arrange
            this.timerService.Tick += (sender, e) => this.eventRaised = true;

            // Act
            this.mockTimer.Raise(t => t.Tick += null, this, EventArgs.Empty);

            // Assert
            Assert.True(this.eventRaised);
        }

        /// <summary>
        /// Verifies that Dispose() properly cleans up event handlers and stops the timer.
        /// Checks both event handler removal and timer stop operations.
        /// </summary>
        [Fact]
        public void Dispose_CleansUpEventHandlerAndStopsTimer()
        {
            // Act
            this.timerService.Dispose();

            // Assert - Verify event handler removal
            this.mockTimer.VerifyRemove(
                t => t.Tick -= It.IsAny<EventHandler<object>>(),
                Times.Once);

            // Assert - Verify timer stop
            this.mockTimer.Verify(t => t.Stop(), Times.Once);
        }

        /// <summary>
        /// Verifies that SimulateTick correctly triggers the Tick event.
        /// </summary>
        [Fact]
        public void SimulateTick_ShouldTriggerTickEvent()
        {
            // Arrange
            var eventTriggered = false;
            this.timerService.Tick += (sender, args) => eventTriggered = true;

            // Act
            this.timerService.SimulateTick();  // Simulate the Tick event

            // Assert
            Assert.True(eventTriggered, "Tick event was not triggered by SimulateTick.");
        }
    }
}
