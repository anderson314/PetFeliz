using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PetFelizApi.Migrations
{
    public partial class RelacaoServicoUsuario : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Servico",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Estado = table.Column<int>(type: "int", nullable: false),
                    DataSolicitacao = table.Column<DateTime>(type: "date", nullable: false),
                    HoraSolicitacao = table.Column<DateTime>(type: "time", nullable: false),
                    HoraInicio = table.Column<DateTime>(type: "time", nullable: true),
                    HoraTermino = table.Column<DateTime>(type: "time", nullable: true),
                    ValorTotal = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    LatitudeProp = table.Column<double>(type: "float", nullable: false),
                    LongitudeProp = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Servico", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UsuariosServico",
                columns: table => new
                {
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    ServicoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuariosServico", x => new { x.UsuarioId, x.ServicoId });
                    table.ForeignKey(
                        name: "FK_UsuariosServico_Servico_ServicoId",
                        column: x => x.ServicoId,
                        principalTable: "Servico",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsuariosServico_Usuario_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UsuariosServico_ServicoId",
                table: "UsuariosServico",
                column: "ServicoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UsuariosServico");

            migrationBuilder.DropTable(
                name: "Servico");
        }
    }
}
