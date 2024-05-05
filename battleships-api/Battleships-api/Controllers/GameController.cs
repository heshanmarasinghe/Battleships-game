using Microsoft.AspNetCore.Mvc;
using Battleships_api.Services;
using Battleships_api.Models;

namespace Battleships_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : Controller
    {
        private readonly GameService _gameService;

        public GameController(GameService gameService)
        {
            _gameService = gameService;
        }

        // GET: api/game
        [HttpGet]
        public ActionResult GetShips()
        {
            ShotResult result = _gameService.GetShips();
            return Ok(result);
        }

        // POST: api/game
        [HttpPost]
        public ActionResult FireShot([FromBody] ShotCoordinates coordinates)
        {
            ShotResult result = _gameService.FireShot(coordinates.Row, coordinates.Col);
            return Ok(result);
        }

        // POST: api/game/reset
        [HttpPost("reset")]
        public ActionResult ResetGame()
        {
            _gameService.ResetShips();
            ShotResult result = _gameService.GetShips();
            return Ok(result);
        }
    }
}

