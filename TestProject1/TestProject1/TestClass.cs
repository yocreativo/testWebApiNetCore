using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using WebApplication1.Controllers;
using WebApplication1.Data;
using Xunit;

public class MarcasAutosControllerTests
{
    private readonly MarcasAutosController _controller;
    private readonly pruebaContext _context;

    public MarcasAutosControllerTests()
    {

        var options = new DbContextOptionsBuilder<pruebaContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _context = new pruebaContext(options);

        //Inicializa datos de prueba
        var existe = _context.marcasAutos.FirstOrDefaultAsync().GetAwaiter().GetResult();
        //Si en la BD en memoria esta vacia la tabla marcasAuto
        //Entonces inserto un registro
        if (existe == null)
        {
            _context.marcasAutos.Add(new MarcasAutos { IdMarca = 1, Marca = "Toyota" });
            _context.SaveChanges();
        }
        //Creo la instancia del context con la Base de Datos en Memoria
        _controller = new MarcasAutosController(_context);
 
       
    }

    [Fact]
    public async Task GetMarcaAuto_ReturnsNotFound_WhenMarcaDoesNotExist()
    {
        // Act
        var result = await _controller.GetMarcaAuto(5); // ID que no existe

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task GetMarcaAuto_ReturnsOkResult_WhenMarcaExists()
    {
        //Act
        //Se ejecuta el método con el
        //controller instanciado a un
        //entorno de prueba con base de Datos en Memoria
        var result = await _controller.GetMarcaAuto(1); // ID que existe en la BD en memoria

        // Se comprueba el tipo devuelto por el método GetMarcaAuto
        Assert.IsType<ActionResult<MarcasAutos>>(result);
        //Se comprueba si el valor devuelto en la propiedad IdMarca es igual al esperado, o sea, "1"
        Assert.Equal("1",result?.Value?.IdMarca.ToString());
    }
}

