using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmtpD.Infrastructure.Sqlite.Migrations
{
    public partial class AddedReadedFlag : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "readed",
                table: "emails",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "readed",
                table: "emails");
        }
    }
}
