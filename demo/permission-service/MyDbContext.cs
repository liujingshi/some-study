using permission.Entities;
using Microsoft.EntityFrameworkCore;

namespace permission;
public class MyDbContext : DbContext
{
    public DbSet<User>? User { get; set; }
    public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
    {

    }
}
