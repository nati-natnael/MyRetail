using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MyRetail.Controllers;
using FluentAssertions;
using AutoFixture;
using System;
using Moq;

namespace MyRetail.UnitTest.Controllers
{
    [TestClass]
    public class ProductControllerTests
    {
        private IFixture _fixture;
        private Mock<IProductManager> _productManagerMock;

        private ProductController _productController;

        [TestInitialize]
        public void Setup()
        {
            _fixture = new Fixture();
            _productManagerMock = new Mock<IProductManager>();
            _productController = new ProductController(_productManagerMock.Object, null);
        }

        [TestMethod]
        public async Task Get()
        {
            // Arrange
            long productId = _fixture.Create<long>();
            Product product = _fixture.Create<Product>();

            _productManagerMock.Setup(p => p.GetProductAsync(It.IsAny<long>())).ReturnsAsync(product);

            // Act
            ObjectResult result = (ObjectResult) await _productController.Get(productId);

            // Assert
            result.Value.Should().Equals(product);
        }

        [TestMethod]
        public async Task Get_Exception()
        {
            // Arrange
            long productId = _fixture.Create<long>();

            _productManagerMock.Setup(p => p.GetProductAsync(It.IsAny<long>()))
                .ThrowsAsync(new Exception("Test exception message"));

            // Act
            ObjectResult result = (ObjectResult)await _productController.Get(productId);

            // Assert
            result.StatusCode.Should().Be(404);
            result.Value.Should().Be($"Product not found: {productId}");
        }

        [TestMethod]
        public async Task Put()
        {
            // Arrange
            var productId = _fixture.Create<long>();
            var priceValue = _fixture.Create<double>();

            _productManagerMock.Setup(p => p.UpdateProductPriceAsync(
                It.IsAny<long>(),
                It.IsAny<double>()
            )).ReturnsAsync(true);

            // Act
            ObjectResult result = (ObjectResult) await _productController.Put(productId, priceValue);

            // Assert
            result.StatusCode.Should().Be(200);

            _productManagerMock.Verify(p => p.UpdateProductPriceAsync(
                It.Is<long>(id => id == productId),
                It.Is<double>(price => price == priceValue)
            ), Times.Once);
        }

        [TestMethod]
        public async Task Put_Exception()
        {
            // Arrange
            var productId = _fixture.Create<long>();
            var priceValue = _fixture.Create<double>();

            _productManagerMock.Setup(p => p.UpdateProductPriceAsync(
                It.IsAny<long>(),
                It.IsAny<double>()
            )).ThrowsAsync(new Exception("Test exception message"));

            // Act
            ObjectResult result = (ObjectResult)await _productController.Put(productId, priceValue);

            // Assert
            result.StatusCode.Should().Be(404);
            result.Value.Should().Be($"Product not found: {productId}");
        }
    }
}
