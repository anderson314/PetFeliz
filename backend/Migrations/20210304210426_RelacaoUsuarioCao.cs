using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PetFelizApi.Migrations
{
    public partial class RelacaoUsuarioCao : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Usuario",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TipoConta = table.Column<int>(type: "int", nullable: false),
                    Nome = table.Column<string>(type: "varchar(40)", nullable: false),
                    dataNascimento = table.Column<DateTime>(type: "date", nullable: false),
                    FotoPerfil = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    Email = table.Column<string>(type: "varchar(50)", nullable: false),
                    Disponivel = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cao",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "varchar(15)", nullable: false),
                    Raca = table.Column<string>(type: "varchar(30)", nullable: true),
                    Idade = table.Column<int>(type: "int", nullable: false),
                    Porte = table.Column<int>(type: "int", nullable: false),
                    Peso = table.Column<int>(type: "varchar(2)", nullable: true),
                    ProprietarioId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cao_Usuario_ProprietarioId",
                        column: x => x.ProprietarioId,
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cao_ProprietarioId",
                table: "Cao",
                column: "ProprietarioId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cao");

            migrationBuilder.DropTable(
                name: "Usuario");
        }
    }
}
