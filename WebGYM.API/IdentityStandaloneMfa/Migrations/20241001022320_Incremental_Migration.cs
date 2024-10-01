using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IdentityStandaloneMfa.Migrations
{
    /// <inheritdoc />
    public partial class Incremental_Migration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SAML_Configuration",
                columns: table => new
                {
                    SAMLConfigurationID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Recipient = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Target = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CertFileLocation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CertPassword = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CertStoreName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CertStoreLocation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CertFindKey = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CertFriendlyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CertFindMethod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Issuer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Domain = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Version = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    APPKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    SAMLConfigurationAttributesID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SAML_Configuration", x => x.SAMLConfigurationID);
                });

            migrationBuilder.CreateTable(
                name: "SAML_ConfigurationAttributes",
                columns: table => new
                {
                    SAMLConfigurationAttributesID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Sequence = table.Column<int>(type: "int", nullable: false),
                    AttributeName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AttributeValue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SAML_ConfigurationAttributes", x => x.SAMLConfigurationAttributesID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SAMLConfiguration_APPKey",
                table: "SAML_Configuration",
                column: "APPKey");

            migrationBuilder.CreateIndex(
                name: "IX_SAMLAttributes_AttributeName",
                table: "SAML_ConfigurationAttributes",
                column: "AttributeName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SAML_Configuration");

            migrationBuilder.DropTable(
                name: "SAML_ConfigurationAttributes");
        }
    }
}
