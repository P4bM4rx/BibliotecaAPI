using BibliotecaAPI.DTOs;

namespace BibliotecaAPI.Interfaces;

public interface ILibroService
{
    Task<List<LibroDTO>> ObtenerTodosAsync();

    Task<LibroDTO?> ObtenerPorIdAsync(int id);

    Task CrearAsync(LibroCreateDTO libro);

    Task<bool> ActualizarAsync(int id, LibroUpdateDTO libro);

    Task<bool> EliminarAsync(int id);

    Task<List<LibroDTO>> ObtenerPorGeneroAsync(string genero);

    Task<List<LibroDTO>> ObtenerPorAutorAsync(int autorId);

    Task<List<LibroDTO>> ObtenerDisponiblesAsync();

    Task<List<LibroDTO>> ObtenerNoDisponiblesAsync();

    Task<List<LibroDTO>> ObtenerBaratosAsync();

    Task<List<LibroDTO>> ObtenerCarosAsync();

    Task<List<LibroDTO>> ObtenerPorRangoPrecioAsync(decimal min, decimal max);

    Task<List<LibroDTO>> ObtenerPorNumeroPaginasAsync(int min, int max);

    Task<List<LibroDTO>> ObtenerRecientesAsync();

    Task<List<LibroDTO>> BuscarPorTituloAsync(string texto);

    Task<EstadisticasDTO> ObtenerEstadisticasAsync();

    Task<LibroDTO?> ObtenerMasCaroAsync();

    Task<LibroDTO?> ObtenerMasBaratoAsync();

    Task<bool> PrestarAsync(int id);

    Task<bool> DevolverAsync(int id);
}