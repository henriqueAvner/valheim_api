using api_valheim.models;

namespace api_valheim.Repository;

public class PlayerRepository : IPlayerRepository
{

    protected readonly ValheimContext _valheim;

    public PlayerRepository(ValheimContext context)
    {
        _valheim = context;
    }

    public Player AddPlayer(Player player)
    {
        var findPlayer = _valheim.Players.FirstOrDefault(p => p.Email == player.Email);

        if (findPlayer != null)
        {
            throw new InvalidOperationException("This player already exist.");
        }
        player.DateJoined = DateTime.Now;
        _valheim.Players.Add(player);
        _valheim.SaveChanges();

        return player;
    }

    public IEnumerable<Player> GetPlayers()
    {
        return _valheim.Players.ToList();
    }

    public void DeletePlayer(int playerId)
    {
        var findPlayerById = _valheim.Players.Find(playerId);

        if (findPlayerById == null)
        {
            throw new KeyNotFoundException("Player Not Found");
        }
        _valheim.Players.Remove(findPlayerById);
        _valheim.SaveChanges();
    }

}