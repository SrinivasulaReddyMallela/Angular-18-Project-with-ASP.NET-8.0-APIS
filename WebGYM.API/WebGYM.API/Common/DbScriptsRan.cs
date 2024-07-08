using Microsoft.EntityFrameworkCore.Migrations;

namespace WebGYM.API.Common
{
    public partial  class DbScriptsRan : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            foreach (var sqlFile in Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory+ "/SQLScripts", "*.sql"))
            {
                var scriptContent = File.ReadAllText(sqlFile);
                migrationBuilder.Sql(scriptContent);
            }
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Rollback logic if needed
        }
    }
    public class ErrorResponse
    {
        public string Title { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
    }
}
