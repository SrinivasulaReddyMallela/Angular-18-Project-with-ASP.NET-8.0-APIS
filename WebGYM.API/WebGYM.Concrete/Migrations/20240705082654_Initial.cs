using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebGYM.Concrete.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MemberRegistration",
                columns: table => new
                {
                    MemberId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MemberNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MemberFName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MemberLName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MemberMName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Dob = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Age = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Contactno = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PlanID = table.Column<int>(type: "int", nullable: true),
                    SchemeID = table.Column<int>(type: "int", nullable: true),
                    Createdby = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: true),
                    JoiningDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MainMemberId = table.Column<long>(type: "bigint", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    MemImagePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MemImagename = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberRegistration", x => x.MemberId);
                });

            migrationBuilder.CreateTable(
                name: "PaymentDetails",
                columns: table => new
                {
                    PaymentID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlanID = table.Column<int>(type: "int", nullable: false),
                    WorkouttypeID = table.Column<int>(type: "int", nullable: true),
                    Paymenttype = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentFromdt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaymentTodt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaymentAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    NextRenwalDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Createdby = table.Column<int>(type: "int", nullable: true),
                    ModifyDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    RecStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MemberID = table.Column<long>(type: "bigint", nullable: true),
                    MemberNo = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentDetails", x => x.PaymentID);
                });

            migrationBuilder.CreateTable(
                name: "PeriodTB",
                columns: table => new
                {
                    PeriodID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Value = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PeriodTB", x => x.PeriodID);
                });

            migrationBuilder.CreateTable(
                name: "PlanMaster",
                columns: table => new
                {
                    PlanID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlanName = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    PlanAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ServicetaxAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ServiceTax = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUserID = table.Column<int>(type: "int", nullable: true),
                    ModifyDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifyUserID = table.Column<int>(type: "int", nullable: true),
                    RecStatus = table.Column<bool>(type: "bit", nullable: false),
                    SchemeID = table.Column<int>(type: "int", nullable: true),
                    PeriodID = table.Column<int>(type: "int", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ServicetaxNo = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanMaster", x => x.PlanID);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "SchemeMaster",
                columns: table => new
                {
                    SchemeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SchemeName = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Createdby = table.Column<int>(type: "int", nullable: false),
                    Createddate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchemeMaster", x => x.SchemeID);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Contactno = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Createdby = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    RefreshTokenExpiryTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RefreshToken = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "UsersInRoles",
                columns: table => new
                {
                    UserRolesId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersInRoles", x => x.UserRolesId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MemberRegistration_PlanID_Ascending",
                table: "MemberRegistration",
                columns: new[] { "PlanID", "SchemeID" });

            migrationBuilder.CreateIndex(
                name: "IX_PaymentDetails_All_Ascending",
                table: "PaymentDetails",
                columns: new[] { "WorkouttypeID", "PlanID", "MemberID", "MemberNo" });

            migrationBuilder.CreateIndex(
                name: "IX_PeriodTB_Text_Value_Ascending",
                table: "PeriodTB",
                columns: new[] { "Text", "Value" });

            migrationBuilder.CreateIndex(
                name: "IX_PlanMaster_PeriodID_SchemeID_Ascending",
                table: "PlanMaster",
                columns: new[] { "PeriodID", "SchemeID" });

            migrationBuilder.CreateIndex(
                name: "IX_PlanMaster_PlanName_Ascending",
                table: "PlanMaster",
                column: "PlanName");

            migrationBuilder.CreateIndex(
                name: "IX_Role_RoleName_Ascending",
                table: "Role",
                columns: new[] { "RoleName", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_SchemeMaster_SchemeName_Ascending",
                table: "SchemeMaster",
                column: "SchemeName");

            migrationBuilder.CreateIndex(
                name: "IX_Users_All_Ascending",
                table: "Users",
                columns: new[] { "UserName", "Password", "RefreshToken", "RefreshTokenExpiryTime", "EmailId" });

            migrationBuilder.CreateIndex(
                name: "IX_UsersInRoles_All_Ascending",
                table: "UsersInRoles",
                columns: new[] { "UserId", "RoleId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MemberRegistration");

            migrationBuilder.DropTable(
                name: "PaymentDetails");

            migrationBuilder.DropTable(
                name: "PeriodTB");

            migrationBuilder.DropTable(
                name: "PlanMaster");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "SchemeMaster");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "UsersInRoles");
        }
    }
}
