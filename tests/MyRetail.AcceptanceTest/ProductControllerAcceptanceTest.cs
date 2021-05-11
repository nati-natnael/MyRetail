using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using System.Text.Json;
using FluentAssertions;
using System.Net.Http;
using AutoFixture;

namespace MyRetail.AcceptanceTest
{
    [TestClass]
    public class MyRatailProductAccept
    {
        private string _testUrl;
        private string _productId;
        private IFixture _fixture;
        private HttpClient _httpClient;

        [TestInitialize]
        public void Setup()
        {
            _testUrl = "https://my-retail-service.azurewebsites.net";
            _productId = "13860428";

            _fixture = new Fixture();
            _httpClient = new HttpClient();
        }

        [TestMethod]
        public async Task GetProductById()
        {
            // Arrange
            var request = $"{_testUrl}/product/{_productId}";

            // Act
            HttpResponseMessage response =  await _httpClient.GetAsync(request);

            string jsonResp = await response.Content.ReadAsStringAsync();

            Product product = JsonSerializer.Deserialize<Product>(jsonResp);

            // Assert
            product.Should().NotBeNull();
        }

        [TestMethod]
        public async Task UpdateProductPriceById()
        {
            // Arrange
            var price = _fixture.Create<double>();
            var request = $"{_testUrl}/product/{_productId}/price/{price}";

            // Act
            HttpResponseMessage response = await _httpClient.PutAsync(request, null);

            // Assert
            response.StatusCode.Should().Be(200);
        }
    }
}
