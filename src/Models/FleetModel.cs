namespace BlazorTestClean.Models
{
    public class Port
    {
        public AnchorageSize AnchorageSize { get; set; } = new AnchorageSize();
        public List<Fleet> Fleets { get; set; } = new List<Fleet>();
    }

    public class AnchorageSize
    {
        public int Width { get; set; }
        public int Height { get; set; }
    }

    public class Fleet
    {
        public SingleShipDimensions SingleShipDimensions { get; set; } = new SingleShipDimensions();
        public string ShipDesignation { get; set; } = default!;
        public int ShipCount { get; set; }
    }

    public class SingleShipDimensions
    {
        public int Width { get; set; }
        public int Height { get; set; }
    }
}
