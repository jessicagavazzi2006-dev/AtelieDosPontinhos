using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AtelieDosPontinhos.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarDestaqueProduto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NomeCompleto",
                table: "Enderecos");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NomeCompleto",
                table: "Enderecos",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
