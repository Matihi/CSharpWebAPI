using Microsoft.AspNetCore.Mvc;
using Pokedex.Models;
using Pokedex.Services;

namespace Pokedex.Controllers;
[ApiController]
[Route("[controller]")]
public class PokemonController : ControllerBase
{
    private readonly IPokemonService _pokemonService;

    public PokemonController(IPokemonService pokemonService)
    {
        _pokemonService = pokemonService;
    }

    [HttpPost("Add")]
    public async Task<IActionResult> AddPokemon([FromBody] Pokemon pokemon)
    {
        
        try{
            await _pokemonService.AddPokemon(pokemon);
            return Ok("Pokemon added successfully.");
        }
        catch(Exception e) {
            return BadRequest(e.Message);
        }
        
    }

    [HttpGet("GetAll")]
    public async Task<IActionResult> GetPokemon()
    {
        return Ok(await _pokemonService.GetPokemon());
    }

    [HttpGet("GetById/{id}")]
    public async Task<IActionResult> GetByID(string id)
    {
        try {
            var pokemon = await _pokemonService.GetByID(id);
            return Ok(pokemon);
        }
        catch{
            return NotFound("Pokemon not found.");
        }
    }

    [HttpGet("search/{name}")]
    public async Task<IActionResult> GetByName(string name)
    {
        try{
            var pokemon = await _pokemonService.GetByName(name);
            return Ok(pokemon);
        }
        catch{
            return NotFound("Pokemon not found.");
        }
    }

    [HttpPut("Update/{id}")]
    public async Task<IActionResult> UpdatePokemon(string id, [FromBody] Pokemon updatedPokemon)
    {
        try {
            if (await _pokemonService.UpdatePokemon(id, updatedPokemon) != null) {
                return Ok("Pokemon updated successfully.");
            }
            return NotFound("Pokemon Not Found.");
        }
        catch(Exception e) {
            return NotFound(e.Message);
        }
        
    }

    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> DeletePokemon(string id)
    {
        try {
            if (await _pokemonService.DeletePokemon(id) == true) {
                return Ok("Pokemon deleted successfully.");
            }
            return NotFound("Pokemon Not Found");
        }
        catch(Exception e) {
            return NotFound(e.Message);
        }
    }
}


