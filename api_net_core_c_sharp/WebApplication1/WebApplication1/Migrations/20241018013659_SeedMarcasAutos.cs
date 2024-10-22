using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    public partial class SeedMarcasAutos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "marcasAutos",
                columns: new[] { "IdMarca", "Marca" },
                values: new object[,]
                {
                    { 1, "Toyota" },
                    { 2, "Ford" },
                    { 3, "Honda" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "marcasAutos",
                keyColumn: "IdMarca",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "marcasAutos",
                keyColumn: "IdMarca",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "marcasAutos",
                keyColumn: "IdMarca",
                keyValue: 3);
        }
    }
}
