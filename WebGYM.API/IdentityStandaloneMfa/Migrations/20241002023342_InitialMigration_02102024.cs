using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IdentityStandaloneMfa.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration_02102024 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SAMLConfigurationAttributesID",
                table: "SAML_Configuration");

            migrationBuilder.AddColumn<int>(
                name: "SAMLConfigurationID",
                table: "SAML_ConfigurationAttributes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SAMLConfigurationID",
                table: "SAML_ConfigurationAttributes");

            migrationBuilder.AddColumn<int>(
                name: "SAMLConfigurationAttributesID",
                table: "SAML_Configuration",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
