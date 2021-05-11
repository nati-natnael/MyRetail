using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using FluentAssertions;
using AutoFixture;
using System;
using Moq;

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
            Func<Task> act = async () => await _productManager.GetProductAsync(productId);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage(exceptionMsg);
        }

        [TestMethod]
        public async Task UpdateProductPrice__NonExistentProduct()
        {
            // Arrange
            var productId = _fixture.Create<long>();
            var priceValue = _fixture.Create<double>();

            _productRepositoryMock.Setup(p => p.UpdateProductPriceAsync(
                It.IsAny<long>(),
                It.IsAny<double>()
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
            var productId = _fixture.Create<long>();
            var priceValue = _fixture.Create<double>();

            _productRepositoryMock.Setup(p => p.UpdateProductPriceAsync(
                It.IsAny<long>(),
                It.IsAny<double>()
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
            var productId = _fixture.Create<long>();
            var priceValue = _fixture.Create<double>();

            string exceptionMsg = "This is test exception";

            _productRepositoryMock.Setup(p => p.UpdateProductPriceAsync(
                It.IsAny<long>(),
                It.IsAny<double>()
            )).ThrowsAsync(new Exception(exceptionMsg));

            // Act
            Func<Task> act = async () => await _productManager.UpdateProductPriceAsync(productId, priceValue);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage(exceptionMsg);
        }
    }
}
