using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Walruslogics.Advertisement.Database.Models
{
  public partial class AdvertisementDBContext : IdentityDbContext<AppUser, AppRole, long, AppUserClaim, AppUserRole, AppUserLogin, AppRoleClaim, AppUserToken>
  {
    public AdvertisementDBContext(DbContextOptions<AdvertisementDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AppUser> AppUsers { get; set; }
    public virtual DbSet<AppRole> AppRoles { get; set; }

    public virtual DbSet<AppUserRole> AppUserRoles { get; set; }
    public virtual DbSet<AppRoleClaim> AppRoleClaims { get; set; }
    public virtual DbSet<AppUserLogin> AppUserLogins { get; set; }
    public virtual DbSet<AppUserToken> AppUserTokens { get; set; }
    public virtual DbSet<UserProfile> UserProfiles { get; set; }

    [NotMapped]
    public virtual DbSet<Country> Country { get; set; }
    [NotMapped]
    public virtual DbSet<City> City { get; set; }
    [NotMapped]
    public virtual DbSet<State> State { get; set; }

    public DbSet<AppUserClaim> AppUserClaims { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);

      modelBuilder.Entity<AppRole>().HasData(new AppRole()
      {
        Id = 1,
        Name = "Super Admin",
        RoleKey = "SuperAdmin",
        Description = "Super Admin Can not be archived.",
        NormalizedName = "Super Admin"

      });

      modelBuilder.Entity<AppRole>().HasData(new AppRole()
      {
        Id = 2,
        Name = "Admin",
        RoleKey = "Admin",
        Description = "Admin Role.",
        NormalizedName = "Admin"
      });

      modelBuilder.Entity<AppRole>().HasData(new AppRole()
      {
        Id = 3,
        Name = "User",
        RoleKey = "User",
        Description = "User Role.",
        NormalizedName = "User"
      });
    }
  }
}
