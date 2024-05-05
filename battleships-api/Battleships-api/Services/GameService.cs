using Battleships_api.Models;
using static Battleships_api.Common.GameOrientation;
using static Battleships_api.Common.ShipConfig;

namespace Battleships_api.Services
{
    public class GameService
    {
        private readonly Grid _grid;
        private readonly Random _random;

        public GameService()
        {
            _grid = new Grid();
            _random = new Random();
            PlaceShips();
        }

        public ShotResult GetShips()
        {
            return new ShotResult
            {
                Ships = _grid.Ships,
                Hit = false,
                Sunk = false,
            };
        }

        public void ResetShips()
        {
            _grid.Ships.Clear();
            Array.Clear(_grid.Cells);
            PlaceShips();
        }

        private void PlaceShips()
        {
            // Place Battleship
            PlaceShip("Battleship", (int)ShipSize.Battleship);

            // Place Destroyers
            PlaceShip("Destroyer", (int)ShipSize.Destroyer);
            PlaceShip("Destroyer", (int)ShipSize.Destroyer);
        }

        private void PlaceShip(string type, int size)
        {
            Ship ship = new Ship
            {
                Type = type,
                Size = size,
                Orientation = _random.Next(2) == 0 ? Orientation.Horizontal : Orientation.Vertical
            };

            bool placed = false;
            while (!placed)
            {
                ship.Row = _random.Next(10);
                ship.Col = _random.Next(10);
                if (CanPlaceShip(ship, ship.Row, ship.Col, ship.Orientation))
                {
                    PlaceShipOnGrid(ship, ship.Row, ship.Col, ship.Orientation);
                    placed = true;
                }
            }
        }

        private bool CanPlaceShip(Ship ship, int row, int col, Orientation orientation)
        {
            if (orientation == Orientation.Horizontal && col + ship.Size > 10)
                return false;
            if (orientation == Orientation.Vertical && row + ship.Size > 10)
                return false;

            for (int i = 0; i < ship.Size; i++)
            {
                int r = orientation == Orientation.Vertical ? row + i : row;
                int c = orientation == Orientation.Horizontal ? col + i : col;

                if (_grid.Cells[r, c] != null)
                    return false;
            }
            return true;
        }

        private void PlaceShipOnGrid(Ship ship, int row, int col, Orientation orientation)
        {
            ship.Hits = 0;
            _grid.Ships.Add(ship);

            for (int i = 0; i < ship.Size; i++)
            {
                int r = orientation == Orientation.Vertical ? row + i : row;
                int c = orientation == Orientation.Horizontal ? col + i : col;
                _grid.Cells[r, c] = ship.Type[0].ToString(); // Using the first letter of ship type as a symbol on the grid
            }
        }

        public ShotResult FireShot(int row, int col)
        {
            ShotResult result = new ShotResult();

            // Check if the cell contains a ship
            if (_grid.Cells[row, col] != null)
            {
                result.Hit = true;

                // Find the ship that was hit
                Ship hitShip = null;
                foreach (var ship in _grid.Ships)
                {
                    for (int i = 0; i < ship.Size; i++)
                    {
                        int r = ship.Orientation == Orientation.Vertical ? ship.Row + i : ship.Row;
                        int c = ship.Orientation == Orientation.Horizontal ? ship.Col + i : ship.Col;
                        if (r == row && c == col)
                        {
                            hitShip = ship;
                            break;
                        }
                    }
                    if (hitShip != null)
                        break;
                }

                // Process the hit ship
                if (hitShip != null)
                {
                    hitShip.Hits++;
                    if (hitShip.Hits == hitShip.Size)
                    {
                        result.Sunk = true;
                        _grid.Ships.Remove(hitShip); // Remove the sunk ship from the list of ships
                    }
                }
            }
            else
            {
                result.Hit = false;
            }

            result.Ships = _grid.Ships;
            return result;
        }

    }
}
