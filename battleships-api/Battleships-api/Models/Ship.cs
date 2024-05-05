using static Battleships_api.Common.GameOrientation;

namespace Battleships_api.Models
{
    public class Ship
    {
        public string Type { get; set; }
        public int Size { get; set; }
        public int Hits { get; set; }

        // Starting row of the ship on the grid
        public int Row { get; set; }

        // Starting column of the ship on the grid
        public int Col { get; set; }

        public Orientation Orientation { get; set; }
    }
}
