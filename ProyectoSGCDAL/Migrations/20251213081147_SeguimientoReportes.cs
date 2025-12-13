using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProyectoSGCDAL.Migrations
{
    public partial class SeguimientoReportes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HistorialGestiones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                              .Annotation("Sqlite:Autoincrement", true),
                    IdSolicitud = table.Column<int>(type: "INTEGER", nullable: false),
                    Fecha = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UsuarioId = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    Accion = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Comentarios = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistorialGestiones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HistorialGestiones_SolicitudesCredito_IdSolicitud",
                        column: x => x.IdSolicitud,
                        principalTable: "SolicitudesCredito",
                        principalColumn: "IdSolicitud",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HistorialGestiones_IdSolicitud",
                table: "HistorialGestiones",
                column: "IdSolicitud");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HistorialGestiones");
        }
    }
}
