using IdentityStandaloneMfa.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace IdentityStandaloneMfa.Data
{
    public class DatabaseContext : IdentityDbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DatabaseConnection"));
            //optionsBuilder.UseSqlServer("Server=DESKTOP-PV0A2LP;Database=UserInfo;Integrated Security=true;TrustServerCertificate=true;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;");
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            /*
            modelBuilder
               .HasAnnotation("ProductVersion", "3.1.0");

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
            {
                b.Property<string>("Id")
                    .HasColumnType("NVARCHAR(450)");

                b.Property<string>("ConcurrencyStamp")
                    .IsConcurrencyToken()
                    .HasColumnType("NVARCHAR(450)");

                b.Property<string>("Name")
                    .HasColumnType("NVARCHAR(450)")
                    .HasMaxLength(256);

                b.Property<string>("NormalizedName")
                    .HasColumnType("NVARCHAR(450)")
                    .HasMaxLength(256);

                b.HasKey("Id");

                b.HasIndex("NormalizedName")
                    .IsUnique()
                    .HasName("RoleNameIndex");

                b.ToTable("AspNetRoles");
            });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("INTEGER");

                b.Property<string>("ClaimType")
                    .HasColumnType("NVARCHAR(450)");

                b.Property<string>("ClaimValue")
                    .HasColumnType("NVARCHAR(450)");

                b.Property<string>("RoleId")
                    .IsRequired()
                    .HasColumnType("NVARCHAR(450)");

                b.HasKey("Id");

                b.HasIndex("RoleId");

                b.ToTable("AspNetRoleClaims");
            });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", b =>
            {
                b.Property<string>("Id")
                    .HasColumnType("NVARCHAR(450)");

                b.Property<int>("AccessFailedCount")
                    .HasColumnType("INTEGER");

                b.Property<string>("ConcurrencyStamp")
                    .IsConcurrencyToken()
                    .HasColumnType("NVARCHAR(450)");

                b.Property<string>("Email")
                    .HasColumnType("NVARCHAR(450)")
                    .HasMaxLength(256);

                b.Property<bool>("EmailConfirmed")
                    .HasColumnType("INTEGER");

                b.Property<bool>("LockoutEnabled")
                    .HasColumnType("INTEGER");

                b.Property<DateTimeOffset?>("LockoutEnd")
                    .HasColumnType("NVARCHAR(450)");

                b.Property<string>("NormalizedEmail")
                    .HasColumnType("NVARCHAR(450)")
                    .HasMaxLength(256);

                b.Property<string>("NormalizedUserName")
                    .HasColumnType("NVARCHAR(450)")
                    .HasMaxLength(256);

                b.Property<string>("PasswordHash")
                    .HasColumnType("NVARCHAR(450)");

                b.Property<string>("PhoneNumber")
                    .HasColumnType("NVARCHAR(450)");

                b.Property<bool>("PhoneNumberConfirmed")
                    .HasColumnType("INTEGER");

                b.Property<string>("SecurityStamp")
                    .HasColumnType("NVARCHAR(450)");

                b.Property<bool>("TwoFactorEnabled")
                    .HasColumnType("INTEGER");

                b.Property<string>("UserName")
                    .HasColumnType("NVARCHAR(450)")
                    .HasMaxLength(256);

                b.HasKey("Id");

                b.HasIndex("NormalizedEmail")
                    .HasName("EmailIndex");

                b.HasIndex("NormalizedUserName")
                    .IsUnique()
                    .HasName("UserNameIndex");

                b.ToTable("AspNetUsers");
            });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("INTEGER");

                b.Property<string>("ClaimType")
                    .HasColumnType("NVARCHAR(450)");

                b.Property<string>("ClaimValue")
                    .HasColumnType("NVARCHAR(450)");

                b.Property<string>("UserId")
                    .IsRequired()
                    .HasColumnType("NVARCHAR(450)");

                b.HasKey("Id");

                b.HasIndex("UserId");

                b.ToTable("AspNetUserClaims");
            });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
            {
                b.Property<string>("LoginProvider")
                    .HasColumnType("NVARCHAR(450)")
                    .HasMaxLength(128);

                b.Property<string>("ProviderKey")
                    .HasColumnType("NVARCHAR(450)")
                    .HasMaxLength(128);

                b.Property<string>("ProviderDisplayName")
                    .HasColumnType("NVARCHAR(450)");

                b.Property<string>("UserId")
                    .IsRequired()
                    .HasColumnType("NVARCHAR(450)");

                b.HasKey("LoginProvider", "ProviderKey");

                b.HasIndex("UserId");

                b.ToTable("AspNetUserLogins");
            });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
            {
                b.Property<string>("UserId")
                    .HasColumnType("NVARCHAR(450)");

                b.Property<string>("RoleId")
                    .HasColumnType("NVARCHAR(450)");

                b.HasKey("UserId", "RoleId");

                b.HasIndex("RoleId");

                b.ToTable("AspNetUserRoles");
            });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
            {
                b.Property<string>("UserId")
                    .HasColumnType("NVARCHAR(450)");

                b.Property<string>("LoginProvider")
                    .HasColumnType("NVARCHAR(450)")
                    .HasMaxLength(128);

                b.Property<string>("Name")
                    .HasColumnType("NVARCHAR(450)")
                    .HasMaxLength(128);

                b.Property<string>("Value")
                    .HasColumnType("NVARCHAR(450)");

                b.HasKey("UserId", "LoginProvider", "Name");

                b.ToTable("AspNetUserTokens");
            });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
            {
                b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                    .WithMany()
                    .HasForeignKey("RoleId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
            });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
            {
                b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                    .WithMany()
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
            });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
            {
                b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                    .WithMany()
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
            });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
            {
                b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                    .WithMany()
                    .HasForeignKey("RoleId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                    .WithMany()
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
            });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
            {
                b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                    .WithMany()
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
            });
            */
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<SAMLConfiguration>()
            .HasIndex(b => new
            {
                b.APPKey,
            }).HasDatabaseName("IX_SAMLConfiguration_APPKey");
            modelBuilder.Entity<SAMLAttributes>()
           .HasIndex(b => new
           {
               b.AttributeName,
           }).HasDatabaseName("IX_SAMLAttributes_AttributeName");


            modelBuilder.Entity<IdentityUserLogin<string>>()
                .HasKey(login => new { login.LoginProvider, login.ProviderKey });

        }
        public DbSet<SAMLConfiguration> SAMLConfiguration { get; set; }
        public DbSet<SAMLAttributes> SAMLAttributes { get; set; }

    }

    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<DatabaseContext>
    {
        public DatabaseContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                            .AddJsonFile("appsettings.json")
                            .Build();
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DatabaseConnection"));
            // optionsBuilder.UseSqlServer("Server=DESKTOP-PV0A2LP;Database=UserInfo;Integrated Security=true;TrustServerCertificate=true;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;");
            return new DatabaseContext(optionsBuilder.Options);
        }
    }
}
