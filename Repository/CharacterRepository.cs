using api_valheim.models;

namespace api_valheim.Repository;

public class CharacterRepository : ICharacterRepository
{

    protected readonly ValheimContext _valheim;

    public CharacterRepository(ValheimContext context)
    {
        _valheim = context;
    }

    public Character AddCharacter(Character character)
    {
        _valheim.Characters.Add(character);
        _valheim.SaveChanges();

        return character;
    }

    public IEnumerable<Character> GetCharacters()
    {
        return _valheim.Characters;
    }
}