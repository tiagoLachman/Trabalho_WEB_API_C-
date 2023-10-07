using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Trabalho_WEB_API_C_.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Especialidades",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    nome = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Especialidades", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Planos",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    convenio = table.Column<string>(type: "TEXT", nullable: false),
                    desconto = table.Column<float>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Planos", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Medicos",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    nome = table.Column<string>(type: "TEXT", nullable: false),
                    email = table.Column<string>(type: "TEXT", nullable: false),
                    crm = table.Column<string>(type: "TEXT", nullable: false),
                    especialidadeid = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medicos", x => x.id);
                    table.ForeignKey(
                        name: "FK_Medicos_Especialidades_especialidadeid",
                        column: x => x.especialidadeid,
                        principalTable: "Especialidades",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pacientes",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    nome = table.Column<string>(type: "TEXT", nullable: false),
                    email = table.Column<string>(type: "TEXT", nullable: false),
                    cpf = table.Column<string>(type: "TEXT", nullable: false),
                    endereco = table.Column<string>(type: "TEXT", nullable: false),
                    planoid = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pacientes", x => x.id);
                    table.ForeignKey(
                        name: "FK_Pacientes_Planos_planoid",
                        column: x => x.planoid,
                        principalTable: "Planos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Agendamentos",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    medicoid = table.Column<int>(type: "INTEGER", nullable: false),
                    pacienteid = table.Column<int>(type: "INTEGER", nullable: false),
                    data = table.Column<DateTime>(type: "TEXT", nullable: false),
                    valor = table.Column<float>(type: "REAL", nullable: false),
                    cancelado = table.Column<bool>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Agendamentos", x => x.id);
                    table.ForeignKey(
                        name: "FK_Agendamentos_Medicos_medicoid",
                        column: x => x.medicoid,
                        principalTable: "Medicos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Agendamentos_Pacientes_pacienteid",
                        column: x => x.pacienteid,
                        principalTable: "Pacientes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Agendamentos_medicoid",
                table: "Agendamentos",
                column: "medicoid");

            migrationBuilder.CreateIndex(
                name: "IX_Agendamentos_pacienteid",
                table: "Agendamentos",
                column: "pacienteid");

            migrationBuilder.CreateIndex(
                name: "IX_Medicos_especialidadeid",
                table: "Medicos",
                column: "especialidadeid");

            migrationBuilder.CreateIndex(
                name: "IX_Pacientes_planoid",
                table: "Pacientes",
                column: "planoid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Agendamentos");

            migrationBuilder.DropTable(
                name: "Medicos");

            migrationBuilder.DropTable(
                name: "Pacientes");

            migrationBuilder.DropTable(
                name: "Especialidades");

            migrationBuilder.DropTable(
                name: "Planos");
        }
    }
}
