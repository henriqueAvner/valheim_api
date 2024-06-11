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
        var characterExists = _valheim.Characters.FirstOrDefault(c => c.CharacterId == character.CharacterId);

        if (characterExists == null)
        {
            throw new InvalidOperationException("Character already exists.");
        }
        _valheim.Characters.Add(character);
        _valheim.SaveChanges();

        return character;
    }

    public void DeleteCharacter(int CharacterId)
    {
        var findCharacter = _valheim.Characters.Find(CharacterId) ?? throw new KeyNotFoundException("Character not found");

        _valheim.Characters.Remove(findCharacter);
        _valheim.SaveChanges();
    }

    public IEnumerable<Character> GetCharacters()
    {
        return _valheim.Characters.ToList();
    }
}