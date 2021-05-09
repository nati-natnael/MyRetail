using AutoFixture;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading.Tasks;

namespace MyRetail.UnitTest.Managers
{
    [TestClass]
    public class ProductManagerTests
    {
        private Mock<IProductRepository> _productRepositoryMock;
        private IProductManager _productManager;
        private IFixture _fixture;

        [TestInitialize]
        public void Setup()
        {
            _fixture = new Fixture();

            _productRepositoryMock = new Mock<IProductRepository>();

            _productManager = new ProductManager(_productRepositoryMock.Object);
        }

        [TestMethod]
        public async Task GetProduct_NonExistentProductName()
        {
            // Arrange
            long productId = _fixture.Create<long>();

            Product product = null;
            Price price = _fixture.Create<Price>();

            _productRepositoryMock.Setup(p => p.GetProductAsync(It.IsAny<long>()))
                .ReturnsAsync(product);
            _productRepositoryMock.Setup(p => p.GetProductPriceAsync(It.IsAny<long>()))
                .ReturnsAsync(price);

            // Act
            Product result = await _productManager.GetProductAsync(productId);

            // Assert
            result.Should().BeNull();

            _productRepositoryMock.Verify(p => p.GetProductPriceAsync(It.IsAny<long>()), Times.Never);
        }

        [TestMethod]
        public async Task GetProduct_ExistentProduct()
        {
            // Arrange
            long productId = _fixture.Create<long>();

            Product product = _fixture.Create<Product>();
            Price price = _fixture.Create<Price>();

            _productRepositoryMock.Setup(p => p.GetProductAsync(It.IsAny<long>()))
                .ReturnsAsync(product);
            _productRepositoryMock.Setup(p => p.GetProductPriceAsync(It.IsAny<long>()))
                .ReturnsAsync(price);

            // Act
            Product result = await _productManager.GetProductAsync(productId);

            // Assert
            result.Should().NotBeNull();
            result.Price.Should().NotBeNull();

            _productRepositoryMock.Verify(p => p.GetProductPriceAsync(
                It.Is<long>(id => id == productId)
            ), Times.Once);
        }

        [TestMethod]
        public async Task GetProduct_HandleException()
        {
            // Arrange
            long productId = _fixture.Create<long>();

            string exceptionMsg = "This is test exception";

            _productRepositoryMock.Setup(p => p.GetProductAsync(It.IsAny<long>()))
                .ThrowsAsync(new Exception(exceptionMsg));

            // Act
            try
            {
                await _productManager.GetProductAsync(productId);
                Assert.IsTrue(false);
            }
            catch (Exception ex)
            {
                ex.Message.Should().Be(exceptionMsg);
            }
        }

        [TestMethod]
        public async Task UpdateProductPrice__NonExistentProduct()
        {
            // Arrange
            long productId = _fixture.Create<long>();
            decimal priceValue = _fixture.Create<decimal>();

            _productRepositoryMock.Setup(p => p.UpdateProductPriceAsync(
                It.IsAny<long>(),
                It.IsAny<decimal>()
            )).ReturnsAsync(false);

            // Act
            bool result = await _productManager.UpdateProductPriceAsync(productId, priceValue);

            // Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public async Task UpdateProductPrice__ExistentProduct()
        {
            // Arrange
            long productId = _fixture.Create<long>();
            decimal priceValue = _fixture.Create<decimal>();

            _productRepositoryMock.Setup(p => p.UpdateProductPriceAsync(
                It.IsAny<long>(),
                It.IsAny<decimal>()
            )).ReturnsAsync(true);

            // Act
            bool result = await _productManager.UpdateProductPriceAsync(productId, priceValue);

            // Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public async Task UpdateProductPrice__HandleException()
        {
            // Arrange
            long productId = _fixture.Create<long>();
            decimal priceValue = _fixture.Create<decimal>();

            string exceptionMsg = "This is test exception";

            _productRepositoryMock.Setup(p => p.UpdateProductPriceAsync(
                It.IsAny<long>(),
                It.IsAny<decimal>()
            )).ThrowsAsync(new Exception(exceptionMsg));

            // Act
            try
            {
                await _productManager.UpdateProductPriceAsync(productId, priceValue);
                Assert.IsTrue(false);
            }
            catch (Exception ex)
            {
                ex.Message.Should().Be(exceptionMsg);
            }
        }
    }
}
