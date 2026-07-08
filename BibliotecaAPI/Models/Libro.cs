namespace BibliotecaAPI.Models;

public class Libro
{
    public int Id { get; set; }

    public string Titulo { get; set; } = string.Empty;

    public string Genero { get; set; } = string.Empty;

    public int NumeroPaginas { get; set; }

    public decimal Precio { get; set; }

    public bool Disponible { get; set; }

    public DateTime FechaPublicacion { get; set; }

    // Clave foránea

    public int AutorId { get; set; }

    // Navegación

    public Autor Autor { get; set; } = null!;
}