using Pokedex.Models;

namespace Pokedex.Services;
public interface IPokemonService
{
    Task AddPokemon(Pokemon pokemon);
    Task<List<Pokemon>> GetPokemon();
    Task<Pokemon?> GetByID(string id);
    Task<Pokemon?> GetByName(string name);
    Task<Pokemon?> UpdatePokemon(string id, Pokemon updatedPokemon);
    Task<bool> DeletePokemon(string id);
}