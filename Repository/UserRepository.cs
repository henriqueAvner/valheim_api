namespace Auth.Repository;

using api_valheim.models;
using api_valheim.Repository;

public class UserRepository : IUserRepository
{

    private readonly IValheimContext _valheim;
    public UserRepository(IValheimContext valheim)
    {
        _valheim = valheim;
    }

    public User Add(User user)
    {
        _valheim.Users.Add(user);
        _valheim.SaveChanges();
        return user;
    }
    public User? GetUserByEmail(string email)
    {
        User? existingUser = _valheim.Users.Where(u => u.Email == email).FirstOrDefault();
        return existingUser;
    }
}