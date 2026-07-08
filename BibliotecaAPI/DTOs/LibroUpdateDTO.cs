namespace BibliotecaAPI.DTOs;

public class LibroUpdateDTO
{
    public string Titulo { get; set; } = string.Empty;

    public string Genero { get; set; } = string.Empty;

    public int NumeroPaginas { get; set; }

    public decimal Precio { get; set; }

    public bool Disponible { get; set; }

    public DateTime FechaPublicacion { get; set; }

    public int AutorId { get; set; }
}