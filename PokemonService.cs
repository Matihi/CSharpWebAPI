using MongoDB.Bson;
using MongoDB.Driver;
using Pokedex.Models;

namespace Pokedex.Services;
public class PokemonService : IPokemonService
{
  private readonly IMongoCollection<Pokemon> _pokemonCollection;

  public PokemonService(IConfiguration configuration) {
    var client = new MongoClient(configuration["MongoDbSettings:ConnectionString"]);
    var database = client.GetDatabase(configuration["MongoDbSettings:DatabaseName"]);
    _pokemonCollection = database.GetCollection<Pokemon>(configuration["MongoDbSettings:CollectionName"]);
  }

  public async Task AddPokemon(Pokemon pokemon)
  {
    if (!string.IsNullOrEmpty(pokemon.Id) && !ObjectId.TryParse(pokemon.Id, out _))
    {
        throw new BsonSerializationException("The provided Id is not a valid 24-character hexadecimal string.");
    }
    
    await _pokemonCollection.InsertOneAsync(pokemon);
  }



  public async Task<List<Pokemon>> GetPokemon()
  {
    return await _pokemonCollection.Find(_ => true).ToListAsync();
  }

  public async Task<Pokemon?> GetByID(string id)
  {
    var pokemon = await _pokemonCollection.Find(p => p.Id == id).FirstOrDefaultAsync();
    if (pokemon == null) {
      throw new Exception("Pokemon not found");
    }
    return pokemon;
  }

  public async Task<Pokemon?> GetByName(string name) {
    var pokemon = await _pokemonCollection.Find(p => p.Name == name).FirstOrDefaultAsync();
    if (pokemon == null) {
      throw new Exception("Pokemon not found really?");
    }
    return pokemon;
  }

  public async Task<Pokemon?> UpdatePokemon(string id, Pokemon updatedPokemon)
  {
    var result = await _pokemonCollection.ReplaceOneAsync(p => p.Id == id, updatedPokemon);
    if (result.MatchedCount == 0) {
      throw new Exception("Pokemon not found for update");
    }
    return updatedPokemon;
  }

  public async Task<bool> DeletePokemon(string id)
  {
    var result = await _pokemonCollection.DeleteOneAsync(p => p.Id == id);
    if (result.DeletedCount == 0) {
      throw new Exception("Pokemon not found for deletion");
    }
    return true;
  }
}