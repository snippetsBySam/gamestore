using GameStore.Api.Data;
using GameStore.Api.Dtos;
using GameStore.Api.Entities;
using GameStore.Api.Mapping;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GamesController : ControllerBase
{
    private readonly GameStoreContext _gameStoreContext;
    public GamesController(GameStoreContext gameStoreContext)
    {
        _gameStoreContext = gameStoreContext;
    }

    //GET /games
    [HttpGet]
    public async Task<IActionResult> GetAllGames()
    {
        var games = await _gameStoreContext.Games
            .Include(game => game.Genre)
            .Select(game => game.ToGameSummaryDto())
            .AsNoTracking()
            .ToListAsync();
        return Ok(games);
    }

    //GET /games/id
    [HttpGet("{id}")]
    public async Task<IActionResult> GetGamesById(int id)
    {
        var game = await _gameStoreContext.Games.FindAsync(id);
        if (game is null)
        {
            return NotFound();
        }
        return Ok(game.ToGameDetailsDto());
    }

    private async Task<Genre?> GetGenre(int id)
    {
        return await _gameStoreContext.Genres.FindAsync(id);
    }

    // POST /games
    [HttpPost]
    public async Task<IActionResult> CreateGame(CreateGameDto newGame)
    {
        var game = newGame.ToEntity();

        await _gameStoreContext.Games.AddAsync(game);
        await _gameStoreContext.SaveChangesAsync();


        return CreatedAtAction(nameof(GetGamesById), new { id = game.Id }, game.ToGameDetailsDto());
    }

    // PUT /games/id
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateGame(int id, UpdateGameDto updateGame)
    {
        var gameToUpdate = await _gameStoreContext.Games.FindAsync(id);

        if (gameToUpdate is null)
        {
            return NotFound();
        }

        _gameStoreContext.Entry(gameToUpdate)
            .CurrentValues
            .SetValues(updateGame.ToEntity(id));
        await _gameStoreContext.SaveChangesAsync();

        return NoContent();
    }

    // DELETE /games/1
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteGame(int id)
    {
        await _gameStoreContext.Games.Where(g => g.Id == id).ExecuteDeleteAsync();

        return NoContent();
    }
}
