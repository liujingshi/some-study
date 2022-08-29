
using permission.Entities;

namespace permission.Services;

public interface IUserService
{
    public string Add();
    public List<User> GetAll();
    public bool CheckUser(string username, string password);
}
