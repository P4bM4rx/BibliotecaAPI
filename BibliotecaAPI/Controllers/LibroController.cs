using BibliotecaAPI.DTOs;
using BibliotecaAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LibroController : ControllerBase
{
    private readonly ILibroService Service;

    public LibroController(ILibroService service)
    {
        Service = service;
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerTodos()
        => Ok(await Service.ObtenerTodosAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        LibroDTO? libro =
            await Service.ObtenerPorIdAsync(id);

        if (libro == null)
            return NotFound();

        return Ok(libro);
    }

    [HttpPost]
    public async Task<IActionResult> Crear(LibroCreateDTO dto)
    {
        await Service.CrearAsync(dto);

        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Actualizar(int id, LibroUpdateDTO dto)
    {
        bool actualizado =
            await Service.ActualizarAsync(id, dto);

        if (!actualizado)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        bool eliminado =
            await Service.EliminarAsync(id);

        if (!eliminado)
            return NotFound();

        return NoContent();
    }

    [HttpGet("genero/{genero}")]
    public async Task<IActionResult> ObtenerGenero(string genero)
        => Ok(await Service.ObtenerPorGeneroAsync(genero));

    [HttpGet("autor/{id}")]
    public async Task<IActionResult> ObtenerAutor(int id)
        => Ok(await Service.ObtenerPorAutorAsync(id));

    [HttpGet("disponibles")]
    public async Task<IActionResult> Disponibles()
        => Ok(await Service.ObtenerDisponiblesAsync());

    [HttpGet("no-disponibles")]
    public async Task<IActionResult> NoDisponibles()
        => Ok(await Service.ObtenerNoDisponiblesAsync());

    [HttpGet("baratos")]
    public async Task<IActionResult> Baratos()
        => Ok(await Service.ObtenerBaratosAsync());

    [HttpGet("caros")]
    public async Task<IActionResult> Caros()
        => Ok(await Service.ObtenerCarosAsync());

    [HttpGet("precio")]
    public async Task<IActionResult> Precio(decimal min, decimal max)
        => Ok(await Service.ObtenerPorRangoPrecioAsync(min, max));

    [HttpGet("paginas")]
    public async Task<IActionResult> Paginas(int min, int max)
        => Ok(await Service.ObtenerPorNumeroPaginasAsync(min, max));

    [HttpGet("recientes")]
    public async Task<IActionResult> Recientes()
        => Ok(await Service.ObtenerRecientesAsync());

    [HttpGet("buscar/{texto}")]
    public async Task<IActionResult> Buscar(string texto)
        => Ok(await Service.BuscarPorTituloAsync(texto));

    [HttpGet("estadisticas")]
    public async Task<IActionResult> Estadisticas()
        => Ok(await Service.ObtenerEstadisticasAsync());

    [HttpGet("mas-caro")]
    public async Task<IActionResult> MasCaro()
        => Ok(await Service.ObtenerMasCaroAsync());

    [HttpGet("mas-barato")]
    public async Task<IActionResult> MasBarato()
        => Ok(await Service.ObtenerMasBaratoAsync());

    [HttpPatch("{id}/prestar")]
    public async Task<IActionResult> Prestar(int id)
    {
        bool correcto =
            await Service.PrestarAsync(id);

        if (!correcto)
            return NotFound();

        return NoContent();
    }

    [HttpPatch("{id}/devolver")]
    public async Task<IActionResult> Devolver(int id)
    {
        bool correcto =
            await Service.DevolverAsync(id);

        if (!correcto)
            return NotFound();

        return NoContent();
    }
}