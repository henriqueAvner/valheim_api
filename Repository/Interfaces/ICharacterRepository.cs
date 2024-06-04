using api_valheim.models;

namespace api_valheim.Repository;

public interface ICharacterRepository
{

    Character AddCharacter(Character character);
    IEnumerable<Character> GetCharacters();

}