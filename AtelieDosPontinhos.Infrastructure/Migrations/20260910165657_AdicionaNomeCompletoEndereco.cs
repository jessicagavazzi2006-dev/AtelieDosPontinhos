using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AtelieDosPontinhos.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AdicionaNomeCompletoEndereco : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Enderecos_AspNetUsers_UserId",
                table: "Enderecos");

            migrationBuilder.DropForeignKey(
                name: "FK_Enderecos_AspNetUsers_UserId1",
                table: "Enderecos");

            migrationBuilder.DropIndex(
                name: "IX_Enderecos_UserId1",
                table: "Enderecos");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Enderecos");

            migrationBuilder.AlterColumn<string>(
                name: "CoverImageUrl",
                table: "Products",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldMaxLength: 2000,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NomeCompleto",
                table: "Enderecos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Enderecos_AspNetUsers_UserId",
                table: "Enderecos",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Enderecos_AspNetUsers_UserId",
                table: "Enderecos");

            migrationBuilder.DropColumn(
                name: "NomeCompleto",
                table: "Enderecos");

            migrationBuilder.AlterColumn<string>(
                name: "CoverImageUrl",
                table: "Products",
                type: "nvarchar(max)",
                maxLength: 2000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(2000)",
                oldMaxLength: 2000,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "Enderecos",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Enderecos_UserId1",
                table: "Enderecos",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Enderecos_AspNetUsers_UserId",
                table: "Enderecos",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Enderecos_AspNetUsers_UserId1",
                table: "Enderecos",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
