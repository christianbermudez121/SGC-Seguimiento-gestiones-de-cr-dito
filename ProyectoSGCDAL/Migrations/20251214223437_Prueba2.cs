using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProyectoSGCDAL.Migrations
{
    /// <inheritdoc />
    public partial class Prueba2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Accion",
                table: "HistorialGestiones",
                newName: "EstadoNuevo");

            migrationBuilder.AddColumn<string>(
                name: "EstadoAnterior",
                table: "HistorialGestiones",
                type: "TEXT",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EstadoAnterior",
                table: "HistorialGestiones");

            migrationBuilder.RenameColumn(
                name: "EstadoNuevo",
                table: "HistorialGestiones",
                newName: "Accion");
        }
    }
}
