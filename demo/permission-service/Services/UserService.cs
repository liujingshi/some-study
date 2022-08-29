using permission.Entities;
using permission.Repositories;
using System.Security.Cryptography;
using System.Text;

namespace permission.Services;

public class UserService : IUserService
{
    private UserRepository UserRepository;

    public UserService(UserRepository UserRepository)
    {
        this.UserRepository = UserRepository;
    }

    public string Add()
    {

        var md5 = MD5.Create();
        User user = new()
        {
            Id = Guid.NewGuid().ToString(),
            Username = "admin",
            Password = BitConverter.ToString(md5.ComputeHash(Encoding.UTF8.GetBytes("123456"))).Replace("-", "")
        };
        string addstring = UserRepository.AddUser(user);
        return addstring;
    }

    public List<User> GetAll()
    {
        List<User> users = UserRepository.GetAllUser();
        return users;
    }

    public bool CheckUser(string username, string password)
    {
        var md5 = MD5.Create();
        password = BitConverter.ToString(md5.ComputeHash(Encoding.UTF8.GetBytes(password))).Replace("-", "");
        User user = UserRepository.GetUser(username, password);
        return user != null;
    }

}
