using Microsoft.EntityFrameworkCore;
using permission;
using permission.Repositories;
using permission.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<MyDbContext>(options =>
  options.UseMySql(builder.Configuration.GetConnectionString("MySQL"), MySqlServerVersion.LatestSupportedServerVersion));
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
