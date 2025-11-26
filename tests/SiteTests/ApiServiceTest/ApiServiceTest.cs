using System.Net;
using System.Text;
using System.Text.Json;
using BlazorTestClean.Models;
using BlazorTestClean.Services;
using Xunit;

namespace SiteTests
{
    public class ApiServiceTests
    {
        [Fact]
        public async Task GetFleet_ReturnsPort_WhenResponseIsValidJson()
        {
            // Arrange
            var expected = new Port
            {
                AnchorageSize = new AnchorageSize { Width = 10, Height = 20 },
                Fleets = new List<Fleet>
                {
                    new Fleet
                    {
                        ShipDesignation = "Destroyer",
                        ShipCount = 2,
                        SingleShipDimensions = new SingleShipDimensions { Width = 1, Height = 2 }
                    }
                }
            };

            var json = JsonSerializer.Serialize(expected);
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };

            using var http = new HttpClient(new FakeHttpMessageHandler(response)) { BaseAddress = new Uri("http://localhost/") };
            var service = new ApiService(http);

            // Act
            var result = await service.GetFleet();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expected.AnchorageSize.Width, result.AnchorageSize.Width);
            Assert.Equal(expected.AnchorageSize.Height, result.AnchorageSize.Height);
            Assert.Single(result.Fleets);
            Assert.Equal("Destroyer", result.Fleets[0].ShipDesignation);
            Assert.Equal(2, result.Fleets[0].ShipCount);
            Assert.Equal(1, result.Fleets[0].SingleShipDimensions.Width);
            Assert.Equal(2, result.Fleets[0].SingleShipDimensions.Height);
        }

        [Fact]
        public async Task GetFleet_ReturnsNewPort_WhenResponseIsJsonNull()
        {
            // Arrange
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("null", Encoding.UTF8, "application/json")
            };

            using var http = new HttpClient(new FakeHttpMessageHandler(response)) { BaseAddress = new Uri("http://localhost/") };
            var service = new ApiService(http);

            // Act
            var result = await service.GetFleet();

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.AnchorageSize);
            Assert.Empty(result.Fleets);
        }

        private sealed class FakeHttpMessageHandler : HttpMessageHandler
        {
            private readonly HttpResponseMessage _response;

            public FakeHttpMessageHandler(HttpResponseMessage response)
            {
                _response = response ?? throw new ArgumentNullException(nameof(response));
            }

            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                // Mirror request headers if needed or validate request here for stronger tests.
                return Task.FromResult(_response);
            }
        }
    }
}