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
        _valheim.Players.Add(player);
        _valheim.SaveChanges();

        return player;
    }

    public IEnumerable<Player> GetPlayers()
    {
        return _valheim.Players;
    }
}