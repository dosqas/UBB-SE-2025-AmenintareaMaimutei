using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading.Tasks;
using DuoClassLibrary.Services;
using DuoClassLibrary.Services.Interfaces;

namespace Duo.Tests.Services
{
    [TestClass]
    public class CoinsServiceTests
    {
        private Mock<ICoinsServiceProxy> _mockProxy;
        private CoinsService _service;

        [TestInitialize]
        public void Setup()
        {
            _mockProxy = new Mock<ICoinsServiceProxy>();
            _service = new CoinsService(_mockProxy.Object);
        }

        [TestMethod]
        public async Task GetBalanceAsync_ReturnsCorrectBalance()
        {
            _mockProxy.Setup(p => p.GetUserCoinBalanceAsync(1)).ReturnsAsync(100);
            var result = await _service.GetCoinBalanceAsync(1);
            Assert.AreEqual(100, result);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task GetBalanceAsync_WhenProxyThrows_ReturnsZero()
        {
            _mockProxy.Setup(p => p.GetUserCoinBalanceAsync(99)).ThrowsAsync(new Exception());
            var result = await _service.GetCoinBalanceAsync(99);
        }

        [TestMethod]
        public async Task SpendAsync_WithEnoughCoins_ReturnsTrue()
        {
            _mockProxy.Setup(p => p.TrySpendingCoinsAsync(1, 10)).ReturnsAsync(true);
            var result = await _service.TrySpendingCoinsAsync(1, 10);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task SpendAsync_WithNegativeAmount_ReturnsFalse()
        {
            _mockProxy.Setup(p => p.TrySpendingCoinsAsync(1, -5)).ReturnsAsync(false);
            var result = await _service.TrySpendingCoinsAsync(1, -5);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task SpendAsync_WhenProxyThrows_ThrowsException()
        {
            _mockProxy.Setup(p => p.TrySpendingCoinsAsync(5, 10))
                      .ThrowsAsync(new Exception());

            await Assert.ThrowsExceptionAsync<Exception>(() =>
                _service.TrySpendingCoinsAsync(5, 10));
        }


        [TestMethod]
        public async Task AddAsync_CallsProxy()
        {
            await _service.AddCoinsAsync(1, 50);
            _mockProxy.Verify(p => p.AddCoinsAsync(1, 50), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task AddAsync_WhenProxyThrows_Returns()
        {
            _mockProxy.Setup(p => p.AddCoinsAsync(1, 50)).ThrowsAsync(new Exception());
            await _service.AddCoinsAsync(1, 50);
        }

        [TestMethod]
        public async Task GiveDailyBonusAsync_Success_ReturnsTrue()
        {
            _mockProxy.Setup(p => p.ApplyDailyLoginBonusAsync(2)).ReturnsAsync(true);
            var result = await _service.ApplyDailyLoginBonusAsync(2);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task GiveDailyBonusAsync_Failure_ReturnsFalse()
        {
            _mockProxy.Setup(p => p.ApplyDailyLoginBonusAsync(It.IsAny<int>())).ReturnsAsync(false);
            var result = await _service.ApplyDailyLoginBonusAsync(3);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task GiveDailyBonusAsync_WhenProxyThrows_ThrowsException()
        {
            _mockProxy.Setup(p => p.ApplyDailyLoginBonusAsync(5))
                      .ThrowsAsync(new Exception());

            await Assert.ThrowsExceptionAsync<Exception>(() =>
                _service.ApplyDailyLoginBonusAsync(5));
        }


        [TestMethod]
        public async Task AddCoinsAsync_WithZeroAmount_CallsProxy()
        {
            await _service.AddCoinsAsync(1, 0);
            _mockProxy.Verify(p => p.AddCoinsAsync(1, 0), Times.Once);
        }

        [TestMethod]
        public async Task SpendCoinsAsync_WithZeroAmount_ReturnsFalse()
        {
            _mockProxy.Setup(p => p.TrySpendingCoinsAsync(1, 0)).ReturnsAsync(false);
            var result = await _service.TrySpendingCoinsAsync(1, 0);
            Assert.IsFalse(result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public async Task GetBalanceAsync_WithNegativeUserId_ReturnsZero()
        {
            _mockProxy.Setup(p => p.GetUserCoinBalanceAsync(It.IsAny<int>())).ThrowsAsync(new ArgumentOutOfRangeException());
            var result = await _service.GetCoinBalanceAsync(-1);
            Assert.AreEqual(0, result);
        }
    }
}