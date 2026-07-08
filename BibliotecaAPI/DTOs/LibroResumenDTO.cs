namespace BibliotecaAPI.DTOs;

public class LibroResumenDTO
{
    public int Id { get; set; }

    public string Titulo { get; set; } = string.Empty;

    public decimal Precio { get; set; }

    public bool Disponible { get; set; }
}