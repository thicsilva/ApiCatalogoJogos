using ApiCatalogoJogo.Exceptions;
using ApiCatalogoJogo.InputModel;
using ApiCatalogoJogo.Services.Interfaces;
using ApiCatalogoJogo.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace ApiCatalogoJogo.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly IGameService _gameService;

        public GamesController(IGameService gameService)
        {
            _gameService = gameService;
        }

        /// <summary>
        /// Get all games with pagination
        /// </summary>
        /// <remarks>
        /// Is not possible get games without pagination.
        /// </remarks>
        /// <param name="page">Indicate which page is being consulted. Minimum 1.</param>
        /// <param name="quantity">Indicates the number of records per page. Minimum 1 and Maximum 50.</param>
        /// <response code="200">Return a list of games.</response>
        /// <response code="204">If there are no games registered.</response>   
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GameViewModel>>> GetGames([FromQuery, Range(1, int.MaxValue)]int page=1, [FromQuery, Range(1,50)]int quantity=5)
        {
            var games = await _gameService.GetList(page, quantity);
            if (games.Count == 0)
                return NoContent();
            return Ok(games);
        }

        /// <summary>
        /// Get a game by your Id
        /// </summary>
        /// <param name="gameId">Id from game searched.</param>
        /// <response code="200">Return a game.</response>
        /// <response code="204">If there are no game registered with Id.</response>   
        [HttpGet("{gameId:guid}")]
        public async Task<ActionResult<GameViewModel>> GetGame([FromRoute]Guid gameId)
        {
            var game = await _gameService.Get(gameId);
            if (game == null)
                return NoContent();
            return Ok(game);
        }

        /// <summary>
        /// Insert a new game into catalog
        /// </summary>
        /// <param name="gameInputModel">Game data to be included.</param>
        /// <response code="201">If the game has included.</response>
        /// <response code="422">If there are registered game with same name and producer.</response>   
        [HttpPost]
        public async Task<ActionResult<GameViewModel>> CreateGame([FromBody]GameInputModel gameInputModel)
        {
            try
            {
                var game = await _gameService.Create(gameInputModel);
                return CreatedAtAction(nameof(GetGame), new { gameid=game.Id }, game);
            } catch (GameAlreadyRegisteredException)
            {
                return UnprocessableEntity("This game already exist in database.");
            }
        }

        /// <summary>
        /// Update a game into catalog
        /// </summary>
        /// <param name="gameId">Game Id to be updated.</param>
        /// <param name="gameInputModel">Game data to be updated.</param>
        /// <response code="200">If the game has updated.</response>
        /// <response code="404">If there are no game registered with this Id.</response>
        [HttpPut("{gameId:guid}")]
        public async Task<IActionResult> UpdateGame([FromRoute]Guid gameId, [FromBody] GameInputModel gameInputModel)
        {
            try
            {
                await _gameService.Update(gameId, gameInputModel);
                return Ok();
            } catch (GameNotRegisteredException)
            {
                return NotFound("This game not registered in database");
            }
        }

        /// <summary>
        /// Update price of a game
        /// </summary>
        /// <param name="gameId">Game Id to be updated.</param>
        /// <param name="price">Game price to be updated.</param>
        /// <response code="200">If the game has updated.</response>
        /// <response code="404">If there are no game registered with this Id.</response>
        [HttpPatch("{gameId:guid}/price/{price:double}")]
        public async Task<IActionResult> UpdateGame([FromRoute]Guid gameId, [FromRoute]double price)
        {
            try
            {
                await _gameService.Update(gameId, price);
                return Ok();
            }
            catch (GameNotRegisteredException)
            {
                return NotFound("This game not registered in database");
            }
        }

        /// <summary>
        /// Remove a game from catalog.
        /// </summary>
        /// <param name="gameId">Game Id to be deleted.</param>
        /// <response code="200">If the game has deleted.</response>
        /// <response code="404">If there are no game registered with this Id.</response>
        [HttpDelete("{gameId:guid}")]
        public async Task<IActionResult> DeleteGame([FromRoute]Guid gameId)
        {
            try
            {
                await _gameService.Delete(gameId);
                return Ok();
            } catch (GameNotRegisteredException)
            {
                return NotFound("This game not registered in database");
            }
        }
    }
}
