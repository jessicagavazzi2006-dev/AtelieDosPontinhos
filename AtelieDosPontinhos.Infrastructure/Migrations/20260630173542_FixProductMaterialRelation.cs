using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AtelieDosPontinhos.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixProductMaterialRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductMaterials_Materials_MaterialId1",
                table: "ProductMaterials");

            migrationBuilder.DropIndex(
                name: "IX_ProductMaterials_MaterialId1",
                table: "ProductMaterials");

            migrationBuilder.DropColumn(
                name: "MaterialId1",
                table: "ProductMaterials");

            migrationBuilder.AlterColumn<string>(
                name: "CoverImageUrl",
                table: "Products",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CoverImageUrl",
                table: "Products",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(2000)",
                oldMaxLength: 2000);

            migrationBuilder.AddColumn<int>(
                name: "MaterialId1",
                table: "ProductMaterials",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductMaterials_MaterialId1",
                table: "ProductMaterials",
                column: "MaterialId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductMaterials_Materials_MaterialId1",
                table: "ProductMaterials",
                column: "MaterialId1",
                principalTable: "Materials",
                principalColumn: "Id");
        }
    }
}
