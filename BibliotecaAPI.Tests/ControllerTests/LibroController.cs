using System.Net;
using System.Net.Http.Json;
using BibliotecaAPI.DTOs;
using Microsoft.AspNetCore.Mvc.Testing;
using FluentAssertions;


namespace BibliotecaAPI.Tests;

public class LibroControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient Client;

    public LibroControllerTests(WebApplicationFactory<Program> factory)
    {
        Client = factory.CreateClient();
    }

    [Fact]
    public async Task ObtenerTodos_DevuelveOkConLaListaDeLibros()
    {
        var respuesta = await Client.GetAsync("api/Libro");

        respuesta.StatusCode.Should().Be(HttpStatusCode.OK);

        var libros = await respuesta.Content.ReadFromJsonAsync<List<LibroDTO>>();

        libros.Should().NotBeNull();
        libros!.Should().NotBeEmpty();
        libros.Should().Contain(l => l.Titulo == "Harry Potter y la piedra filosofal");
    }

    [Fact]
    public async Task ObtenerPorId_ConIdExistente_DevuelveElLibroCorrecto()
    {
        var respuesta = await Client.GetAsync("api/Libro/1");

        respuesta.StatusCode.Should().Be(HttpStatusCode.OK);

        var libro = await respuesta.Content.ReadFromJsonAsync<LibroDTO>();

        libro.Should().NotBeNull();
        libro!.Id.Should().Be(1);
        libro.Titulo.Should().Be("Harry Potter y la piedra filosofal");
        libro.Autor.Should().Be("J. K. Rowling");
    }

    [Fact]
    public async Task ObtenerPorId_ConIdInexistente_DevuelveNotFound()
    {
        var respuesta = await Client.GetAsync("api/Libro/999999");

        respuesta.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Crear_Actualizar_Eliminar_Libro_CicloCompletoFuncionaCorrectamente()
    {
        var tituloUnico = $"Libro de Prueba {Guid.NewGuid()}";

        var nuevoLibro = new LibroCreateDTO
        {
            Titulo = tituloUnico,
            Genero = "PruebaIntegracion",
            NumeroPaginas = 100,
            Precio = 15.00m,
            Disponible = true,
            FechaPublicacion = new DateTime(2020, 1, 1),
            AutorId = 1 // J. K. Rowling, ya existente
        };

        // ---- POST: crear correctamente un libro ----
        var respuestaCrear = await Client.PostAsJsonAsync("api/Libro", nuevoLibro);
        respuestaCrear.StatusCode.Should().Be(HttpStatusCode.OK);

        // Comprobar que realmente existe en la base de datos.
        var listaTrasCrear = await Client.GetFromJsonAsync<List<LibroDTO>>("api/Libro");
        var libroCreado = listaTrasCrear!.SingleOrDefault(l => l.Titulo == tituloUnico);

        libroCreado.Should().NotBeNull();
        int id = libroCreado!.Id;

        // ---- PUT: modificar correctamente el libro ----
        var libroActualizado = new LibroUpdateDTO
        {
            Titulo = tituloUnico,
            Genero = "PruebaIntegracion",
            NumeroPaginas = 200,
            Precio = 25.00m,
            Disponible = false,
            FechaPublicacion = new DateTime(2021, 6, 15),
            AutorId = 1
        };

        var respuestaActualizar = await Client.PutAsJsonAsync($"api/Libro/{id}", libroActualizado);
        respuestaActualizar.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Comprobar que la modificación se ha guardado.
        var libroTrasActualizar = await Client.GetFromJsonAsync<LibroDTO>($"api/Libro/{id}");

        libroTrasActualizar!.NumeroPaginas.Should().Be(200);
        libroTrasActualizar.Precio.Should().Be(25.00m);
        libroTrasActualizar.Disponible.Should().BeFalse();

        // ---- DELETE: eliminar correctamente el libro ----
        var respuestaEliminar = await Client.DeleteAsync($"api/Libro/{id}");
        respuestaEliminar.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Comprobar que ya no existe en la base de datos.
        var respuestaTrasEliminar = await Client.GetAsync($"api/Libro/{id}");
        respuestaTrasEliminar.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Actualizar_ConIdInexistente_DevuelveNotFound()
    {
        var libroActualizado = new LibroUpdateDTO
        {
            Titulo = "No existe",
            Genero = "Ninguno",
            NumeroPaginas = 1,
            Precio = 1,
            Disponible = true,
            FechaPublicacion = new DateTime(2000, 1, 1),
            AutorId = 1
        };

        var respuesta = await Client.PutAsJsonAsync("api/Libro/999999", libroActualizado);

        respuesta.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Eliminar_ConIdInexistente_DevuelveNotFound()
    {
        var respuesta = await Client.DeleteAsync("api/Libro/999999");

        respuesta.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Prestar_Y_Devolver_Libro_CambianElEstadoEnLaBaseDeDatos()
    {
        // Se crea un libro propio, disponible, para no alterar los datos semilla.
        var tituloUnico = $"Libro Prestamo {Guid.NewGuid()}";

        var nuevoLibro = new LibroCreateDTO
        {
            Titulo = tituloUnico,
            Genero = "PruebaIntegracion",
            NumeroPaginas = 150,
            Precio = 10.00m,
            Disponible = true,
            FechaPublicacion = new DateTime(2019, 3, 3),
            AutorId = 1
        };

        await Client.PostAsJsonAsync("api/Libro", nuevoLibro);

        var lista = await Client.GetFromJsonAsync<List<LibroDTO>>("api/Libro");
        var libroCreado = lista!.Single(l => l.Titulo == tituloUnico);
        int id = libroCreado.Id;

        libroCreado.Disponible.Should().BeTrue();

        // ---- PATCH: prestar el libro ----
        var respuestaPrestar = await Client.PatchAsync($"api/Libro/{id}/prestar", content: null);
        respuestaPrestar.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var libroTrasPrestar = await Client.GetFromJsonAsync<LibroDTO>($"api/Libro/{id}");
        libroTrasPrestar!.Disponible.Should().BeFalse();

        // ---- PATCH: devolver el libro ----
        var respuestaDevolver = await Client.PatchAsync($"api/Libro/{id}/devolver", content: null);
        respuestaDevolver.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var libroTrasDevolver = await Client.GetFromJsonAsync<LibroDTO>($"api/Libro/{id}");
        libroTrasDevolver!.Disponible.Should().BeTrue();

        // Limpieza: se elimina el libro creado para el test.
        await Client.DeleteAsync($"api/Libro/{id}");
    }

    [Fact]
    public async Task Prestar_ConIdInexistente_DevuelveNotFound()
    {
        var respuesta = await Client.PatchAsync("api/Libro/999999/prestar", content: null);

        respuesta.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Devolver_ConIdInexistente_DevuelveNotFound()
    {
        var respuesta = await Client.PatchAsync("api/Libro/999999/devolver", content: null);

        respuesta.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task ObtenerPorGenero_DevuelveSoloLibrosDeEseGenero()
    {
        var respuesta = await Client.GetAsync("api/Libro/genero/Fantasía");

        respuesta.StatusCode.Should().Be(HttpStatusCode.OK);

        var libros = await respuesta.Content.ReadFromJsonAsync<List<LibroDTO>>();

        libros.Should().NotBeNull();
        libros!.Should().NotBeEmpty();
        libros.Should().OnlyContain(l => l.Genero == "Fantasía");
    }

    [Fact]
    public async Task ObtenerPorAutor_DevuelveSoloLibrosDeEseAutor()
    {
        // AutorId = 1 -> J. K. Rowling (3 libros en los datos semilla)
        var respuesta = await Client.GetAsync("api/Libro/autor/1");

        respuesta.StatusCode.Should().Be(HttpStatusCode.OK);

        var libros = await respuesta.Content.ReadFromJsonAsync<List<LibroDTO>>();

        libros.Should().NotBeNull();
        libros!.Should().NotBeEmpty();
        libros.Should().OnlyContain(l => l.AutorId == 1);
    }

    [Fact]
    public async Task ObtenerDisponibles_DevuelveSoloLibrosDisponibles()
    {
        var respuesta = await Client.GetAsync("api/Libro/disponibles");

        respuesta.StatusCode.Should().Be(HttpStatusCode.OK);

        var libros = await respuesta.Content.ReadFromJsonAsync<List<LibroDTO>>();

        libros.Should().NotBeNull();
        libros!.Should().NotBeEmpty();
        libros.Should().OnlyContain(l => l.Disponible);
    }

    [Fact]
    public async Task ObtenerNoDisponibles_DevuelveSoloLibrosNoDisponibles()
    {
        var respuesta = await Client.GetAsync("api/Libro/no-disponibles");

        respuesta.StatusCode.Should().Be(HttpStatusCode.OK);

        var libros = await respuesta.Content.ReadFromJsonAsync<List<LibroDTO>>();

        libros.Should().NotBeNull();
        libros!.Should().NotBeEmpty();
        libros.Should().OnlyContain(l => !l.Disponible);
    }

    [Fact]
    public async Task ObtenerBaratos_DevuelveSoloLibrosConPrecioMenorA20()
    {
        var respuesta = await Client.GetAsync("api/Libro/baratos");

        respuesta.StatusCode.Should().Be(HttpStatusCode.OK);

        var libros = await respuesta.Content.ReadFromJsonAsync<List<LibroDTO>>();

        libros.Should().NotBeNull();
        libros!.Should().NotBeEmpty();
        libros.Should().OnlyContain(l => l.Precio < 20);
    }

    [Fact]
    public async Task ObtenerCaros_DevuelveSoloLibrosConPrecioMayorA50()
    {
        var respuesta = await Client.GetAsync("api/Libro/caros");

        respuesta.StatusCode.Should().Be(HttpStatusCode.OK);

        var libros = await respuesta.Content.ReadFromJsonAsync<List<LibroDTO>>();

        libros.Should().NotBeNull();
        libros!.Should().NotBeEmpty();
        libros.Should().OnlyContain(l => l.Precio > 50);
    }

    [Fact]
    public async Task BuscarPorTitulo_DevuelveLosLibrosQueContienenElTexto()
    {
        var respuesta = await Client.GetAsync("api/Libro/buscar/Harry");

        respuesta.StatusCode.Should().Be(HttpStatusCode.OK);

        var libros = await respuesta.Content.ReadFromJsonAsync<List<LibroDTO>>();

        libros.Should().NotBeNull();
        libros!.Should().HaveCount(3);
        libros.Should().OnlyContain(l => l.Titulo.Contains("Harry"));
    }

    [Fact]
    public async Task ObtenerEstadisticas_DevuelveDatosCoherentesConLosLibrosExistentes()
    {
        var respuestaTodos = await Client.GetAsync("api/Libro");
        var libros = await respuestaTodos.Content.ReadFromJsonAsync<List<LibroDTO>>();

        var respuestaEstadisticas = await Client.GetAsync("api/Libro/estadisticas");
        respuestaEstadisticas.StatusCode.Should().Be(HttpStatusCode.OK);

        var estadisticas = await respuestaEstadisticas.Content.ReadFromJsonAsync<EstadisticasDTO>();

        estadisticas.Should().NotBeNull();
        estadisticas!.TotalLibros.Should().Be(libros!.Count);
        estadisticas.Disponibles.Should().Be(libros.Count(l => l.Disponible));
        estadisticas.Prestados.Should().Be(libros.Count(l => !l.Disponible));
        (estadisticas.Disponibles + estadisticas.Prestados).Should().Be(estadisticas.TotalLibros);
        estadisticas.PrecioMedio.Should().BeGreaterThan(0);
        estadisticas.PaginasMedias.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task ObtenerPorRangoPrecio_DevuelveSoloLibrosDentroDelRango()
    {
        var respuesta = await Client.GetAsync("api/Libro/precio?min=20&max=30");

        respuesta.StatusCode.Should().Be(HttpStatusCode.OK);

        var libros = await respuesta.Content.ReadFromJsonAsync<List<LibroDTO>>();

        libros.Should().NotBeNull();
        libros!.Should().NotBeEmpty();
        libros.Should().OnlyContain(l => l.Precio >= 20 && l.Precio <= 30);
    }

    [Fact]
    public async Task ObtenerPorNumeroPaginas_DevuelveSoloLibrosDentroDelRango()
    {
        var respuesta = await Client.GetAsync("api/Libro/paginas?min=300&max=500");

        respuesta.StatusCode.Should().Be(HttpStatusCode.OK);

        var libros = await respuesta.Content.ReadFromJsonAsync<List<LibroDTO>>();

        libros.Should().NotBeNull();
        libros!.Should().NotBeEmpty();
        libros.Should().OnlyContain(l => l.NumeroPaginas >= 300 && l.NumeroPaginas <= 500);
    }

    [Fact]
    public async Task ObtenerRecientes_DevuelveSoloLibrosPublicadosEnLosUltimos5Anios()
    {
        var respuesta = await Client.GetAsync("api/Libro/recientes");

        respuesta.StatusCode.Should().Be(HttpStatusCode.OK);

        var libros = await respuesta.Content.ReadFromJsonAsync<List<LibroDTO>>();

        libros.Should().NotBeNull();

        var fechaLimite = DateTime.Today.AddYears(-5);
        libros!.Should().OnlyContain(l => l.FechaPublicacion >= fechaLimite);
    }

    [Fact]
    public async Task ObtenerMasCaro_DevuelveElLibroDeMayorPrecio()
    {
        var respuestaTodos = await Client.GetAsync("api/Libro");
        var libros = await respuestaTodos.Content.ReadFromJsonAsync<List<LibroDTO>>();
        var precioMaximo = libros!.Max(l => l.Precio);

        var respuesta = await Client.GetAsync("api/Libro/mas-caro");
        respuesta.StatusCode.Should().Be(HttpStatusCode.OK);

        var libroMasCaro = await respuesta.Content.ReadFromJsonAsync<LibroDTO>();

        libroMasCaro.Should().NotBeNull();
        libroMasCaro!.Precio.Should().Be(precioMaximo);
    }

    [Fact]
    public async Task ObtenerMasBarato_DevuelveElLibroDeMenorPrecio()
    {
        var respuestaTodos = await Client.GetAsync("api/Libro");
        var libros = await respuestaTodos.Content.ReadFromJsonAsync<List<LibroDTO>>();
        var precioMinimo = libros!.Min(l => l.Precio);

        var respuesta = await Client.GetAsync("api/Libro/mas-barato");
        respuesta.StatusCode.Should().Be(HttpStatusCode.OK);

        var libroMasBarato = await respuesta.Content.ReadFromJsonAsync<LibroDTO>();

        libroMasBarato.Should().NotBeNull();
        libroMasBarato!.Precio.Should().Be(precioMinimo);
    }
}