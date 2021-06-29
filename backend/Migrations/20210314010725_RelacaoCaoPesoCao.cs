using Microsoft.EntityFrameworkCore.Migrations;

namespace PetFelizApi.Migrations
{
    public partial class RelacaoCaoPesoCao : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Peso",
                table: "Cao");

            migrationBuilder.AddColumn<int>(
                name: "PesoId",
                table: "Cao",
                type: "int",
                nullable: false);

            migrationBuilder.CreateTable(
                name: "PesoCao",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descricao = table.Column<string>(type: "varchar(10)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PesoCao", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cao_PesoId",
                table: "Cao",
                column: "PesoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cao_PesoCao_PesoId",
                table: "Cao",
                column: "PesoId",
                principalTable: "PesoCao",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cao_PesoCao_PesoId",
                table: "Cao");

            migrationBuilder.DropTable(
                name: "PesoCao");

            migrationBuilder.DropIndex(
                name: "IX_Cao_PesoId",
                table: "Cao");

            migrationBuilder.DropColumn(
                name: "PesoId",
                table: "Cao");

            migrationBuilder.AddColumn<string>(
                name: "Peso",
                table: "Cao",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
