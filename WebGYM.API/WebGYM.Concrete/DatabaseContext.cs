using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using WebGYM.Models;

namespace WebGYM.Concrete
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }
        public DatabaseContext()
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DatabaseConnection"));
            //optionsBuilder.UseSqlServer("Server=DESKTOP-PV0A2LP;Database=GymDataBase;Integrated Security=true;TrustServerCertificate=true;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;");
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SchemeMaster>()
            .HasIndex(b => new
            {
                b.SchemeName,
            }).HasDatabaseName("IX_SchemeMaster_SchemeName_Ascending");

            modelBuilder.Entity<PeriodTB>()
           .HasIndex(b => new
           {
               b.Text,
               b.Value
           }).HasDatabaseName("IX_PeriodTB_Text_Value_Ascending");

            modelBuilder.Entity<PlanMaster>()
            .HasIndex(b => new
            {
                b.PlanName,
            }).HasDatabaseName("IX_PlanMaster_PlanName_Ascending");

            modelBuilder.Entity<PlanMaster>()
          .HasIndex(b => new
          {
              b.PeriodID,
              b.SchemeID
          }).HasDatabaseName("IX_PlanMaster_PeriodID_SchemeID_Ascending");

            modelBuilder.Entity<Role>()
          .HasIndex(b => new
          {
              b.RoleName,
              b.Status
          }).HasDatabaseName("IX_Role_RoleName_Ascending");
            modelBuilder.Entity<MemberRegistration>()
         .HasIndex(b => new
         {
             b.PlanID,
             b.SchemeID
         }).HasDatabaseName("IX_MemberRegistration_PlanID_Ascending");

            modelBuilder.Entity<Users>()
        .HasIndex(b => new
        {
            b.UserName,
            b.Password,
            b.RefreshToken,
            b.RefreshTokenExpiryTime,
            b.EmailId
        }).HasDatabaseName("IX_Users_All_Ascending");

            modelBuilder.Entity<UsersInRoles>()
        .HasIndex(b => new
        {
            b.UserId,
            b.RoleId,
        }).HasDatabaseName("IX_UsersInRoles_All_Ascending");
            modelBuilder.Entity<PaymentDetails>()
        .HasIndex(b => new
        {
            b.WorkouttypeID,
            b.PlanID,
            b.MemberID,
            b.MemberNo,
        }).HasDatabaseName("IX_PaymentDetails_All_Ascending");

            modelBuilder.Entity<SchemeMaster>().Property(e => e.SchemeName).IsRequired(false);
           // modelBuilder.Entity<SchemeMaster>().Property(e => e.Createdby).IsRequired(false);
           // modelBuilder.Entity<SchemeMaster>().Property(e => e.Createddate).IsRequired(false);
            //modelBuilder.Entity<SchemeMaster>().Property(e => e.Status).IsRequired(false);

            modelBuilder.Entity<PeriodTB>().Property(e => e.Text).IsRequired(false);
            modelBuilder.Entity<PeriodTB>().Property(e => e.Value).IsRequired(false);

            modelBuilder.Entity<PlanMaster>().Property(e => e.PlanName).IsRequired(false);
            modelBuilder.Entity<PlanMaster>().Property(e => e.PlanAmount).IsRequired(false);
            modelBuilder.Entity<PlanMaster>().Property(e => e.ServicetaxAmount).IsRequired(false);
            modelBuilder.Entity<PlanMaster>().Property(e => e.ServiceTax).IsRequired(false);
            modelBuilder.Entity<PlanMaster>().Property(e => e.CreateDate).IsRequired(false);
            //modelBuilder.Entity<PlanMaster>().Property(e => e.CreateUserID).IsRequired(false);
            modelBuilder.Entity<PlanMaster>().Property(e => e.ModifyDate).IsRequired(false);
           // modelBuilder.Entity<PlanMaster>().Property(e => e.ModifyUserID).IsRequired(false);
           // modelBuilder.Entity<PlanMaster>().Property(e => e.RecStatus).IsRequired(false);
            //modelBuilder.Entity<PlanMaster>().Property(e => e.SchemeID).IsRequired(false);
           // modelBuilder.Entity<PlanMaster>().Property(e => e.PeriodID).IsRequired(false);
            modelBuilder.Entity<PlanMaster>().Property(e => e.TotalAmount).IsRequired(false);
            modelBuilder.Entity<PlanMaster>().Property(e => e.ServicetaxNo).IsRequired(false);

            modelBuilder.Entity<Role>().Property(e => e.RoleName).IsRequired(false);
            //modelBuilder.Entity<Role>().Property(e => e.Status).IsRequired(false);
            modelBuilder.Entity<MemberRegistration>().Property(e => e.MemberNo).IsRequired(false);
            modelBuilder.Entity<MemberRegistration>().Property(e => e.MemberFName).IsRequired(false);
            modelBuilder.Entity<MemberRegistration>().Property(e => e.MemberLName).IsRequired(false);
            modelBuilder.Entity<MemberRegistration>().Property(e => e.MemberMName).IsRequired(false);
            modelBuilder.Entity<MemberRegistration>().Property(e => e.Dob).IsRequired(false);
            modelBuilder.Entity<MemberRegistration>().Property(e => e.Age).IsRequired(false);
            modelBuilder.Entity<MemberRegistration>().Property(e => e.Contactno).IsRequired(false);
            modelBuilder.Entity<MemberRegistration>().Property(e => e.EmailId).IsRequired(false);
            modelBuilder.Entity<MemberRegistration>().Property(e => e.Gender).IsRequired(false);
            //modelBuilder.Entity<MemberRegistration>().Property(e => e.PlanID).IsRequired(false);
           // modelBuilder.Entity<MemberRegistration>().Property(e => e.SchemeID).IsRequired(false);
           // modelBuilder.Entity<MemberRegistration>().Property(e => e.Createdby).IsRequired(false);
            //modelBuilder.Entity<MemberRegistration>().Property(e => e.ModifiedBy).IsRequired(false);
            modelBuilder.Entity<MemberRegistration>().Property(e => e.JoiningDate).IsRequired(false);
            modelBuilder.Entity<MemberRegistration>().Property(e => e.Address).IsRequired(false);
            //modelBuilder.Entity<MemberRegistration>().Property(e => e.MainMemberId).IsRequired(false);
            modelBuilder.Entity<MemberRegistration>().Property(e => e.Amount).IsRequired(false);
            modelBuilder.Entity<MemberRegistration>().Property(e => e.MemImagePath).IsRequired(false);
            modelBuilder.Entity<MemberRegistration>().Property(e => e.MemImagename).IsRequired(false);


            modelBuilder.Entity<Users>().Property(e => e.FullName).IsRequired(false);
            modelBuilder.Entity<Users>().Property(e => e.EmailId).IsRequired(false);
            modelBuilder.Entity<Users>().Property(e => e.Contactno).IsRequired(false);
           // modelBuilder.Entity<Users>().Property(e => e.Createdby).IsRequired(false);
            modelBuilder.Entity<Users>().Property(e => e.CreatedDate).IsRequired(false);
           // modelBuilder.Entity<Users>().Property(e => e.Status).IsRequired(false);
          //  modelBuilder.Entity<Users>().Property(e => e.RefreshTokenExpiryTime).IsRequired(false);
            modelBuilder.Entity<Users>().Property(e => e.RefreshToken).IsRequired(false);

           // modelBuilder.Entity<UsersInRoles>().Property(e => e.RoleId).IsRequired(false);
          //  modelBuilder.Entity<UsersInRoles>().Property(e => e.UserId).IsRequired(false);
           // modelBuilder.Entity<PaymentDetails>().Property(e => e.PlanID).IsRequired(false);
           // modelBuilder.Entity<PaymentDetails>().Property(e => e.WorkouttypeID).IsRequired(false);
            modelBuilder.Entity<PaymentDetails>().Property(e => e.Paymenttype).IsRequired(false);
           // modelBuilder.Entity<PaymentDetails>().Property(e => e.PaymentFromdt).IsRequired(false);
            //modelBuilder.Entity<PaymentDetails>().Property(e => e.PaymentTodt).IsRequired(false);
            modelBuilder.Entity<PaymentDetails>().Property(e => e.PaymentAmount).IsRequired(false);
            //modelBuilder.Entity<PaymentDetails>().Property(e => e.NextRenwalDate).IsRequired(false);
            modelBuilder.Entity<PaymentDetails>().Property(e => e.CreateDate).IsRequired(false);
            //modelBuilder.Entity<PaymentDetails>().Property(e => e.Createdby).IsRequired(false);
            modelBuilder.Entity<PaymentDetails>().Property(e => e.ModifyDate).IsRequired(false);
           // modelBuilder.Entity<PaymentDetails>().Property(e => e.ModifiedBy).IsRequired(false);
            modelBuilder.Entity<PaymentDetails>().Property(e => e.RecStatus).IsRequired(false);
           // modelBuilder.Entity<PaymentDetails>().Property(e => e.MemberID).IsRequired(false);
            modelBuilder.Entity<PaymentDetails>().Property(e => e.MemberNo).IsRequired(false);
        }

        public DbSet<SchemeMaster> SchemeMaster { get; set; }
        public DbSet<PeriodTB> PeriodTb { get; set; }
        public DbSet<PlanMaster> PlanMaster { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<MemberRegistration> MemberRegistration { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<UsersInRoles> UsersInRoles { get; set; }
        public DbSet<PaymentDetails> PaymentDetails { get; set; }
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
           // optionsBuilder.UseSqlServer("Server=DESKTOP-PV0A2LP;Database=GymDataBase;Integrated Security=true;TrustServerCertificate=true;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;");
            return new DatabaseContext(optionsBuilder.Options);
        }
    }
}
