using BibliotecaAPI.DTOs;
using BibliotecaAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AutorController : ControllerBase
{
    private readonly IAutorService Service;

    public AutorController(IAutorService service)
    {
        Service = service;
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerTodos()
    {
        return Ok(await Service.ObtenerTodosAsync());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        AutorDTO? autor = await Service.ObtenerPorIdAsync(id);

        if (autor == null)
            return NotFound();

        return Ok(autor);
    }

    [HttpPost]
    public async Task<IActionResult> Crear(AutorCreateDTO dto)
    {
        await Service.CrearAsync(dto);

        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Actualizar(int id, AutorUpdateDTO dto)
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

    [HttpGet("nacionalidad/{pais}")]
    public async Task<IActionResult> ObtenerPorNacionalidad(string pais)
    {
        return Ok(await Service.ObtenerPorNacionalidadAsync(pais));
    }

    [HttpGet("sin-libros")]
    public async Task<IActionResult> ObtenerSinLibros()
    {
        return Ok(await Service.ObtenerSinLibrosAsync());
    }

    [HttpGet("mas-libros")]
    public async Task<IActionResult> ObtenerAutorConMasLibros()
    {
        return Ok(await Service.ObtenerAutorConMasLibrosAsync());
    }
}