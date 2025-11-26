namespace BlazorTestClean.DTO
{
    public class PlacedShip : Ship
    {
        public double Left { get; set; }
        public double Top { get; set; }
        public string Leftpx => $"{Left}px";
        public string Toppx => $"{Top}px";
    }
}
