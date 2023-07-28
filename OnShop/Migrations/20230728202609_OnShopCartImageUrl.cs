using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnShop.Migrations
{
    /// <inheritdoc />
    public partial class OnShopCartImageUrl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "ShoppingCarts",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "ShoppingCarts");
        }
    }
}
