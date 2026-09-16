using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AtelieDosPontinhos.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddNewApplicationUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<string>(
                name: "NomeCartao",
                table: "Pagamentos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NumeroCartao",
                table: "Pagamentos",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NomeCartao",
                table: "Pagamentos");

            migrationBuilder.DropColumn(
                name: "NumeroCartao",
                table: "Pagamentos");
        }
    }
}
