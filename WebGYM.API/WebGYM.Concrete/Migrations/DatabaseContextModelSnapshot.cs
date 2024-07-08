﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebGYM.Concrete;

#nullable disable

namespace WebGYM.Concrete.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    partial class DatabaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("WebGYM.Models.MemberRegistration", b =>
                {
                    b.Property<long>("MemberId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("MemberId"));

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Age")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal?>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Contactno")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long?>("Createdby")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("Dob")
                        .HasColumnType("datetime2");

                    b.Property<string>("EmailId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Gender")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("JoiningDate")
                        .HasColumnType("datetime2");

                    b.Property<long?>("MainMemberId")
                        .HasColumnType("bigint");

                    b.Property<string>("MemImagePath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MemImagename")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MemberFName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MemberLName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MemberMName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MemberNo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long?>("ModifiedBy")
                        .HasColumnType("bigint");

                    b.Property<int?>("PlanID")
                        .HasColumnType("int");

                    b.Property<int?>("SchemeID")
                        .HasColumnType("int");

                    b.HasKey("MemberId");

                    b.HasIndex("PlanID", "SchemeID")
                        .HasDatabaseName("IX_MemberRegistration_PlanID_Ascending");

                    b.ToTable("MemberRegistration");
                });

            modelBuilder.Entity("WebGYM.Models.PaymentDetails", b =>
                {
                    b.Property<long>("PaymentID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("PaymentID"));

                    b.Property<DateTime?>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("Createdby")
                        .HasColumnType("int");

                    b.Property<long?>("MemberID")
                        .HasColumnType("bigint");

                    b.Property<string>("MemberNo")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int?>("ModifiedBy")
                        .HasColumnType("int");

                    b.Property<DateTime?>("ModifyDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("NextRenwalDate")
                        .HasColumnType("datetime2");

                    b.Property<decimal?>("PaymentAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("PaymentFromdt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("PaymentTodt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Paymenttype")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PlanID")
                        .HasColumnType("int");

                    b.Property<string>("RecStatus")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("WorkouttypeID")
                        .HasColumnType("int");

                    b.HasKey("PaymentID");

                    b.HasIndex("WorkouttypeID", "PlanID", "MemberID", "MemberNo")
                        .HasDatabaseName("IX_PaymentDetails_All_Ascending");

                    b.ToTable("PaymentDetails");
                });

            modelBuilder.Entity("WebGYM.Models.PeriodTB", b =>
                {
                    b.Property<int>("PeriodID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PeriodID"));

                    b.Property<string>("Text")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("PeriodID");

                    b.HasIndex("Text", "Value")
                        .HasDatabaseName("IX_PeriodTB_Text_Value_Ascending");

                    b.ToTable("PeriodTB");
                });

            modelBuilder.Entity("WebGYM.Models.PlanMaster", b =>
                {
                    b.Property<int>("PlanID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PlanID"));

                    b.Property<DateTime?>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("CreateUserID")
                        .HasColumnType("int");

                    b.Property<DateTime?>("ModifyDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("ModifyUserID")
                        .HasColumnType("int");

                    b.Property<int>("PeriodID")
                        .HasColumnType("int");

                    b.Property<decimal?>("PlanAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("PlanName")
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("RecStatus")
                        .HasColumnType("bit");

                    b.Property<int?>("SchemeID")
                        .HasColumnType("int");

                    b.Property<string>("ServiceTax")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal?>("ServicetaxAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("ServicetaxNo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal?>("TotalAmount")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("PlanID");

                    b.HasIndex("PlanName")
                        .HasDatabaseName("IX_PlanMaster_PlanName_Ascending");

                    b.HasIndex("PeriodID", "SchemeID")
                        .HasDatabaseName("IX_PlanMaster_PeriodID_SchemeID_Ascending");

                    b.ToTable("PlanMaster");
                });

            modelBuilder.Entity("WebGYM.Models.Role", b =>
                {
                    b.Property<int>("RoleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RoleId"));

                    b.Property<string>("RoleName")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.HasKey("RoleId");

                    b.HasIndex("RoleName", "Status")
                        .HasDatabaseName("IX_Role_RoleName_Ascending");

                    b.ToTable("Role");
                });

            modelBuilder.Entity("WebGYM.Models.SchemeMaster", b =>
                {
                    b.Property<int>("SchemeID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SchemeID"));

                    b.Property<int>("Createdby")
                        .HasColumnType("int");

                    b.Property<DateTime>("Createddate")
                        .HasColumnType("datetime2");

                    b.Property<string>("SchemeName")
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.HasKey("SchemeID");

                    b.HasIndex("SchemeName")
                        .HasDatabaseName("IX_SchemeMaster_SchemeName_Ascending");

                    b.ToTable("SchemeMaster");
                });

            modelBuilder.Entity("WebGYM.Models.Users", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"));

                    b.Property<string>("Contactno")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("Createdby")
                        .HasColumnType("int");

                    b.Property<string>("EmailId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("FullName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RefreshToken")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("RefreshTokenExpiryTime")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId");

                    b.HasIndex("UserName", "Password", "RefreshToken", "RefreshTokenExpiryTime", "EmailId")
                        .HasDatabaseName("IX_Users_All_Ascending");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("WebGYM.Models.UsersInRoles", b =>
                {
                    b.Property<int>("UserRolesId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserRolesId"));

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("UserRolesId");

                    b.HasIndex("UserId", "RoleId")
                        .HasDatabaseName("IX_UsersInRoles_All_Ascending");

                    b.ToTable("UsersInRoles");
                });
#pragma warning restore 612, 618
        }
    }
}
