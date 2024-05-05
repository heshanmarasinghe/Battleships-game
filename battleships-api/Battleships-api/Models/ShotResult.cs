namespace Battleships_api.Models
{
    public class ShotResult
    {
        public bool Hit { get; set; }
        public bool Sunk { get; set; }
        public List<Ship> Ships { get; set; }
    }
}
