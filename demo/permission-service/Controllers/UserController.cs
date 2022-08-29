using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using permission.Entities;
using permission.Services;

namespace permission.Controllers;
[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private IUserService UserService;

    public UserController(IUserService UserService)
    {
        this.UserService = UserService;
    }

    [HttpGet]
    [Route("check/{username}/{password}")]
    public IActionResult CheckUser(string username, string password)
    {
        return Ok(UserService.CheckUser(username, password));
    }

    [HttpPost]
    public IActionResult AddUser()
    {
        string s = UserService.Add();
        return Ok(s);
    }

}
