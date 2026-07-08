using BibliotecaAPI.DTOs;

namespace BibliotecaAPI.Interfaces;

public interface IAutorService
{
    Task<List<AutorDTO>> ObtenerTodosAsync();

    Task<AutorDTO?> ObtenerPorIdAsync(int id);

    Task CrearAsync(AutorCreateDTO autor);

    Task<bool> ActualizarAsync(int id, AutorUpdateDTO autor);

    Task<bool> EliminarAsync(int id);

    Task<List<AutorDTO>> ObtenerPorNacionalidadAsync(string pais);

    Task<List<AutorDTO>> ObtenerSinLibrosAsync();

    Task<AutorDTO?> ObtenerAutorConMasLibrosAsync();
}