using Bunit;
using BlazorTestClean.DTO;
using BlazorTestClean.Models;
using BlazorTestClean.Services.Interfaces;
using BlazorTestClean.Components.Pages;
using Microsoft.Extensions.DependencyInjection;

namespace SiteTests
{
    public class HomeTests : BunitContext
    {
        [Fact]
        public void Home_RendersDropAreaAndAvailableShips_FromApi()
        {
            // Arrange
            var port = new Port
            {
                AnchorageSize = new AnchorageSize { Width = 10, Height = 5 },
                Fleets = new List<Fleet>
                {
                    new Fleet
                    {
                        ShipCount = 3,
                        SingleShipDimensions = new SingleShipDimensions { Width = 2, Height = 1 },
                        ShipDesignation = "TestShip"
                    }
                }
            };

            Services.AddSingleton<IApiService>(new FakeApiService(port));

            // Provide a JSInterop response should the component call it (safety for any JS calls)
            JSInterop
                .Setup<DomRect>("dragDropWireUp.getRect")
                .SetResult(new DomRect
                {
                    Left = 0,
                    Top = 0,
                    Width = port.AnchorageSize.Width * 20,
                    Height = port.AnchorageSize.Height * 20
                });

            // Act
            var cut = Render<Home>();

            // Assert - drop area dimensions use CellSize = 20 => width = 10 * 20 = 200px, height = 5 * 20 = 100px
            var dropArea = cut.Find(".drop-area");
            Assert.Contains("width:200px", dropArea.GetAttribute("style"));
            Assert.Contains("height:100px", dropArea.GetAttribute("style"));

            // Assert - number of draggable items equals total ships (ShipCount * fleets)
            var draggableItems = cut.FindAll(".draggableq");
            Assert.Equal(3, draggableItems.Count);

            // Assert - the visible text for a ship contains expected dimension string (component shows Width x Height)
            Assert.Contains("2x1", cut.Markup);
        }

        private sealed class FakeApiService : IApiService
        {
            private readonly Port _port;

            public FakeApiService(Port port) => _port = port;

            public Task<Port> GetFleet() => Task.FromResult(_port);
        }
    }
}