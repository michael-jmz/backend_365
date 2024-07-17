using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiBanca.Migrations
{
    /// <inheritdoc />
    public partial class mssql_migration_418 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Usuario",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombres = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Apellidos = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Cedula = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    CodigoDactilar = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Celular = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "varchar(2048)", unicode: false, maxLength: 2048, nullable: false),
                    Provincia = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    FotoRostroURL = table.Column<string>(type: "varchar(2048)", unicode: false, maxLength: 2048, nullable: false),
                    SituacionLaboral = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Empresa = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    PaisPagoImpuestos = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    AceptoTerminosYConcidiones = table.Column<bool>(type: "bit", nullable: false),
                    CodigoVerificado = table.Column<bool>(type: "bit", nullable: false),
                    Inactivo = table.Column<bool>(type: "bit", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CodigoVerificacion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Codigo = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    FechaExpiracion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdUsuario = table.Column<int>(type: "int", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CodigoVerificacion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CodigoVerificacion_Usuario_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cuenta",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Numero = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Saldo = table.Column<double>(type: "float", nullable: false),
                    IdUsuario = table.Column<int>(type: "int", nullable: false),
                    Inactivo = table.Column<bool>(type: "bit", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cuenta", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cuenta_Usuario_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CodigoVerificacion_IdUsuario",
                table: "CodigoVerificacion",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_Cuenta_IdUsuario",
                table: "Cuenta",
                column: "IdUsuario");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CodigoVerificacion");

            migrationBuilder.DropTable(
                name: "Cuenta");

            migrationBuilder.DropTable(
                name: "Usuario");
        }
    }
}
