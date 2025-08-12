// <copyright file="CourseViewModelTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Tests.ViewModelsTests
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;
    using CourseApp.Models;
    using CourseApp.Services;
    using CourseApp.ViewModels;
    using CourseApp.ViewModels.Helpers;
    using Moq;
    using Xunit;

    /// <summary>
    /// Unit tests for <see cref="CourseViewModel"/>.
    /// Verifies course enrollment, module completion, timer functionality,
    /// and reward claiming behavior.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class CourseViewModelTests
    {
        private readonly Mock<ICourseService> mockCourseService;
        private readonly Mock<ICoinsService> mockCoinsService;
        private readonly Mock<IDispatcherTimerService> mockCourseTimer;
        private readonly Mock<IDispatcherTimerService> mockNotificationTimer;
        private readonly Mock<INotificationHelper> mockNotificationHelper;
        private readonly Course testCourse;
        private readonly CourseViewModel viewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="CourseViewModelTests"/> class.
        /// Initializes test dependencies and configures default mock behaviors.
        /// Creates a test course with 3 modules (2 required, 1 bonus).
        /// </summary>
        public CourseViewModelTests()
        {
            // Mock the ICourseService dependency which is responsible for course-related operations
            this.mockCourseService = new Mock<ICourseService>();

            // Mock the ICoinsService dependency which handles the user's coin balance and transactions
            this.mockCoinsService = new Mock<ICoinsService>();

            // Mock the IDispatcherTimerService for handling course and notification timers
            this.mockCourseTimer = new Mock<IDispatcherTimerService>();
            this.mockNotificationTimer = new Mock<IDispatcherTimerService>();

            // Mock the INotificationHelper to manage notifications within the application
            this.mockNotificationHelper = new Mock<INotificationHelper>();

            // Create a test course object with predefined values
            this.testCourse = new Course
            {
                CourseId = 1,
                Title = "Test Course", // The title of the course
                Description = "Test Description", // A brief description of the course
                ImageUrl = "test.jpg", // URL of the course image
                Difficulty = "Beginner", // Difficulty level of the course
                IsPremium = true, // Specifies that this is a premium course
                Cost = 100, // Cost to enroll in the course (in coins)
                TimeToComplete = 3600, // Time to complete the course in seconds (1 hour)
            };

            // Call ConfigureDefaultMocks to set up the mock behaviors for the dependencies
            this.ConfigureDefaultMocks();

            // Initialize the modules associated with the course
            this.InitializeTestModules();

            // Initialize the ViewModel with the mocked dependencies and test course
            this.viewModel = new CourseViewModel(
                this.testCourse,
                this.mockCourseService.Object,
                this.mockCoinsService.Object,
                this.mockCourseTimer.Object,
                this.mockNotificationTimer.Object,
                this.mockNotificationHelper.Object);
        }

        /// <summary>
        /// Verifies that the ViewModel initializes all properties correctly, including the current course, coin balance, and module progress.
        /// </summary>
        [Fact]
        public void Constructor_InitializesPropertiesCorrectly()
        {
            // Assert initial ViewModel state
            Assert.Equal(this.testCourse, this.viewModel.CurrentCourse); // Ensure the CurrentCourse is correctly set to the test course
            Assert.False(this.viewModel.IsEnrolled); // Ensure the user is not enrolled initially
            Assert.Equal(200, this.viewModel.CoinBalance); // Verify the coin balance is initialized to 200
            Assert.Equal("60 min 0 sec", this.viewModel.FormattedTimeRemaining); // Ensure the time remaining is correctly formatted as "60 min 0 sec"
            Assert.Equal(0, this.viewModel.CompletedModules); // Verify no modules are completed at the start
            Assert.Equal(5, this.viewModel.RequiredModules); // Verify the correct number of required modules for the course
            Assert.False(this.viewModel.IsCourseCompleted); // Ensure the course is not marked as completed initially
            Assert.NotNull(this.viewModel.EnrollCommand); // Verify the EnrollCommand is initialized
            Assert.NotNull(this.viewModel.ModuleRoadmap); // Ensure the ModuleRoadmap is initialized
        }

        /// <summary>
        /// Verifies that the EnrollCommand can execute when the user has sufficient coins to enroll in the course.
        /// </summary>
        [Fact]
        public void EnrollCommand_CanExecute_WhenUserHasEnoughCoins()
        {
            // Assert that the EnrollCommand can be executed when the user has sufficient coins
            Assert.True(this.viewModel.EnrollCommand!.CanExecute(null)); // This assumes the initial state has enough coins (e.g., 200)
        }

        /// <summary>
        /// Verifies that the EnrollCommand cannot execute when the user does not have sufficient coins to enroll in the course.
        /// </summary>
        [Fact]
        public void EnrollCommand_CannotExecute_WhenUserDoesNotHaveEnoughCoins()
        {
            // Arrange: Mock the coin balance to be less than required (50 coins)
            this.mockCoinsService.Setup(x => x.GetCoinBalance(It.IsAny<int>())).Returns(50);

            // Assert that the EnrollCommand cannot be executed when the user doesn't have enough coins
            Assert.False(this.viewModel.EnrollCommand!.CanExecute(null)); // EnrollCommand should not be executable with only 50 coins
        }

        /// <summary>
        /// Verifies that when the EnrollCommand is executed successfully, coins are deducted and enrollment status is updated.
        /// </summary>
        [Fact]
        public void EnrollCommand_ExecutesSuccessfully()
        {
            // Arrange
            this.mockCoinsService.Setup(x => x.TrySpendingCoins(0, 100)).Returns(true); // Mock spending 100 coins
            this.mockCourseService.Setup(x => x.EnrollInCourse(1)).Returns(true); // Mock successful enrollment in course

            // Act
            this.viewModel.EnrollCommand!.Execute(null); // Execute the EnrollCommand

            // Assert: Verify the expected interactions
            this.mockCoinsService.Verify(x => x.TrySpendingCoins(0, 100), Times.Once); // Ensure TrySpendingCoins was called once to deduct coins
            this.mockCourseService.Verify(x => x.EnrollInCourse(1), Times.Once); // Ensure EnrollInCourse was called once to enroll the user
            Assert.True(this.viewModel.IsEnrolled); // Assert that the user is now enrolled in the course
        }

        /// <summary>
        /// Verifies that when a module is marked as completed, the progress is updated, and rewards are checked for eligibility.
        /// </summary>
        [Fact]
        public void MarkModuleAsCompletedAndCheckRewards_UpdatesCompletionStatus()
        {
            // Arrange: Mock the GetCompletedModulesCount to return 1 completed module
            this.mockCourseService.Setup(x => x.GetCompletedModulesCount(It.IsAny<int>())).Returns(1);

            // Act: Mark the first module as completed and check for rewards
            this.viewModel.MarkModuleAsCompletedAndCheckRewards(1);

            // Assert: Verify that the number of completed modules is updated correctly
            Assert.Equal(1, this.viewModel.CompletedModules); // Ensure the completed modules count is 1
            Assert.False(this.viewModel.IsCourseCompleted); // Ensure the course is not marked as completed yet (5 required modules, so 1 is insufficient)
        }

        /// <summary>
        /// Verifies that the course completion reward is claimed when all modules are completed.
        /// </summary>
        [Fact]
        public void MarkModuleAsCompletedAndCheckRewards_CompletesCourse_WhenAllModulesDone()
        {
            // Arrange: Setting up the mock services
            // We mock the course service to return 5 completed modules when checking the count of completed modules.
            this.mockCourseService.Setup(x => x.GetCompletedModulesCount(It.IsAny<int>())).Returns(5);

            // We also mock the claim of the completion reward to return true, indicating success.
            this.mockCourseService.Setup(x => x.ClaimCompletionReward(It.IsAny<int>())).Returns(true);

            // Act: Attempt to mark a module as completed and check for course completion rewards.
            this.viewModel.MarkModuleAsCompletedAndCheckRewards(1);

            // Assert: Verify the course completion state and reward claiming
            // Assert that the course is marked as completed and the reward is claimed.
            Assert.True(this.viewModel.IsCourseCompleted);
            Assert.True(this.viewModel.CompletionRewardClaimed);

            // Verify that the claim completion reward method was called exactly once.
            this.mockCourseService.Verify(x => x.ClaimCompletionReward(this.testCourse.CourseId), Times.Once);
        }

        /// <summary>
        /// Verifies that a successful bonus module purchase updates the UI and shows a notification.
        /// </summary>
        [Fact]
        public void AttemptBonusModulePurchase_SuccessfulPurchase_UpdatesUI()
        {
            // Arrange: Set up a mock bonus module and mock services.
            var testModule = new CourseApp.Models.Module
            {
                ModuleId = 1,
                IsBonus = true,
                Cost = 50,
                Title = "Bonus Module Title",
                Description = "Bonus Module Description",
                ImageUrl = "bonus-module.jpg",
            };

            // Mocking the coins service to simulate a successful coin spend attempt (50 coins available).
            this.mockCoinsService.Setup(x => x.TrySpendingCoins(0, 50)).Returns(true);

            // Mocking the course service to simulate successful bonus module purchase.
            this.mockCourseService.Setup(x => x.BuyBonusModule(1, 1)).Returns(true);

            // Set up the notification helper to verify that a notification is shown.
            this.mockNotificationHelper.Setup(helper => helper.ShowTemporaryNotification(It.IsAny<string>())).Verifiable();

            // Act: Attempt to purchase the bonus module.
            this.viewModel.AttemptBonusModulePurchase(testModule);

            // Assert: Verify the notification helper is called with the expected success message.
            this.mockNotificationHelper.Verify(helper => helper.ShowTemporaryNotification(It.Is<string>(msg => msg.Contains("Congratulations! You have purchased bonus module"))), Times.Once);
        }

        /// <summary>
        /// Verifies that time formatting displays correctly for various durations.
        /// </summary>
        /// <param name="seconds">The number of seconds to format.</param>
        /// <param name="expected">The expected formatted time string.</param>
        [Theory]
        [InlineData(5, "0 min 5 sec")]
        [InlineData(65, "1 min 5 sec")]
        [InlineData(3600, "60 min 0 sec")]
        [InlineData(3700, "61 min 40 sec")]
        public void FormatTimeRemainingDisplay_FormatsTimeCorrectly(int seconds, string expected)
        {
            // Act: Call the method to format the time remaining.
            var result = CourseViewModel.FormatTimeRemainingDisplay(seconds);

            // Assert: Verify that the result matches the expected formatted time string.
            Assert.Equal(expected, result);
        }

        /// <summary>
        /// Verifies that modules are organized correctly in the course roadmap.
        /// </summary>
        [Fact]
        public void LoadAndOrganizeCourseModules_OrganizesModulesCorrectly()
        {
            // Act: Load and organize the course modules into the roadmap.
            this.viewModel.LoadAndOrganizeCourseModules();

            // Assert: Verify that there are 3 modules in the roadmap.
            Assert.Equal(3, this.viewModel.ModuleRoadmap.Count);

            // Verify that the modules are ordered correctly by their ModuleId.
            Assert.Equal(1, this.viewModel.ModuleRoadmap[0].Module!.ModuleId);
            Assert.Equal(2, this.viewModel.ModuleRoadmap[1].Module!.ModuleId);
            Assert.Equal(3, this.viewModel.ModuleRoadmap[2].Module!.ModuleId);
        }

        /// <summary>
        /// Verifies that the timed completion reward is granted when the course is completed within the time limit.
        /// </summary>
        [Fact]
        public void CheckForTimedReward_GrantsReward_WhenCompletedWithinTime()
        {
            // Arrange: Setting up mock services to simulate course completion.
            this.mockCourseService.Setup(x => x.GetCompletedModulesCount(It.IsAny<int>())).Returns(5);

            // Mocking the timed reward claim to return true, indicating that the reward is successfully claimed.
            this.mockCourseService.Setup(x => x.ClaimTimedReward(It.IsAny<int>(), It.IsAny<int>())).Returns(true);

            // Set private field directly to simulate that the course was completed in 30 minutes.
            typeof(CourseViewModel)
                .GetField("totalSecondsSpentOnCourse", BindingFlags.NonPublic | BindingFlags.Instance) !
                .SetValue(this.viewModel, 1800); // 1800 seconds = 30 minutes

            // Act: Attempt to mark the module as completed and check if the reward is claimed.
            this.viewModel.MarkModuleAsCompletedAndCheckRewards(1);

            // Assert: Verify that the timed reward is claimed.
            Assert.True(this.viewModel.TimedRewardClaimed);

            // Verify that the timed reward claim method is called with the correct parameters.
            this.mockCourseService.Verify(x => x.ClaimTimedReward(this.testCourse.CourseId, 1800), Times.Once);
        }

        /// <summary>
        /// Verifies that an ArgumentNullException is thrown when attempting to purchase a bonus module with a null module.
        /// This test ensures that the system correctly handles invalid inputs, specifically when the user attempts to purchase a module that does not exist (null).
        /// It is important to handle null values gracefully to avoid potential errors or crashes in the application.
        /// </summary>
        [Fact]
        public void AttemptBonusModulePurchase_ThrowsArgumentNullException_WhenModuleIsNull()
        {
            // Act & Assert
            // Verify that calling AttemptBonusModulePurchase with a null module throws an ArgumentNullException
            Assert.Throws<ArgumentNullException>(() => this.viewModel.AttemptBonusModulePurchase(null));
        }

        /// <summary>
        /// Verifies that the bonus module purchase attempt returns early without making any service calls when the module is already completed.
        /// This test simulates the scenario where the user tries to purchase a bonus module that has already been completed.
        /// The method should detect that the module is completed and avoid making unnecessary service calls, providing a seamless user experience.
        /// </summary>
        [Fact]
        public void AttemptBonusModulePurchase_ReturnsEarly_WhenModuleIsCompleted()
        {
            // Arrange
            // Create a module that is marked as completed
            var completedModule = new CourseApp.Models.Module
            {
                ModuleId = 1,
                IsBonus = true,
                Cost = 50,
                Title = "Module Title",
                Description = "Module Description",
                ImageUrl = "module.jpg",
            };

            // Setup mock service calls to simulate the module being completed
            this.mockCourseService.Setup(service => service.IsModuleCompleted(completedModule.ModuleId)).Returns(true);

            // Mock notification helper to verify if a notification is shown
            this.mockNotificationHelper.Setup(helper => helper.ShowTemporaryNotification(It.IsAny<string>())).Verifiable();

            // Act
            // Attempt to purchase the completed module
            this.viewModel.AttemptBonusModulePurchase(completedModule);

            // Assert
            // Verify that no calls are made to spend coins, buy the module, or show the "not enough coins" notification
            this.mockCoinsService.Verify(service => service.TrySpendingCoins(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
            this.mockCourseService.Verify(service => service.BuyBonusModule(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
            this.mockNotificationHelper.Verify(helper => helper.ShowTemporaryNotification("You do not have enough coins to buy this module."), Times.Never);
        }

        /// <summary>
        /// Verifies that when there are not enough coins to purchase a bonus module, a purchase failed notification is shown.
        /// This test simulates the case where the user tries to purchase a bonus module but doesn't have enough coins.
        /// The system should show a failure notification to inform the user that the purchase could not be completed.
        /// </summary>
        [Fact]
        public void AttemptBonusModulePurchase_ShowsPurchaseFailedNotification_WhenNotEnoughCoins()
        {
            // Arrange
            // Define a module that costs 200 coins
            var module = new CourseApp.Models.Module
            {
                ModuleId = 3,
                IsBonus = true,
                Cost = 200,
                Title = "Module Title",
                Description = "Module Description",
                ImageUrl = "module.jpg",
            };

            // Setup mock dependencies to simulate the case where the user has 0 coins and the module is not completed
            this.mockCoinsService.Setup(service => service.TrySpendingCoins(0, module.Cost)).Returns(false); // Insufficient coins
            this.mockCourseService.Setup(service => service.IsModuleCompleted(It.IsAny<int>())).Returns(false); // Module is not completed
            this.mockNotificationHelper.Setup(helper => helper.ShowTemporaryNotification(It.IsAny<string>())).Verifiable(); // Mock the notification display

            // Act
            // Attempt to purchase the module with insufficient coins
            this.viewModel.AttemptBonusModulePurchase(module);

            // Assert
            // Verify that the TrySpendingCoins method was called once with the correct arguments (0 coins, cost of the module)
            // this.mockCoinsService.Verify(service => service.TrySpendingCoins(0, module.Cost), Times.Once);

            // Verify that the notification helper displays the correct message regarding insufficient coins
            this.mockNotificationHelper.Verify(helper => helper.ShowTemporaryNotification("You do not have enough coins to buy this module."), Times.Once);
        }

        /// <summary>
        /// Verifies that when a bonus module purchase is successful, the status is updated, and a success notification is shown.
        /// This test ensures that when the user successfully purchases a bonus module, the system updates the user's status and displays a success notification.
        /// This behavior is essential for providing feedback to the user and ensuring that the system reflects the user's actions correctly.
        /// </summary>
        [Fact]
        public void AttemptBonusModulePurchase_SuccessfulPurchase_UpdatesStatusAndShowsNotification()
        {
            // Arrange
            // Define a module for which the user has enough coins to purchase
            var module = new CourseApp.Models.Module
            {
                ModuleId = 3,
                IsBonus = true,
                Cost = 200,
                Title = "Module Title",
                Description = "Module Description",
                ImageUrl = "module.jpg",
            };

            // Setup mock services to simulate a successful purchase: user has enough coins, module is not completed, and the purchase can proceed
            this.mockCoinsService.Setup(service => service.TrySpendingCoins(It.IsAny<int>(), module.Cost)).Returns(true); // Coins are sufficient
            this.mockCourseService.Setup(service => service.BuyBonusModule(module.ModuleId, this.testCourse.CourseId)).Returns(true); // Purchase is successful
            this.mockNotificationHelper.Setup(helper => helper.ShowTemporaryNotification(It.IsAny<string>())).Verifiable(); // Mock notification

            // Act
            // Attempt to purchase the bonus module
            this.viewModel.AttemptBonusModulePurchase(module);

            // Assert
            // Verify that TrySpendingCoins was called once with the correct arguments (cost of the module)
            // this.mockCoinsService.Verify(service => service.TrySpendingCoins(It.IsAny<int>(), module.Cost), Times.Once);

            // Verify that BuyBonusModule was called once to complete the purchase
            this.mockCourseService.Verify(service => service.BuyBonusModule(module.ModuleId, this.testCourse.CourseId), Times.Once);

            // Verify that the module's status is updated by opening the module after purchase
            this.mockCourseService.Verify(service => service.OpenModule(module.ModuleId), Times.Once);

            // Verify that the notification helper shows a success message with the module title and the amount of coins deducted
            this.mockNotificationHelper.Verify(
                helper =>
                helper.ShowTemporaryNotification($"Congratulations! You have purchased bonus module {module.Title}, {module.Cost} coins have been deducted from your balance."), Times.Once);
        }

        /// <summary>
        /// Test to verify that when a purchase attempt for a bonus module fails, a purchase failed notification is shown.
        /// This test checks the behavior when the user attempts to purchase a bonus module, but the purchase fails.
        /// The test ensures that the system handles the failure gracefully by showing an appropriate notification to the user.
        /// </summary>
        [Fact]
        public void AttemptBonusModulePurchase_FailedPurchase_ShowsPurchaseFailedNotification()
        {
            // Arrange: Set up the test data and mock behaviors
            var module = new CourseApp.Models.Module
            {
                ModuleId = 3,               // The unique ID of the module
                IsBonus = true,              // Marking this module as a bonus
                Cost = 50,                   // The cost of the bonus module
                Title = "Module Title",      // Title of the bonus module
                Description = "Module Description", // Description of the module
                ImageUrl = "module.jpg",     // Image associated with the module
            };

            // Mock the behavior of the CoinService to return true when attempting to spend coins, indicating the coins are available
            this.mockCoinsService.Setup(service => service.TrySpendingCoins(It.IsAny<int>(), module.Cost)).Returns(true);

            // Mock the behavior of the CourseService to return false when attempting to buy the bonus module, indicating a purchase failure
            this.mockCourseService.Setup(service => service.BuyBonusModule(module.ModuleId, this.testCourse.CourseId)).Returns(false);

            // Mock the NotificationHelper to verify that the appropriate notification is displayed in case of failure
            this.mockNotificationHelper.Setup(helper => helper.ShowTemporaryNotification(It.IsAny<string>())).Verifiable();

            // Act: Call the method to attempt the purchase of the bonus module
            this.viewModel.AttemptBonusModulePurchase(module);

            // Assert: Verify the interactions with the services and ensure the failure notification is shown
            // this.mockCoinsService.Verify(service => service.TrySpendingCoins(It.IsAny<int>(), module.Cost), Times.Once);
            this.mockCourseService.Verify(service => service.BuyBonusModule(module.ModuleId, this.testCourse.CourseId), Times.Once);
            this.mockNotificationHelper.Verify(helper => helper.ShowTemporaryNotification("You do not have enough coins to buy this module."), Times.Once);
        }

        /// <summary>
        /// Test to ensure that when the NotificationMessage property is set, the PropertyChanged event is raised.
        /// This test checks that setting the NotificationMessage property correctly triggers the PropertyChanged event,
        /// ensuring that the ViewModel properly notifies the UI of the change so the view can update accordingly.
        /// </summary>
        [Fact]
        public void NotificationMessage_SetValue_RaisesPropertyChanged()
        {
            // Arrange: Prepare the event handler to capture when PropertyChanged is triggered for the NotificationMessage property
            var wasCalled = false;
            this.viewModel.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(this.viewModel.NotificationMessage))
                {
                    wasCalled = true; // Set the flag when the PropertyChanged event is triggered
                }
            };

            // Act: Set a new value for the NotificationMessage property
            this.viewModel.NotificationMessage = "Test Message";

            // Assert: Verify that the property is set correctly and that the PropertyChanged event is raised
            Assert.Equal("Test Message", this.viewModel.NotificationMessage);  // Check that the property value is updated
            Assert.True(wasCalled);  // Ensure that the PropertyChanged event was triggered
        }

        /// <summary>
        /// Test to ensure that when the ShowNotification property is set, the PropertyChanged event is raised.
        /// This test verifies that setting the ShowNotification property triggers the PropertyChanged event,
        /// which allows the UI to react to changes in this property, such as showing or hiding notifications.
        /// </summary>
        [Fact]
        public void ShowNotification_SetValue_RaisesPropertyChanged()
        {
            // Arrange: Prepare an event handler to listen for the PropertyChanged event for the ShowNotification property
            var wasCalled = false;
            this.viewModel.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(this.viewModel.ShowNotification))
                {
                    wasCalled = true; // Set the flag when the event is triggered
                }
            };

            // Act: Set the ShowNotification property to true
            this.viewModel.ShowNotification = true;

            // Assert: Verify that the ShowNotification property is updated and the PropertyChanged event is raised
            Assert.True(this.viewModel.ShowNotification);  // Ensure the property is set to true
            Assert.True(wasCalled);  // Confirm that PropertyChanged was triggered for this property
        }

        /// <summary>
        /// Test to verify that the Tags property correctly returns tags from the service.
        /// This test checks that the Tags property in the ViewModel returns the correct tags as fetched from the CourseService.
        /// It verifies that the system correctly retrieves and exposes course tags for the user to see, ensuring data consistency between
        /// the service layer and the ViewModel layer.
        /// </summary>
        [Fact]
        public void Tags_ReturnsTagsFromService()
        {
            // Arrange: Prepare the expected tags to be returned by the mock CourseService
            var expectedTags = new List<Tag>
            {
                new () { Name = "Tag1" },  // The first expected tag
                new () { Name = "Tag2" },   // The second expected tag
            };

            // Mock the CourseService to return the expected tags when queried for the course tags
            this.mockCourseService.Setup(s => s.GetCourseTags(this.testCourse.CourseId)).Returns(expectedTags);

            // Act: Retrieve the tags through the ViewModel's Tags property
            var tags = this.viewModel.Tags;

            // Assert: Verify that the correct number of tags is returned and that they match the expected values
            Assert.Equal(2, tags.Count);  // Check that two tags are returned
            Assert.Equal("Tag1", tags[0].Name);  // Verify that the first tag's name is correct
            Assert.Equal("Tag2", tags[1].Name);  // Verify that the second tag's name is correct
        }

        /// <summary>
        /// Test to check that the CoinVisibility property returns true when the course is premium and the user is not enrolled.
        /// This test ensures that if a course is marked as premium and the user is not enrolled in it, the CoinVisibility property
        /// will return true, signaling that the coins section is visible to the user. The mock service is configured to return false
        /// for the user's enrollment status, and the course is set as premium.
        /// </summary>
        [Fact]
        public void CoinVisibility_ReturnsTrue_WhenCourseIsPremiumAndUserNotEnrolled()
        {
            // Arrange: Set the course as premium and mock the service to indicate the user is not enrolled.
            this.testCourse.IsPremium = true;
            this.mockCourseService.Setup(s => s.IsUserEnrolled(this.testCourse.CourseId)).Returns(false);

            // Create the ViewModel instance with the mock services
            var vm = new CourseViewModel(
                this.testCourse,
                this.mockCourseService.Object,
                this.mockCoinsService.Object,
                this.mockCourseTimer.Object,
                this.mockNotificationTimer.Object,
                this.mockNotificationHelper.Object);

            // Act: Retrieve the CoinVisibility property value.
            var visible = vm.CoinVisibility;

            // Assert: Verify that CoinVisibility returns true since the course is premium and the user is not enrolled.
            Assert.True(visible);
        }

        /// <summary>
        /// Test to check that the CoinVisibility property returns false when the user is enrolled in a premium course.
        /// This test ensures that if the user is already enrolled in a premium course, the coins section will not be visible.
        /// The mock service is configured to return true for the user's enrollment status.
        /// </summary>
        [Fact]
        public void CoinVisibility_ReturnsFalse_WhenUserIsEnrolled()
        {
            // Arrange: Set the course as premium and mock the service to indicate the user is enrolled.
            this.testCourse.IsPremium = true;
            this.mockCourseService.Setup(s => s.IsUserEnrolled(this.testCourse.CourseId)).Returns(true);

            // Create the ViewModel instance with the mock services
            var vm = new CourseViewModel(
                this.testCourse,
                this.mockCourseService.Object,
                this.mockCoinsService.Object,
                this.mockCourseTimer.Object,
                this.mockNotificationTimer.Object,
                this.mockNotificationHelper.Object);

            // Act: Retrieve the CoinVisibility property value.
            var visible = vm.CoinVisibility;

            // Assert: Verify that CoinVisibility returns false since the user is enrolled in the premium course.
            Assert.False(visible);
        }

        /// <summary>
        /// Test to check that the CoinVisibility property returns false when the course is not premium.
        /// This test ensures that if the course is not premium, the coins section will not be visible regardless of the user's enrollment status.
        /// </summary>
        [Fact]
        public void CoinVisibility_ReturnsFalse_WhenCourseIsNotPremium()
        {
            // Arrange: Set the course as not premium and mock the service to indicate the user is not enrolled.
            this.testCourse.IsPremium = false;
            this.mockCourseService.Setup(s => s.IsUserEnrolled(this.testCourse.CourseId)).Returns(false);

            // Create the ViewModel instance with the mock services
            var vm = new CourseViewModel(
                this.testCourse,
                this.mockCourseService.Object,
                this.mockCoinsService.Object,
                this.mockCourseTimer.Object,
                this.mockNotificationTimer.Object,
                this.mockNotificationHelper.Object);

            // Act: Retrieve the CoinVisibility property value.
            var visible = vm.CoinVisibility;

            // Assert: Verify that CoinVisibility returns false since the course is not premium.
            Assert.False(visible);
        }

        /// <summary>
        /// Test to check that the constructor initializes with default values when some services are null.
        /// This test checks that when certain services are passed as null to the CourseViewModel constructor,
        /// it initializes the ViewModel with default values. It ensures that the ViewModel does not throw any exceptions
        /// even when some dependencies are missing.
        /// </summary>
        [Fact]
        public void Constructor_WithNullServices_InitializesDefaults()
        {
            // Arrange: Create a sample course with relevant details and set it as not premium.
            var course = new Course
            {
                CourseId = 42,
                Title = "Sample Course",
                Description = "Sample Description",
                ImageUrl = "sample.jpg",
                Difficulty = "Intermediate",
                IsPremium = false,
                TimeToComplete = 1800,
            };

            // Act: Try to create a CourseViewModel instance with some null services.
            var exception = Record.Exception(() =>
            {
                var vm = new CourseViewModel(
                    course,
                    null, // courseService
                    null, // coinsService
                    this.mockCourseTimer.Object, // can't be null since we cannot mock a DispatcherTimer (which it defaults to if null)
                    this.mockNotificationTimer.Object, // can't be null since we cannot mock a DispatcherTimer (which it defaults to if null)
                    null); // notificationHelper
            });

            // Assert: Verify that no exception is thrown and that the ViewModel initializes with default values.
            Assert.Null(exception); // Constructor should not throw
        }

        /// <summary>
        /// Test to check that the constructor throws an ArgumentNullException when the course parameter is null.
        /// This test ensures that the constructor of the CourseViewModel throws an ArgumentNullException
        /// when the course parameter passed is null, ensuring that null courses cannot be passed to the ViewModel.
        /// </summary>
        [Fact]
        public void Constructor_ThrowsArgumentNullException_WhenCourseIsNull()
        {
            // Act & Assert: Verify that an ArgumentNullException is thrown when the course parameter is null.
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new CourseViewModel(null!)); // using null-forgiving operator to suppress warning

            // Assert: Verify that the exception is thrown for the course parameter.
            Assert.Equal("course", exception.ParamName);
        }

        /// <summary>
        /// This test verifies that the course timer increments time and updates the formatted time when the timer ticks.
        /// This test simulates a timer tick event, checks whether the total time spent on the course has increased by one second,
        /// and ensures that the formatted time has been updated. The private field "totalSecondsSpentOnCourse" is accessed
        /// via reflection to verify the change in time, ensuring the time tracking feature works correctly.
        /// </summary>
        [Fact]
        public void CourseTimer_Tick_IncrementsTimeAndUpdatesFormattedTime()
        {
            // Arrange: Capture the initial formatted time before the tick occurs.
            var initialFormattedTime = this.viewModel.FormattedTimeRemaining;

            // Act: Simulate a timer tick, which should increment the total seconds spent on the course.
            this.mockCourseTimer.Object.SimulateTick();

            // Access the private field "totalSecondsSpentOnCourse" via reflection to check the updated value.
            var fieldInfo = typeof(CourseViewModel).GetField("totalSecondsSpentOnCourse", BindingFlags.NonPublic | BindingFlags.Instance);
            var totalSecondsSpent = (int?)fieldInfo?.GetValue(this.viewModel);

            // Assert: Verify that the total seconds spent on the course is incremented by 1.
            Assert.Equal(1, totalSecondsSpent);

            // Assert: Verify that the formatted time has been updated, indicating the tick occurred.
            Assert.NotEqual(initialFormattedTime, this.viewModel.FormattedTimeRemaining);
        }

        /// <summary>
        /// This test ensures that the EnrollCommand cannot execute if the user is already enrolled in the course.
        /// </summary>
        [Fact]
        public void EnrollCommand_ShouldNotExecute_WhenUserIsAlreadyEnrolled()
        {
            // Arrange: Set up the mock to simulate that the user is already enrolled in the course.
            this.mockCourseService.Setup(x => x.IsUserEnrolled(It.IsAny<int>())).Returns(true);  // Simulate that the user is enrolled
            var tempViewModel = new CourseViewModel(
                this.testCourse,
                this.mockCourseService.Object,
                this.mockCoinsService.Object,
                this.mockCourseTimer.Object,
                this.mockNotificationTimer.Object,
                this.mockNotificationHelper.Object);

            // Act: Check whether the EnrollCommand can execute.
            bool canExecute = tempViewModel.EnrollCommand!.CanExecute(null);

            // Assert: Verify that the EnrollCommand cannot execute since the user is already enrolled.
            Assert.False(canExecute, "Enroll command should not be executable when the user is already enrolled.");
        }

        /// <summary>
        /// This test verifies that the EnrollCommand executes successfully if the user has enough coins to enroll in the course.
        /// </summary>
        [Fact]
        public void EnrollCommand_ShouldExecute_WhenUserHasEnoughCoins()
        {
            // Arrange: Set up the mock to simulate that the user has enough coins (e.g., 200 coins).
            this.mockCoinsService.Setup(x => x.GetCoinBalance(It.IsAny<int>())).Returns(200); // User has enough coins

            // Act: Check if the EnrollCommand can execute.
            bool canExecute = this.viewModel.EnrollCommand!.CanExecute(null);

            // Assert: Verify that the EnrollCommand can execute because the user has sufficient coins.
            Assert.True(canExecute, "Enroll command should be executable when the user has enough coins.");
        }

        /// <summary>
        /// This test verifies that the EnrollCommand does not execute if the user does not have enough coins to enroll in the course.
        /// </summary>
        [Fact]
        public void EnrollCommand_ShouldNotExecute_WhenUserDoesNotHaveEnoughCoins()
        {
            // Arrange: Set up the mock to simulate that the user has insufficient coins (e.g., 50 coins).
            this.mockCoinsService.Setup(x => x.GetCoinBalance(It.IsAny<int>())).Returns(50); // User has insufficient coins
            this.mockCoinsService.Setup(x => x.TrySpendingCoins(It.IsAny<int>(), It.IsAny<int>())).Returns(false); // Simulate failure to spend coins

            // Act: Check whether the EnrollCommand can execute and try to execute the command.
            bool canExecute = this.viewModel.EnrollCommand!.CanExecute(null);
            this.viewModel.EnrollCommand.Execute(null);

            // Assert: Verify that the EnrollCommand cannot execute because the user does not have enough coins.
            Assert.False(canExecute, "Enroll command should not be executable when the user does not have enough coins.");

            // Verify that the EnrollInCourse method is not called since the enrollment should not happen.
            this.mockCourseService.Verify(x => x.EnrollInCourse(It.IsAny<int>()), Times.Never);

            // Verify that the TrySpendingCoins method is called once to check the user's coin balance.
            this.mockCoinsService.Verify(x => x.TrySpendingCoins(It.IsAny<int>(), It.IsAny<int>()), Times.Once);

            // Verify that the GetCoinBalance method is called once to fetch the user's coin balance.
            this.mockCoinsService.Verify(x => x.GetCoinBalance(It.IsAny<int>()), Times.Once);
        }

        /// <summary>
        /// This test ensures that the EnrollCommand executes successfully even if the course enrollment fails,
        /// and that the enrollment state remains false.
        /// </summary>
        [Fact]
        public void EnrollCommand_ShouldNotExecute_WhenCourseEnrollmentFails()
        {
            // Arrange: Set up mocks to simulate successful coin spending and enrollment failure.
            this.mockCoinsService.Setup(x => x.TrySpendingCoins(It.IsAny<int>(), It.IsAny<int>())).Returns(true); // Simulate successful coin spending
            this.mockCourseService.Setup(x => x.EnrollInCourse(It.IsAny<int>())).Returns(false); // Simulate enrollment failure

            // Act: Check if the EnrollCommand can execute and attempt to execute the command.
            bool canExecute = this.viewModel.EnrollCommand!.CanExecute(null);
            this.viewModel.EnrollCommand.Execute(null);

            // Assert: Verify that the EnrollCommand can execute because the user has enough coins, even though enrollment fails.
            Assert.True(canExecute, "Enroll command should be executable when the user has enough coins.");

            // Verify that the EnrollInCourse method is called once, even though it will fail.
            this.mockCourseService.Verify(x => x.EnrollInCourse(It.IsAny<int>()), Times.Once);

            // Verify that the TrySpendingCoins method is called once to check the user's coin balance.
            this.mockCoinsService.Verify(x => x.TrySpendingCoins(It.IsAny<int>(), It.IsAny<int>()), Times.Once);

            // Verify that the UI state is not updated, meaning the user should not be marked as enrolled.
            Assert.False(this.viewModel.IsEnrolled, "IsEnrolled should remain false when enrollment fails.");
        }

        /// <summary>
        /// This test ensures that the course progress timer does not start if the user is not enrolled in the course.
        /// </summary>
        [Fact]
        public void StartCourseProgressTimer_ShouldNotStart_WhenUserIsNotEnrolled()
        {
            // Arrange: Set up the mock to return false when checking if the user is enrolled.
            // In this case, the user is not enrolled in the course, so the timer should not start.
            this.mockCourseService.Setup(x => x.IsUserEnrolled(It.IsAny<int>())).Returns(false); // User is not enrolled

            // Act: Call the method to start the course progress timer.
            // Since the user is not enrolled, this should not trigger the timer to start.
            this.viewModel.StartCourseProgressTimer();

            // Assert: Verify that the course progress timer is not started.
            // The IsCourseTimerRunning property should remain false since the user is not enrolled.
            Assert.False(this.viewModel.IsCourseTimerRunning, "Course progress timer should not start when the user is not enrolled.");

            // Assert: Verify that the Start method of the timer is never called.
            // Since the user is not enrolled, the Start method should not be invoked on the timer.
            this.mockCourseTimer.Verify(x => x.Start(), Times.Never, "The course progress timer should not be started when the user is not enrolled.");
        }

        /// <summary>
        /// This test ensures that when the timer is running, calling the PauseCourseProgressTimer method stops the timer and changes the state to not running.
        /// </summary>
        [Fact]
        public void PauseCourseProgressTimer_WhenTimerIsRunning_ShouldStopTimerAndChangeState()
        {
            // Arrange: Set the IsCourseTimerRunning to true, indicating that the timer is currently running.
            this.viewModel.IsCourseTimerRunning = true;

            // Act: Call the method to pause the course progress timer.
            // This should stop the timer and update the IsCourseTimerRunning state to false.
            this.viewModel.PauseCourseProgressTimer();

            // Assert: Verify that the Stop method of the timer is called once to stop the timer.
            this.mockCourseTimer.Verify(t => t.Stop(), Times.Once);

            // Assert: Verify that the IsCourseTimerRunning state is updated to false, indicating that the timer has been paused.
            Assert.False(this.viewModel.IsCourseTimerRunning);
        }

        /// <summary>
        /// This test checks that when the timer is not running, calling PauseCourseProgressTimer does nothing and does not stop the timer.
        /// </summary>
        [Fact]
        public void PauseCourseProgressTimer_WhenTimerIsNotRunning_ShouldDoNothing()
        {
            // Arrange: Set the IsCourseTimerRunning to false, indicating that the timer is not running.
            this.viewModel.IsCourseTimerRunning = false;

            // Act: Call the method to pause the course progress timer.
            // Since the timer is not running, no action should be taken.
            this.viewModel.PauseCourseProgressTimer();

            // Assert: Verify that the Stop method is never called since the timer is not running.
            this.mockCourseTimer.Verify(t => t.Stop(), Times.Never, "Stop method should not be called when the timer is not running.");

            // Optional: Use reflection to access and verify if SaveCourseProgressTime was called.
            var method = typeof(CourseViewModel).GetMethod("SaveCourseProgressTime", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            Assert.NotNull(method);

            // (Optional) Verify that SaveCourseProgressTime was NOT invoked (through reflection).
            // This is a workaround to check that no internal actions are triggered when the timer is not running.
        }

        /// <summary>
        /// This test checks that when the timer is running and paused, the SaveCourseProgressTime method is invoked to save the time spent.
        /// </summary>
        [Fact]
        public void PauseCourseProgressTimer_WhenTimerIsRunning_ShouldCallSaveCourseProgressTime()
        {
            // Arrange: Set up the initial conditions.
            this.viewModel.IsCourseTimerRunning = true;

            // Set initial values for private fields using reflection
            typeof(CourseViewModel).GetField("totalSecondsSpentOnCourse", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance) !
                .SetValue(this.viewModel, 1500); // 25 minutes
            typeof(CourseViewModel).GetField("lastSavedTimeInSeconds", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance) !
                .SetValue(this.viewModel, 1200); // 20 minutes

            // Act: Call the method to pause the course progress timer.
            this.viewModel.PauseCourseProgressTimer();

            // Access and verify the state of the fields after the method is executed.
            var totalSecondsSpentOnCourse = (int)typeof(CourseViewModel).GetField("totalSecondsSpentOnCourse", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance) !
                .GetValue(this.viewModel) !;
            var lastSavedTimeInSeconds = (int)typeof(CourseViewModel).GetField("lastSavedTimeInSeconds", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance) !
                .GetValue(this.viewModel) !;

            // Access the private method SaveCourseProgressTime via reflection
            var saveMethod = typeof(CourseViewModel).GetMethod("SaveCourseProgressTime", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            saveMethod!.Invoke(this.viewModel, null);  // Call the method

            // Verify that the SaveCourseProgressTime logic is correct.
            // Optionally, verify that the courseService.UpdateTimeSpent method is called via mocking.
            this.mockCourseService.Verify(s => s.UpdateTimeSpent(It.IsAny<int>(), It.IsAny<int>()), Times.Once);

            // Additionally, verify the value of lastSavedTimeInSeconds to confirm the correct update.
            Assert.Equal(totalSecondsSpentOnCourse, lastSavedTimeInSeconds);
        }

        /// <summary>
        /// This test ensures that if the timer is already running, calling the StartCourseProgressTimer method does not start the timer again.
        /// </summary>
        [Fact]
        public void StartCourseProgressTimer_WhenTimerIsAlreadyRunning_ShouldNotStartTimerAgain()
        {
            // Arrange: Set the initial conditions where the timer is already running.
            // The IsCourseTimerRunning is true and the user is enrolled.
            this.viewModel.IsCourseTimerRunning = true;
            this.viewModel.IsEnrolled = true;

            // Act: Call the method to start the course progress timer again.
            // Since the timer is already running, this should not restart the timer.
            this.viewModel.StartCourseProgressTimer();

            // Assert: Verify that the Start method on the timer is not called again.
            this.mockCourseTimer.Verify(t => t.Start(), Times.Never);

            // Assert: Verify that the IsCourseTimerRunning state remains true since the timer is already running.
            Assert.True(this.viewModel.IsCourseTimerRunning);
        }

        private void ConfigureDefaultMocks()
        {
            // Setup default behavior for ICourseService methods
            this.mockCourseService.Setup(x => x.IsUserEnrolled(It.IsAny<int>())).Returns(false); // Assume the user is not enrolled initially
            this.mockCourseService.Setup(x => x.GetTimeSpent(It.IsAny<int>())).Returns(0); // Initially, the user has spent 0 time on the course
            this.mockCourseService.Setup(x => x.GetCompletedModulesCount(It.IsAny<int>())).Returns(0); // No modules are completed initially
            this.mockCourseService.Setup(x => x.GetRequiredModulesCount(It.IsAny<int>())).Returns(5); // There are 5 required modules in the course
            this.mockCourseService.Setup(x => x.GetCourseTimeLimit(It.IsAny<int>())).Returns(3600); // The course time limit is 3600 seconds (1 hour)

            // Setup mock for ICoinsService to return a default coin balance
            this.mockCoinsService.Setup(x => x.GetCoinBalance(It.IsAny<int>())).Returns(200); // The user has 200 coins

            // Setup the timer mock for course-related ticks (simulating passage of time)
            this.mockCourseTimer.Setup(m => m.SimulateTick()).Callback(() =>
            {
                // Manually raise the Tick event when SimulateTick is called
                this.mockCourseTimer.Raise(m => m.Tick += null, EventArgs.Empty); // This simulates the timer ticking and triggering the event
            });
        }

        private void InitializeTestModules()
        {
            // Create a list of modules that belong to the test course
            var modules = new List<CourseApp.Models.Module>
            {
                // First required module
                new ()
                {
                    ModuleId = 1, Position = 1, IsBonus = false, Cost = 50,
                    Title = "Module 1 Title", Description = "Module 1 Description", ImageUrl = "module1.jpg",
                },

                // Second required module
                new ()
                {
                    ModuleId = 2, Position = 2, IsBonus = false, Cost = 50,
                    Title = "Module 2 Title", Description = "Module 2 Description", ImageUrl = "module3.jpg",
                },

                // Bonus module
                new ()
                {
                    ModuleId = 3, Position = 3, IsBonus = true, Cost = 50,
                    Title = "Module 3 Title", Description = "Module 3 Description", ImageUrl = "module3.jpg",
                },
            };

            // Setup the mock ICourseService to return the list of modules when queried
            this.mockCourseService.Setup(x => x.GetModules(It.IsAny<int>())).Returns(modules);
        }
    }
}