using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using System.Threading;
using System.Net.Http;
using MongoDB.Driver;
using Moq.Protected;
using MongoDB.Bson;
using AutoFixture;
using System.Net;
using System;
using Moq;

namespace MyRetail.UnitTest.Repositories
{
    [TestClass]
    public class ProductRepositoryTests
    {
        private IFixture _fixture;

        private Mock<IAsyncCursor<BsonDocument>> _docCursor;
        private Mock<IMongoCollection<BsonDocument>> _collectionMock;

        private IProductRepository _productRepository;

        [TestInitialize]
        public void Setup()
        {
            _fixture = new Fixture();

            _docCursor = new Mock<IAsyncCursor<BsonDocument>>();

            _collectionMock = new Mock<IMongoCollection<BsonDocument>>();
        }

        [TestMethod]
        public async Task GetProductAsync_ExistingProductId()
        {
            // Arrange
            var productId = _fixture.Create<long>();
            var productName = _fixture.Create<string>();

            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("{ \"product\": { \"item\": { \"product_description\": { \"title\": \"" + productName + "\" } } } }")
            };

            var httpHandlerMock = new Mock<HttpMessageHandler>();
            httpHandlerMock.Protected().Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            ).ReturnsAsync(response)
            .Verifiable();

            var httpClient = new HttpClient(httpHandlerMock.Object);

            _productRepository = new ProductRepository(httpClient, _collectionMock.Object);

            // Act
            Product result = await _productRepository.GetProductAsync(productId);

            // Assert
            result.Should().NotBeNull();
            result.Price.Should().BeNull();
            result.Id.Should().Be(productId);
            result.Name.Should().Be(productName);
        }

        [TestMethod]
        public async Task GetProductAsync_NonExistentProductId()
        {
            // Arrange
            var productId = _fixture.Create<long>();

            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NotFound
            };

            var httpHandlerMock = new Mock<HttpMessageHandler>();
            httpHandlerMock.Protected().Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            ).ReturnsAsync(response)
            .Verifiable();

            var httpClient = new HttpClient(httpHandlerMock.Object);

            _productRepository = new ProductRepository(httpClient, _collectionMock.Object);

            // Act
            Func<Task> act = async () => await _productRepository.GetProductAsync(productId);

            // Assert
            await act.Should().ThrowAsync<Exception>();
        }

        [TestMethod]
        public async Task GetProductAsync_ProductNameNotFound()
        {
            // Arrange
            var productId = _fixture.Create<long>();

            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("{ \"product\": { \"item\": { \"product_description\": {  } } } }")
            };

            var httpHandlerMock = new Mock<HttpMessageHandler>();
            httpHandlerMock.Protected().Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            ).ReturnsAsync(response)
            .Verifiable();

            var httpClient = new HttpClient(httpHandlerMock.Object);

            _productRepository = new ProductRepository(httpClient, _collectionMock.Object);

            // Act
            Product result = await _productRepository.GetProductAsync(productId);

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().BeNull();
            result.Price.Should().BeNull();
            result.Id.Should().Be(productId);
        }

        [TestMethod]
        public async Task GetProductPriceAsync_ExistingProductId()
        {
            // Arrange
            var productId = _fixture.Create<long>();
            var priceValue = _fixture.Create<double>();
            var currency = _fixture.Create<string>();

            _productRepository = new ProductRepository(null, _collectionMock.Object);

            BsonDocument doc = new BsonDocument
            {
                { "id", productId },
                { "name", "test name" },
                {
                    "current_price",
                    new BsonDocument
                    {
                        { "value", priceValue },
                        { "currency_code", currency }
                    }
                }
            };

            _docCursor.Setup(p => p.Current).Returns(new List<BsonDocument>() { doc });
            _docCursor.SetupSequence(p => p.MoveNext(It.IsAny<CancellationToken>())).Returns(true).Returns(false);
            _docCursor.SetupSequence(p => p.MoveNextAsync(It.IsAny<CancellationToken>())).ReturnsAsync(true).ReturnsAsync(false);

            _collectionMock.Setup(p => p.FindAsync(
                It.IsAny<FilterDefinition<BsonDocument>>(),
                It.IsAny<FindOptions<BsonDocument, BsonDocument>>(),
                It.IsAny<CancellationToken>()
            )).ReturnsAsync(_docCursor.Object);

            // Act
            Price result = await _productRepository.GetProductPriceAsync(productId);

            // Assert
            result.Should().NotBeNull();
            result.Currency.Should().Be(currency);
            result.Value.Should().Be((decimal) priceValue);
        }

        [TestMethod]
        public async Task GetProductPriceAsync_NoPriceValue()
        {
            // Arrange
            var productId = _fixture.Create<long>();
            var priceValue = _fixture.Create<double>();
            var currency = _fixture.Create<string>();

            _productRepository = new ProductRepository(null, _collectionMock.Object);

            BsonDocument doc = new BsonDocument
            {
                { "id", productId },
                { "name", "test name" },
                {
                    "current_price",
                    new BsonDocument()
                }
            };

            _docCursor.Setup(p => p.Current).Returns(new List<BsonDocument>() { doc });
            _docCursor.SetupSequence(p => p.MoveNext(It.IsAny<CancellationToken>())).Returns(true).Returns(false);
            _docCursor.SetupSequence(p => p.MoveNextAsync(It.IsAny<CancellationToken>())).ReturnsAsync(true).ReturnsAsync(false);

            _collectionMock.Setup(p => p.FindAsync(
                It.IsAny<FilterDefinition<BsonDocument>>(),
                It.IsAny<FindOptions<BsonDocument, BsonDocument>>(),
                It.IsAny<CancellationToken>()
            )).ReturnsAsync(_docCursor.Object);

            // Act
            Price result = await _productRepository.GetProductPriceAsync(productId);

            // Assert
            result.Should().NotBeNull();
            result.Value.Should().Be(0);
            result.Currency.Should().BeNull();
        }

        [TestMethod]
        public async Task GetProductPriceAsync_NonExistentProductId()
        {
            // Arrange
            var productId = _fixture.Create<long>();

            _productRepository = new ProductRepository(null, _collectionMock.Object);

            _docCursor.Setup(p => p.Current).Returns(new List<BsonDocument>());
            _docCursor.SetupSequence(p => p.MoveNext(It.IsAny<CancellationToken>())).Returns(true).Returns(false);
            _docCursor.SetupSequence(p => p.MoveNextAsync(It.IsAny<CancellationToken>())).ReturnsAsync(true).ReturnsAsync(false);

            _collectionMock.Setup(p => p.FindAsync(
                It.IsAny<FilterDefinition<BsonDocument>>(),
                It.IsAny<FindOptions<BsonDocument, BsonDocument>>(),
                It.IsAny<CancellationToken>()
            )).ReturnsAsync(_docCursor.Object);

            // Act
            Price result = await _productRepository.GetProductPriceAsync(productId);

            // Assert
            result.Should().BeNull();
        }

        [TestMethod]
        public async Task UpdateProductPriceAsync_ExistingProductId()
        {
            // Arrange
            var productId = _fixture.Create<long>();
            var priceValue = _fixture.Create<double>();
            var currency = _fixture.Create<string>();

            var doc = new BsonDocument
            {
                { "id", productId },
                { "name", "test name" },
                {
                    "current_price",
                    new BsonDocument
                    {
                        { "value", priceValue },
                        { "currency_code", currency }
                    }
                }
            };

            _docCursor.Setup(p => p.Current).Returns(new List<BsonDocument>() { doc });
            _docCursor.SetupSequence(p => p.MoveNext(It.IsAny<CancellationToken>())).Returns(true).Returns(false);
            _docCursor.SetupSequence(p => p.MoveNextAsync(It.IsAny<CancellationToken>())).ReturnsAsync(true).ReturnsAsync(false);

            _collectionMock.Setup(p => p.FindAsync(
                It.IsAny<FilterDefinition<BsonDocument>>(),
                It.IsAny<FindOptions<BsonDocument, BsonDocument>>(),
                It.IsAny<CancellationToken>()
            )).ReturnsAsync(_docCursor.Object);

            var updateResultMock = new Mock<UpdateResult>();

            updateResultMock.Setup(p => p.IsAcknowledged).Returns(true);

            _collectionMock.Setup(p => p.UpdateOneAsync(
                It.IsAny<FilterDefinition<BsonDocument>>(),
                It.IsAny<UpdateDefinition<BsonDocument>>(),
                It.IsAny<UpdateOptions>(),
                It.IsAny<CancellationToken>()
            )).ReturnsAsync(updateResultMock.Object);

            _productRepository = new ProductRepository(null, _collectionMock.Object);

            // Act
            bool result = await _productRepository.UpdateProductPriceAsync(productId, priceValue);

            // Assert
            result.Should().Be(true);
        }

        [TestMethod]
        public async Task UpdateProductPriceAsync_NonExistentProductId()
        {
            // Arrange
            var productId = _fixture.Create<long>();
            var priceValue = _fixture.Create<double>();

            _collectionMock.Setup(p => p.FindAsync(
                It.IsAny<FilterDefinition<BsonDocument>>(),
                It.IsAny<FindOptions<BsonDocument, BsonDocument>>(),
                It.IsAny<CancellationToken>()
            )).ReturnsAsync(_docCursor.Object);

            var updateResultMock = new Mock<UpdateResult>();

            updateResultMock.Setup(p => p.IsAcknowledged).Returns(false);

            _collectionMock.Setup(p => p.UpdateOneAsync(
                It.IsAny<FilterDefinition<BsonDocument>>(),
                It.IsAny<UpdateDefinition<BsonDocument>>(),
                It.IsAny<UpdateOptions>(),
                It.IsAny<CancellationToken>()
            )).ReturnsAsync(updateResultMock.Object);

            _productRepository = new ProductRepository(null, _collectionMock.Object);

            // Act
            Func<Task> act = async () => await _productRepository.UpdateProductPriceAsync(productId, priceValue);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>().WithMessage($"Product not found {productId}");
        }
    }
}
