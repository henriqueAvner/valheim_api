using api_valheim.models;

namespace api_valheim.Repository;

public interface IPlayerRepository
{
    Player AddPlayer(Player player);
    IEnumerable<Player> GetPlayers();
}