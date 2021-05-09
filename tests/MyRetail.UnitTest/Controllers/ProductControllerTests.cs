using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            Product result = await _productController.Get(productId);

            // Assert
            result.Should().Equals(product);
        }

        [TestMethod]
        public async Task Get_Exception()
        {
            // Arrange
            long productId = _fixture.Create<long>();
            string exceptionMsg = "Test exception message";

            _productManagerMock.Setup(p => p.GetProductAsync(It.IsAny<long>()))
                .ThrowsAsync(new Exception(exceptionMsg));

            // Act
            try
            {
                await _productController.Get(productId);
                Assert.Fail();
            }
            catch (Exception ex)
            {
                ex.Message.Should().Be(exceptionMsg);
            }
        }

        [TestMethod]
        public async Task Put()
        {
            // Arrange
            long productId = _fixture.Create<long>();
            decimal priceValue = _fixture.Create<decimal>();

            _productManagerMock.Setup(p => p.UpdateProductPriceAsync(
                It.IsAny<long>(),
                It.IsAny<decimal>()
            )).ReturnsAsync(true);

            // Act
            await _productController.Put(productId, priceValue);

            // Assert
            _productManagerMock.Verify(p => p.UpdateProductPriceAsync(
                It.Is<long>(id => id == productId),
                It.Is<decimal>(price => price == priceValue)
            ), Times.Once);
        }

        [TestMethod]
        public async Task Put_Exception()
        {
            // Arrange
            long productId = _fixture.Create<long>();
            decimal priceValue = _fixture.Create<decimal>();
            string exceptionMsg = "Test exception message";

            _productManagerMock.Setup(p => p.UpdateProductPriceAsync(
                It.IsAny<long>(),
                It.IsAny<decimal>()
            )).ThrowsAsync(new Exception(exceptionMsg));

            // Act
            try
            {
                await _productController.Put(productId, priceValue);
                Assert.Fail();
            }
            catch (Exception ex)
            {
                ex.Message.Should().Be(exceptionMsg);
            }
        }
    }
}
