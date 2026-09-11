using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AtelieDosPontinhos.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarColunaDestaque : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Destaque",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Destaque",
                table: "Products");
        }
    }
}
