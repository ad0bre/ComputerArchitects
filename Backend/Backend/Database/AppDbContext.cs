using Backend.Auth.Users;
using Backend.Features.FormResponses;
using Backend.Features.NewEmployee;
using Backend.Features.OldEmployee;
using Backend.Features.QuestionOptions;
using Backend.Features.Questions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Backend.Database;

public class AppDbContext : IdentityDbContext<User, Role, string>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

    public DbSet<OldEmployee> OldEmployees { get; set; }
    
    public DbSet<NewEmployee> NewEmployees { get; set; }
    
    public DbSet<QuestionModel> Questions { get; set; }
    
    public DbSet<QuestionOption> Options { get; set; }
    
    public DbSet<FormResponse> Responses { get; set; }
}