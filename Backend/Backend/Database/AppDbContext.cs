using Backend.Auth.Users;
using Backend.Features.NewEmployee;
using Backend.Features.OldEmployee;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Backend.Database;

public class AppDbContext : IdentityDbContext<User, Role, string>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

    public DbSet<OldEmployee> OldEmployees { get; set; }
    
    public DbSet<NewEmployee> NewEmployees { get; set; }
}