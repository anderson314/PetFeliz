using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PetFelizApi.Migrations
{
    public partial class RelacaoDogWalkerInfoServico : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "dataNascimento",
                table: "Usuario",
                newName: "DataNascimento");

            migrationBuilder.AddColumn<DateTime>(
                name: "DataCadastro",
                table: "Usuario",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "WhatsApp",
                table: "Usuario",
                type: "char(11)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Peso",
                table: "Cao",
                type: "varchar(2)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int(2)");

            migrationBuilder.CreateTable(
                name: "ServicoDogWalker",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DogWalkerId = table.Column<int>(type: "int", nullable: false),
                    AvaliacaoMedia = table.Column<decimal>(type: "decimal(1,1)", nullable: true),
                    Sobre = table.Column<string>(type: "varchar(600)", nullable: true),
                    Preferencias = table.Column<string>(type: "varchar(250)", nullable: true),
                    ValorServico = table.Column<decimal>(type: "decimal(3,2)", nullable: true),
                    AceitaCartao = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServicoDogWalker", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServicoDogWalker_Usuario_DogWalkerId",
                        column: x => x.DogWalkerId,
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ServicoDogWalker_DogWalkerId",
                table: "ServicoDogWalker",
                column: "DogWalkerId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ServicoDogWalker");

            migrationBuilder.DropColumn(
                name: "DataCadastro",
                table: "Usuario");

            migrationBuilder.DropColumn(
                name: "WhatsApp",
                table: "Usuario");

            migrationBuilder.RenameColumn(
                name: "DataNascimento",
                table: "Usuario",
                newName: "dataNascimento");

            migrationBuilder.AlterColumn<int>(
                name: "Peso",
                table: "Cao",
                type: "int(2)",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
