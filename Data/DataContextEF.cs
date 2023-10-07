using DotNetAPI.Models;
using Microsoft.EntityFrameworkCore;


namespace DotNetAPI.Data;

public class DataContextEf : DbContext
{
    private readonly IConfiguration _config;

    public DataContextEf(IConfiguration config)
    {
        _config = config;
    }

    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<UserJobInfo> UserJobInfo { get; set; }
    public virtual DbSet<UserSalary> UserSalary { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder
                .UseSqlServer(_config.GetConnectionString("DefaultConnection"),
                    optionsBuilder => optionsBuilder.EnableRetryOnFailure());
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("TutorialAppSchema");
        modelBuilder.Entity<UserJobInfo>()
            .ToTable("UserJobInfo", "TutorialAppSchema")
            .HasKey(obj => obj.UserId);

        modelBuilder.Entity<UserJobInfo>()
            .ToTable("UserJobInfo", "TutorialAppSchema")
            .HasKey(obj => obj.UserId);

        modelBuilder.Entity<UserSalary>()
            .ToTable("UserSalary", "TutorialAppSchema")
            .HasKey(obj => obj.UserId);
    }
}