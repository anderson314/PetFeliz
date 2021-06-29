using Microsoft.EntityFrameworkCore.Migrations;

namespace PetFelizApi.Migrations
{
    public partial class RelacaoCursoInfoDogW : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InfoServDogWId",
                table: "Curso",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Curso_InfoServDogWId",
                table: "Curso",
                column: "InfoServDogWId");

            migrationBuilder.AddForeignKey(
                name: "FK_Curso_ServicoDogWalker_InfoServDogWId",
                table: "Curso",
                column: "InfoServDogWId",
                principalTable: "ServicoDogWalker",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Curso_ServicoDogWalker_InfoServDogWId",
                table: "Curso");

            migrationBuilder.DropIndex(
                name: "IX_Curso_InfoServDogWId",
                table: "Curso");

            migrationBuilder.DropColumn(
                name: "InfoServDogWId",
                table: "Curso");
        }
    }
}
