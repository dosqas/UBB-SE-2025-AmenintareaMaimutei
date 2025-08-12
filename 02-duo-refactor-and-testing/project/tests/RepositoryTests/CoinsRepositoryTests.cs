// <copyright file="CoinsRepositoryTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Tests.RepositoryTests
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using CourseApp.ModelViews;
    using CourseApp.Repository;
    using Moq;
    using Xunit;

    /// <summary>
    /// Contains unit tests for the CoinsRepository class, ensuring correct delegation
    /// of wallet-related operations to the IUserWalletModelView dependency.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class CoinsRepositoryTests
    {
        private readonly Mock<IUserWalletModelView> mockWalletModelView;
        private readonly CoinsRepository repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CoinsRepositoryTests"/> class,
        /// setting up the mocked IUserWalletModelView and CoinsRepository instance.
        /// </summary>
        public CoinsRepositoryTests()
        {
            mockWalletModelView = new Mock<IUserWalletModelView>();
            repository = new CoinsRepository(mockWalletModelView.Object);
        }

        /// <summary>
        /// Verifies that InitializeUserWalletIfNotExists calls the model view with the correct parameters.
        /// </summary>
        [Fact]
        public void InitializeUserWalletIfNotExists_WhenCalled_CallsModelViewWithCorrectParameters()
        {
            // Act
            repository.InitializeUserWalletIfNotExists(1, 100);

            // Assert
            mockWalletModelView.Verify(m => m.InitializeUserWalletIfNotExists(1, 100), Times.Once);
        }

        /// <summary>
        /// Verifies that GetUserCoinBalance returns the correct coin balance retrieved from the model view.
        /// </summary>
        [Fact]
        public void GetUserCoinBalance_WhenCalled_ReturnsCorrectBalance()
        {
            // Arrange
            mockWalletModelView.Setup(m => m.GetUserCoinBalance(2)).Returns(150);

            // Act
            var result = repository.GetUserCoinBalance(2);

            // Assert
            Assert.Equal(150, result);
        }

        /// <summary>
        /// Verifies that SetUserCoinBalance delegates the operation to the model view.
        /// </summary>
        [Fact]
        public void SetUserCoinBalance_WhenCalled_DelegatesToModelView()
        {
            // Act
            repository.SetUserCoinBalance(3, 200);

            // Assert
            mockWalletModelView.Verify(m => m.SetUserCoinBalance(3, 200), Times.Once);
        }

        /// <summary>
        /// Verifies that GetUserLastLoginTime returns the correct datetime from the model view.
        /// </summary>
        [Fact]
        public void GetUserLastLoginTime_WhenCalled_ReturnsCorrectTime()
        {
            // Arrange
            var expectedDate = new DateTime(2023, 01, 01);
            mockWalletModelView.Setup(m => m.GetUserLastLoginTime(4)).Returns(expectedDate);

            // Act
            var result = repository.GetUserLastLoginTime(4);

            // Assert
            Assert.Equal(expectedDate, result);
        }

        /// <summary>
        /// Verifies that UpdateUserLastLoginTimeToNow delegates the update operation to the model view.
        /// </summary>
        [Fact]
        public void UpdateUserLastLoginTimeToNow_WhenCalled_DelegatesToModelView()
        {
            // Act
            repository.UpdateUserLastLoginTimeToNow(5);

            // Assert
            mockWalletModelView.Verify(m => m.UpdateUserLastLoginTimeToNow(5), Times.Once);
        }

        /// <summary>
        /// Verifies that AddCoinsToUserWallet correctly delegates the add operation to the model view.
        /// </summary>
        [Fact]
        public void AddCoinsToUserWallet_WhenCalled_DelegatesToModelView()
        {
            // Act
            repository.AddCoinsToUserWallet(6, 50);

            // Assert
            mockWalletModelView.Verify(m => m.AddCoinsToUserWallet(6, 50), Times.Once);
        }

        /// <summary>
        /// Verifies that TryDeductCoinsFromUserWallet returns true when the balance is sufficient.
        /// </summary>
        [Fact]
        public void TryDeductCoinsFromUserWallet_WhenSufficientBalance_ReturnsTrue()
        {
            // Arrange
            mockWalletModelView.Setup(m => m.TryDeductCoinsFromUserWallet(7, 75)).Returns(true);

            // Act
            var result = repository.TryDeductCoinsFromUserWallet(7, 75);

            // Assert
            Assert.True(result);
        }

        /// <summary>
        /// Verifies that TryDeductCoinsFromUserWallet returns false when the balance is insufficient.
        /// </summary>
        [Fact]
        public void TryDeductCoinsFromUserWallet_WhenInsufficientBalance_ReturnsFalse()
        {
            // Arrange
            mockWalletModelView.Setup(m => m.TryDeductCoinsFromUserWallet(8, 999)).Returns(false);

            // Act
            var result = repository.TryDeductCoinsFromUserWallet(8, 999);

            // Assert
            Assert.False(result);
        }
    }
}
