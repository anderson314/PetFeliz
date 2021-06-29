using Microsoft.EntityFrameworkCore.Migrations;

namespace PetFelizApi.Migrations
{
    public partial class TirarValorServico : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ValorServico",
                table: "ServicoDogWalker");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "ValorServico",
                table: "ServicoDogWalker",
                type: "decimal(3,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
