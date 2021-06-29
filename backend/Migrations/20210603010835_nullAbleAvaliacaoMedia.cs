using Microsoft.EntityFrameworkCore.Migrations;

namespace PetFelizApi.Migrations
{
    public partial class nullAbleAvaliacaoMedia : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "AvaliacaoMedia",
                table: "ServicoDogWalker",
                type: "decimal(5,2)",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");
            
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
