namespace api_valheim.Repository;

using api_valheim.models;

public interface IUserRepository
{
    User Add(User user);
    User? GetUserByEmail(string email);
}