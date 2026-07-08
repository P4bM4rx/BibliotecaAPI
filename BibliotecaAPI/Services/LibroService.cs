using BibliotecaAPI.Data;
using BibliotecaAPI.DTOs;
using BibliotecaAPI.Interfaces;
using BibliotecaAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaAPI.Services;

public class LibroService : ILibroService
{
    private readonly BibliotecaContext Context;

    public LibroService(BibliotecaContext context)
    {
        Context = context;
    }

    public async Task<List<LibroDTO>> ObtenerTodosAsync()
    {
        return await Context.Libros
            .Include(l => l.Autor)
            .Select(l => new LibroDTO
            {
                Id = l.Id,
                Titulo = l.Titulo,
                Genero = l.Genero,
                NumeroPaginas = l.NumeroPaginas,
                Precio = l.Precio,
                Disponible = l.Disponible,
                FechaPublicacion = l.FechaPublicacion,
                AutorId = l.AutorId,
                Autor = l.Autor.Nombre
            })
            .ToListAsync();
    }

    public async Task<LibroDTO?> ObtenerPorIdAsync(int id)
    {
        Libro? libro = await Context.Libros
            .Include(l => l.Autor)
            .FirstOrDefaultAsync(l => l.Id == id);

        if (libro == null)
            return null;

        return new LibroDTO
        {
            Id = libro.Id,
            Titulo = libro.Titulo,
            Genero = libro.Genero,
            NumeroPaginas = libro.NumeroPaginas,
            Precio = libro.Precio,
            Disponible = libro.Disponible,
            FechaPublicacion = libro.FechaPublicacion,
            AutorId = libro.AutorId,
            Autor = libro.Autor.Nombre
        };
    }

    public async Task CrearAsync(LibroCreateDTO dto)
    {
        Libro libro = new()
        {
            Titulo = dto.Titulo,
            Genero = dto.Genero,
            NumeroPaginas = dto.NumeroPaginas,
            Precio = dto.Precio,
            Disponible = dto.Disponible,
            FechaPublicacion = dto.FechaPublicacion,
            AutorId = dto.AutorId
        };

        Context.Libros.Add(libro);

        await Context.SaveChangesAsync();
    }

    public async Task<bool> ActualizarAsync(int id, LibroUpdateDTO dto)
    {
        Libro? libro = await Context.Libros.FindAsync(id);

        if (libro == null)
            return false;

        libro.Titulo = dto.Titulo;
        libro.Genero = dto.Genero;
        libro.NumeroPaginas = dto.NumeroPaginas;
        libro.Precio = dto.Precio;
        libro.Disponible = dto.Disponible;
        libro.FechaPublicacion = dto.FechaPublicacion;
        libro.AutorId = dto.AutorId;

        await Context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> EliminarAsync(int id)
    {
        Libro? libro = await Context.Libros.FindAsync(id);

        if (libro == null)
            return false;

        Context.Libros.Remove(libro);

        await Context.SaveChangesAsync();

        return true;
    }

    public async Task<List<LibroDTO>> ObtenerPorGeneroAsync(string genero)
    {
        return await Context.Libros
            .Include(l => l.Autor)
            .Where(l => l.Genero == genero)
            .Select(l => new LibroDTO
            {
                Id = l.Id,
                Titulo = l.Titulo,
                Genero = l.Genero,
                NumeroPaginas = l.NumeroPaginas,
                Precio = l.Precio,
                Disponible = l.Disponible,
                FechaPublicacion = l.FechaPublicacion,
                AutorId = l.AutorId,
                Autor = l.Autor.Nombre
            })
            .ToListAsync();
    }

    public async Task<List<LibroDTO>> ObtenerPorAutorAsync(int autorId)
    {
        return await Context.Libros
            .Include(l => l.Autor)
            .Where(l => l.AutorId == autorId)
            .Select(l => new LibroDTO
            {
                Id = l.Id,
                Titulo = l.Titulo,
                Genero = l.Genero,
                NumeroPaginas = l.NumeroPaginas,
                Precio = l.Precio,
                Disponible = l.Disponible,
                FechaPublicacion = l.FechaPublicacion,
                AutorId = l.AutorId,
                Autor = l.Autor.Nombre
            })
            .ToListAsync();
    }

    public async Task<List<LibroDTO>> ObtenerDisponiblesAsync()
    {
        return await Context.Libros
            .Include(l => l.Autor)
            .Where(l => l.Disponible)
            .Select(l => new LibroDTO
            {
                Id = l.Id,
                Titulo = l.Titulo,
                Genero = l.Genero,
                NumeroPaginas = l.NumeroPaginas,
                Precio = l.Precio,
                Disponible = l.Disponible,
                FechaPublicacion = l.FechaPublicacion,
                AutorId = l.AutorId,
                Autor = l.Autor.Nombre
            })
            .ToListAsync();
    }

    public async Task<List<LibroDTO>> ObtenerNoDisponiblesAsync()
    {
        return await Context.Libros
            .Include(l => l.Autor)
            .Where(l => !l.Disponible)
            .Select(l => new LibroDTO
            {
                Id = l.Id,
                Titulo = l.Titulo,
                Genero = l.Genero,
                NumeroPaginas = l.NumeroPaginas,
                Precio = l.Precio,
                Disponible = l.Disponible,
                FechaPublicacion = l.FechaPublicacion,
                AutorId = l.AutorId,
                Autor = l.Autor.Nombre
            })
            .ToListAsync();
    }

    public async Task<List<LibroDTO>> ObtenerBaratosAsync()
    {
        return await Context.Libros
            .Include(l => l.Autor)
            .Where(l => l.Precio < 20)
            .Select(l => new LibroDTO
            {
                Id = l.Id,
                Titulo = l.Titulo,
                Genero = l.Genero,
                NumeroPaginas = l.NumeroPaginas,
                Precio = l.Precio,
                Disponible = l.Disponible,
                FechaPublicacion = l.FechaPublicacion,
                AutorId = l.AutorId,
                Autor = l.Autor.Nombre
            })
            .ToListAsync();
    }

    public async Task<List<LibroDTO>> ObtenerCarosAsync()
    {
        return await Context.Libros
            .Include(l => l.Autor)
            .Where(l => l.Precio > 50)
            .Select(l => new LibroDTO
            {
                Id = l.Id,
                Titulo = l.Titulo,
                Genero = l.Genero,
                NumeroPaginas = l.NumeroPaginas,
                Precio = l.Precio,
                Disponible = l.Disponible,
                FechaPublicacion = l.FechaPublicacion,
                AutorId = l.AutorId,
                Autor = l.Autor.Nombre
            })
            .ToListAsync();
    }

    public async Task<List<LibroDTO>> ObtenerPorRangoPrecioAsync(decimal min, decimal max)
    {
        return await Context.Libros
            .Include(l => l.Autor)
            .Where(l => l.Precio >= min && l.Precio <= max)
            .Select(l => new LibroDTO
            {
                Id = l.Id,
                Titulo = l.Titulo,
                Genero = l.Genero,
                NumeroPaginas = l.NumeroPaginas,
                Precio = l.Precio,
                Disponible = l.Disponible,
                FechaPublicacion = l.FechaPublicacion,
                AutorId = l.AutorId,
                Autor = l.Autor.Nombre
            })
            .ToListAsync();
    }

    public async Task<List<LibroDTO>> ObtenerPorNumeroPaginasAsync(int min, int max)
    {
        return await Context.Libros
            .Include(l => l.Autor)
            .Where(l => l.NumeroPaginas >= min &&
                        l.NumeroPaginas <= max)
            .Select(l => new LibroDTO
            {
                Id = l.Id,
                Titulo = l.Titulo,
                Genero = l.Genero,
                NumeroPaginas = l.NumeroPaginas,
                Precio = l.Precio,
                Disponible = l.Disponible,
                FechaPublicacion = l.FechaPublicacion,
                AutorId = l.AutorId,
                Autor = l.Autor.Nombre
            })
            .ToListAsync();
    }

    public async Task<List<LibroDTO>> ObtenerRecientesAsync()
    {
        DateTime fecha = DateTime.Today.AddYears(-5);

        return await Context.Libros
            .Include(l => l.Autor)
            .Where(l => l.FechaPublicacion >= fecha)
            .Select(l => new LibroDTO
            {
                Id = l.Id,
                Titulo = l.Titulo,
                Genero = l.Genero,
                NumeroPaginas = l.NumeroPaginas,
                Precio = l.Precio,
                Disponible = l.Disponible,
                FechaPublicacion = l.FechaPublicacion,
                AutorId = l.AutorId,
                Autor = l.Autor.Nombre
            })
            .ToListAsync();
    }

    public async Task<List<LibroDTO>> BuscarPorTituloAsync(string texto)
    {
        return await Context.Libros
            .Include(l => l.Autor)
            .Where(l => l.Titulo.Contains(texto))
            .Select(l => new LibroDTO
            {
                Id = l.Id,
                Titulo = l.Titulo,
                Genero = l.Genero,
                NumeroPaginas = l.NumeroPaginas,
                Precio = l.Precio,
                Disponible = l.Disponible,
                FechaPublicacion = l.FechaPublicacion,
                AutorId = l.AutorId,
                Autor = l.Autor.Nombre
            })
            .ToListAsync();
    }

    public async Task<LibroDTO?> ObtenerMasCaroAsync()
    {
        Libro? libro = await Context.Libros
            .Include(l => l.Autor)
            .OrderByDescending(l => l.Precio)
            .FirstOrDefaultAsync();

        if (libro == null)
            return null;

        return new LibroDTO
        {
            Id = libro.Id,
            Titulo = libro.Titulo,
            Genero = libro.Genero,
            NumeroPaginas = libro.NumeroPaginas,
            Precio = libro.Precio,
            Disponible = libro.Disponible,
            FechaPublicacion = libro.FechaPublicacion,
            AutorId = libro.AutorId,
            Autor = libro.Autor.Nombre
        };
    }

    public async Task<LibroDTO?> ObtenerMasBaratoAsync()
    {
        Libro? libro = await Context.Libros
            .Include(l => l.Autor)
            .OrderBy(l => l.Precio)
            .FirstOrDefaultAsync();

        if (libro == null)
            return null;

        return new LibroDTO
        {
            Id = libro.Id,
            Titulo = libro.Titulo,
            Genero = libro.Genero,
            NumeroPaginas = libro.NumeroPaginas,
            Precio = libro.Precio,
            Disponible = libro.Disponible,
            FechaPublicacion = libro.FechaPublicacion,
            AutorId = libro.AutorId,
            Autor = libro.Autor.Nombre
        };
    }

    public async Task<EstadisticasDTO> ObtenerEstadisticasAsync()
    {
        return new EstadisticasDTO
        {
            TotalLibros = await Context.Libros.CountAsync(),

            Disponibles = await Context.Libros
                .CountAsync(l => l.Disponible),

            Prestados = await Context.Libros
                .CountAsync(l => !l.Disponible),

            PrecioMedio = await Context.Libros
                .AverageAsync(l => l.Precio),

            PaginasMedias = await Context.Libros
                .AverageAsync(l => l.NumeroPaginas)
        };
    }

    public async Task<bool> PrestarAsync(int id)
    {
        Libro? libro = await Context.Libros.FindAsync(id);

        if (libro == null)
            return false;

        libro.Disponible = false;

        await Context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DevolverAsync(int id)
    {
        Libro? libro = await Context.Libros.FindAsync(id);

        if (libro == null)
            return false;

        libro.Disponible = true;

        await Context.SaveChangesAsync();

        return true;
    }
}