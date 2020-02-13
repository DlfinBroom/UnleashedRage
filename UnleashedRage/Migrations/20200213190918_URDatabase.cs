using Microsoft.EntityFrameworkCore.Migrations;

namespace UnleashedRage.Migrations
{
    public partial class URDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "SendEmail",
                table: "User",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<double>(
                name: "Price",
                table: "Merch",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SendEmail",
                table: "User");

            migrationBuilder.AlterColumn<int>(
                name: "Price",
                table: "Merch",
                type: "int",
                nullable: false,
                oldClrType: typeof(double));
        }
    }
}
