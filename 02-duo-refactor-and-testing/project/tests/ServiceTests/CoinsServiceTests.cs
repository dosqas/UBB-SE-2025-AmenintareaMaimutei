// <copyright file="CoinsServiceTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Tests.ServiceTests
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using CourseApp.Repository;
    using CourseApp.Services;
    using Moq;
    using Xunit;

    /// <summary>
    /// Contains unit tests for the <see cref="CoinsService"/> class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class CoinsServiceTests
    {
        private readonly FakeCoinsRepository fakeRepo;
        private readonly CoinsService coinsService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CoinsServiceTests"/> class.
        /// Sets up the mock repository and the <see cref="CoinsService"/> instance for testing.
        /// </summary>
        public CoinsServiceTests()
        {
            fakeRepo = new FakeCoinsRepository();
            coinsService = new CoinsService(fakeRepo);
        }

        /// <summary>
        /// Tests that <see cref="CoinsService.GetCoinBalance"/> returns the correct balance.
        /// </summary>
        [Fact]
        public void GetCoinBalance_ReturnsCorrectBalance()
        {
            int balance = coinsService.GetCoinBalance(0);
            Assert.Equal(100, balance);
        }

        /// <summary>
        /// Tests that <see cref="CoinsService.TrySpendingCoins"/> deducts coins successfully.
        /// </summary>
        [Fact]
        public void TrySpendingCoins_DeductsCoinsSuccessfully()
        {
            bool result = coinsService.TrySpendingCoins(0, 50);
            Assert.True(result);
            Assert.Equal(50, coinsService.GetCoinBalance(0));
        }

        /// <summary>
        /// Tests that <see cref="CoinsService.TrySpendingCoins"/> fails when there are insufficient funds.
        /// </summary>
        [Fact]
        public void TrySpendingCoins_FailsWhenInsufficientFunds()
        {
            bool result = coinsService.TrySpendingCoins(0, 150);
            Assert.False(result);
            Assert.Equal(100, coinsService.GetCoinBalance(0));
        }

        /// <summary>
        /// Tests that <see cref="CoinsService.AddCoins"/> increases the coin balance correctly.
        /// </summary>
        [Fact]
        public void AddCoins_IncreasesBalanceCorrectly()
        {
            coinsService.AddCoins(0, 50);
            Assert.Equal(150, coinsService.GetCoinBalance(0));
        }

        /// <summary>
        /// Tests that <see cref="CoinsService.ApplyDailyLoginBonus"/> adds coins when the login is on a new day.
        /// </summary>
        [Fact]
        public void ApplyDailyLoginBonus_AddsCoinsWhenLoginIsNewDay()
        {
            bool result = coinsService.ApplyDailyLoginBonus(0);
            Assert.True(result);
            Assert.Equal(200, coinsService.GetCoinBalance(0));
        }

        /// <summary>
        /// Tests that <see cref="CoinsService.ApplyDailyLoginBonus"/> does not add coins when the login is on the same day.
        /// </summary>
        [Fact]
        public void ApplyDailyLoginBonus_DoesNotAddCoinsWhenSameDay()
        {
            coinsService.ApplyDailyLoginBonus(0);

            bool result = coinsService.ApplyDailyLoginBonus(0);

            Assert.False(result);
            Assert.Equal(200, coinsService.GetCoinBalance(0));
        }

        /// <summary>
        /// Tests that the constructor of <see cref="CoinsService"/> uses the default repository when none is provided.
        /// </summary>
        [Fact]
        public void Constructor_UsesDefaultRepositoryWhenNoneProvided()
        {
            var service = new CoinsService();
            Assert.NotNull(service);
        }

        /// <summary>
        /// Tests that <see cref="CoinsService.GetCoinBalance"/> returns zero for a new user.
        /// </summary>
        [Fact]
        public void GetCoinBalance_ReturnsZeroForNewUser()
        {
            fakeRepo.SetUserCoinBalance(1, 0);
            int balance = coinsService.GetCoinBalance(1);
            Assert.Equal(0, balance);
        }
    }
}
