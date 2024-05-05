namespace Battleships_api.Models
{
    public class Grid
    {
        public List<Ship> Ships { get; set; }
        public string[,] Cells { get; set; }

        public Grid()
        {
            Ships = new List<Ship>();

            // Initialize the Cells array with dimensions 10x10
            Cells = new string[10, 10];
        }
    }
}
