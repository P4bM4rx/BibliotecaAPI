using BibliotecaAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaAPI.Data;

public class BibliotecaContext : DbContext
{
    public BibliotecaContext(
        DbContextOptions<BibliotecaContext> options)
        : base(options)
    {
    }

    public DbSet<Autor> Autores => Set<Autor>();

    public DbSet<Libro> Libros => Set<Libro>();
}