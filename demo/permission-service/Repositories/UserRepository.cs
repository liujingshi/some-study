using permission.Entities;

namespace permission.Repositories;

public class UserRepository
{
    private readonly MyDbContext _dbContext;

    public UserRepository(MyDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public string AddUser(User user)
    {
        var message = "";
        {
            _dbContext.User.Add(user);
            var i = _dbContext.SaveChanges();
            message = i > 0 ? "add successed" : "add failed";
        }
        return message;
    }

    public User GetUser(string username, string password)
    {
        User result = null;
        foreach (var user in this.GetAllUser())
        {
            if (user.Username == username && user.Password == password)
            {
                result = user;
                break;
            }
        }
        return result;
    }

    public List<User> GetAllUser()
    {
        using (_dbContext)
        {
            List<User> ps = _dbContext.User.ToList();
            return ps;
        }
    }
}
