using Microsoft.EntityFrameworkCore.Migrations;

namespace PetFelizApi.Migrations
{
    public partial class ApagarRelacionCurso : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Curso_Usuario_DogWalkerId",
                table: "Curso");

            migrationBuilder.DropIndex(
                name: "IX_Curso_DogWalkerId",
                table: "Curso");

            migrationBuilder.DropColumn(
                name: "DogWalkerId",
                table: "Curso");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DogWalkerId",
                table: "Curso",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Curso_DogWalkerId",
                table: "Curso",
                column: "DogWalkerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Curso_Usuario_DogWalkerId",
                table: "Curso",
                column: "DogWalkerId",
                principalTable: "Usuario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
