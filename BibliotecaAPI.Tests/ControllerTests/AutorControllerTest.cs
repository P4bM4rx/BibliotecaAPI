using System.Net;
using System.Net.Http.Json;
using BibliotecaAPI.DTOs;
using Microsoft.AspNetCore.Mvc.Testing;
using FluentAssertions;


namespace BibliotecaAPI.Tests;

public class AutorControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient Client;

    public AutorControllerTests(WebApplicationFactory<Program> factory)
    {
        Client = factory.CreateClient();
    }

    [Fact]
    public async Task ObtenerTodos_DevuelveOkConLaListaDeAutores()
    {
        var respuesta = await Client.GetAsync("api/Autor");

        respuesta.StatusCode.Should().Be(HttpStatusCode.OK);

        var autores = await respuesta.Content.ReadFromJsonAsync<List<AutorDTO>>();

        autores.Should().NotBeNull();
        autores!.Should().NotBeEmpty();
        autores.Should().Contain(a => a.Nombre == "J. K. Rowling");
    }

    [Fact]
    public async Task ObtenerPorId_ConIdExistente_DevuelveElAutorCorrecto()
    {
        var respuesta = await Client.GetAsync("api/Autor/1");

        respuesta.StatusCode.Should().Be(HttpStatusCode.OK);

        var autor = await respuesta.Content.ReadFromJsonAsync<AutorDTO>();

        autor.Should().NotBeNull();
        autor!.Id.Should().Be(1);
        autor.Nombre.Should().Be("J. K. Rowling");
        autor.Nacionalidad.Should().Be("Reino Unido");
    }

    [Fact]
    public async Task ObtenerPorId_ConIdInexistente_DevuelveNotFound()
    {
        var respuesta = await Client.GetAsync("api/Autor/999999");

        respuesta.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Crear_Actualizar_Eliminar_Autor_CicloCompletoFuncionaCorrectamente()
    {
        // Se usa un nombre único (GUID) para no chocar con los autores
        // ya existentes en la base de datos ni con otros tests.
        var nombreUnico = $"Autor Prueba {Guid.NewGuid()}";

        var nuevoAutor = new AutorCreateDTO
        {
            Nombre = nombreUnico,
            Nacionalidad = "PaisDePrueba",
            FechaNacimiento = new DateTime(1980, 1, 1)
        };

        // ---- POST: crear correctamente un autor ----
        var respuestaCrear = await Client.PostAsJsonAsync("api/Autor", nuevoAutor);
        respuestaCrear.StatusCode.Should().Be(HttpStatusCode.OK);

        // Comprobar que realmente existe en la base de datos.
        // El endpoint de creación no devuelve el Id, así que se busca
        // el autor recién creado por su nombre único.
        var listaTrasCrear = await Client.GetFromJsonAsync<List<AutorDTO>>("api/Autor");
        var autorCreado = listaTrasCrear!.SingleOrDefault(a => a.Nombre == nombreUnico);

        autorCreado.Should().NotBeNull();
        int id = autorCreado!.Id;

        // ---- PUT: modificar correctamente el autor ----
        var autorActualizado = new AutorUpdateDTO
        {
            Nombre = nombreUnico,
            Nacionalidad = "PaisModificado",
            FechaNacimiento = new DateTime(1985, 5, 5)
        };

        var respuestaActualizar = await Client.PutAsJsonAsync($"api/Autor/{id}", autorActualizado);
        respuestaActualizar.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Comprobar que la modificación se ha guardado.
        var autorTrasActualizar = await Client.GetFromJsonAsync<AutorDTO>($"api/Autor/{id}");

        autorTrasActualizar!.Nacionalidad.Should().Be("PaisModificado");
        autorTrasActualizar.FechaNacimiento.Should().Be(new DateTime(1985, 5, 5));

        // ---- DELETE: eliminar correctamente el autor ----
        var respuestaEliminar = await Client.DeleteAsync($"api/Autor/{id}");
        respuestaEliminar.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Comprobar que ya no existe en la base de datos.
        var respuestaTrasEliminar = await Client.GetAsync($"api/Autor/{id}");
        respuestaTrasEliminar.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Actualizar_ConIdInexistente_DevuelveNotFound()
    {
        var autorActualizado = new AutorUpdateDTO
        {
            Nombre = "No existe",
            Nacionalidad = "Ninguna",
            FechaNacimiento = new DateTime(2000, 1, 1)
        };

        var respuesta = await Client.PutAsJsonAsync("api/Autor/999999", autorActualizado);

        respuesta.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Eliminar_ConIdInexistente_DevuelveNotFound()
    {
        var respuesta = await Client.DeleteAsync("api/Autor/999999");

        respuesta.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task ObtenerPorNacionalidad_DevuelveSoloAutoresDeEsePais()
    {
        var respuesta = await Client.GetAsync("api/Autor/nacionalidad/Estados Unidos");

        respuesta.StatusCode.Should().Be(HttpStatusCode.OK);

        var autores = await respuesta.Content.ReadFromJsonAsync<List<AutorDTO>>();

        autores.Should().NotBeNull();
        autores!.Should().NotBeEmpty();
        autores.Should().OnlyContain(a => a.Nacionalidad == "Estados Unidos");
    }

    [Fact]
    public async Task ObtenerSinLibros_DevuelveSoloAutoresQueNoTienenLibros()
    {
        // Se crea un autor propio sin libros asociados, ya que todos los
        // autores de los datos semilla tienen libros.
        var nombreUnico = $"Autor Sin Libros {Guid.NewGuid()}";

        await Client.PostAsJsonAsync("api/Autor", new AutorCreateDTO
        {
            Nombre = nombreUnico,
            Nacionalidad = "PaisDePrueba",
            FechaNacimiento = new DateTime(1990, 1, 1)
        });

        var respuesta = await Client.GetAsync("api/Autor/sin-libros");

        respuesta.StatusCode.Should().Be(HttpStatusCode.OK);

        var autores = await respuesta.Content.ReadFromJsonAsync<List<AutorDTO>>();

        autores.Should().NotBeNull();
        autores!.Should().Contain(a => a.Nombre == nombreUnico);

        // Limpieza
        var autorCreado = autores.Single(a => a.Nombre == nombreUnico);
        await Client.DeleteAsync($"api/Autor/{autorCreado.Id}");
    }

    [Fact]
    public async Task ObtenerAutorConMasLibros_DevuelveUnAutorConLibrosAsociados()
    {
        var respuesta = await Client.GetAsync("api/Autor/mas-libros");

        respuesta.StatusCode.Should().Be(HttpStatusCode.OK);

        var autor = await respuesta.Content.ReadFromJsonAsync<AutorDTO>();

        autor.Should().NotBeNull();
        autor!.Libros.Should().NotBeEmpty();
    }
}