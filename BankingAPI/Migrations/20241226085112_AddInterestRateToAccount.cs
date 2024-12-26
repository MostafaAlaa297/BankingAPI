using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankingAPI.Migrations
{
    public partial class AddInterestRateToAccount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "InterestRate",
                table: "Accounts",
                type: "REAL",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "OverdraftLimit",
                table: "Accounts",
                type: "REAL",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InterestRate",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "OverdraftLimit",
                table: "Accounts");
        }
    }
}
