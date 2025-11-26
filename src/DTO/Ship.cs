using BlazorTestClean.Models;
using Microsoft.AspNetCore.Components;

namespace BlazorTestClean.DTO
{
    public class Ship : SingleShipDimensions
    {
        public int Row { get; set; }
        public int Col { get; set; }
        public double Xpx { get; set; }
        public double Ypx { get; set; }
        public ElementReference elRef { get; set; }
        public bool isPlaced { get; set; }
    }
}
