using BibliotecaAPI.Data;
using BibliotecaAPI.DTOs;
using BibliotecaAPI.Interfaces;
using BibliotecaAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaAPI.Services;

public class AutorService : IAutorService
{
    private readonly BibliotecaContext Context;

    public AutorService(BibliotecaContext context)
    {
        Context = context;
    }

    public async Task<List<AutorDTO>> ObtenerTodosAsync()
    {
        return await Context.Autores
            .Include(autor => autor.Libros)
            .Select(autor => new AutorDTO
            {
                Id = autor.Id,
                Nombre = autor.Nombre,
                Nacionalidad = autor.Nacionalidad,
                FechaNacimiento = autor.FechaNacimiento,

                Libros = autor.Libros
                    .Select(libro => new LibroResumenDTO
                    {
                        Id = libro.Id,
                        Titulo = libro.Titulo,
                        Precio = libro.Precio,
                        Disponible = libro.Disponible
                    })
                    .ToList()
            })
            .ToListAsync();
    }

    public async Task<AutorDTO?> ObtenerPorIdAsync(int id)
    {
        Autor? autor = await Context.Autores
            .Include(a => a.Libros)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (autor == null)
        {
            return null;
        }

        return new AutorDTO
        {
            Id = autor.Id,
            Nombre = autor.Nombre,
            Nacionalidad = autor.Nacionalidad,
            FechaNacimiento = autor.FechaNacimiento,

            Libros = autor.Libros
                .Select(libro => new LibroResumenDTO
                {
                    Id = libro.Id,
                    Titulo = libro.Titulo,
                    Precio = libro.Precio,
                    Disponible = libro.Disponible
                })
                .ToList()
        };
    }

    public async Task CrearAsync(AutorCreateDTO autorDTO)
    {
        Autor autor = new()
        {
            Nombre = autorDTO.Nombre,
            Nacionalidad = autorDTO.Nacionalidad,
            FechaNacimiento = autorDTO.FechaNacimiento
        };

        Context.Autores.Add(autor);

        await Context.SaveChangesAsync();
    }

    public async Task<bool> ActualizarAsync(int id, AutorUpdateDTO autorDTO)
    {
        Autor? autor = await Context.Autores.FindAsync(id);

        if (autor == null)
        {
            return false;
        }

        autor.Nombre = autorDTO.Nombre;
        autor.Nacionalidad = autorDTO.Nacionalidad;
        autor.FechaNacimiento = autorDTO.FechaNacimiento;

        await Context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> EliminarAsync(int id)
    {
        Autor? autor = await Context.Autores.FindAsync(id);

        if (autor == null)
        {
            return false;
        }

        Context.Autores.Remove(autor);

        await Context.SaveChangesAsync();

        return true;
    }

    public async Task<List<AutorDTO>> ObtenerPorNacionalidadAsync(string pais)
    {
        return await Context.Autores
            .Where(a => a.Nacionalidad == pais)
            .Select(a => new AutorDTO
            {
                Id = a.Id,
                Nombre = a.Nombre,
                Nacionalidad = a.Nacionalidad,
                FechaNacimiento = a.FechaNacimiento
            })
            .ToListAsync();
    }

    public async Task<List<AutorDTO>> ObtenerSinLibrosAsync()
    {
        return await Context.Autores
            .Where(a => !a.Libros.Any())
            .Select(a => new AutorDTO
            {
                Id = a.Id,
                Nombre = a.Nombre,
                Nacionalidad = a.Nacionalidad,
                FechaNacimiento = a.FechaNacimiento
            })
            .ToListAsync();
    }

    public async Task<AutorDTO?> ObtenerAutorConMasLibrosAsync()
    {
        Autor? autor = await Context.Autores
            .Include(a => a.Libros)
            .OrderByDescending(a => a.Libros.Count)
            .FirstOrDefaultAsync();

        if (autor == null)
        {
            return null;
        }

        return new AutorDTO
        {
            Id = autor.Id,
            Nombre = autor.Nombre,
            Nacionalidad = autor.Nacionalidad,
            FechaNacimiento = autor.FechaNacimiento,

            Libros = autor.Libros
                .Select(libro => new LibroResumenDTO
                {
                    Id = libro.Id,
                    Titulo = libro.Titulo,
                    Precio = libro.Precio,
                    Disponible = libro.Disponible
                })
                .ToList()
        };
    }
}