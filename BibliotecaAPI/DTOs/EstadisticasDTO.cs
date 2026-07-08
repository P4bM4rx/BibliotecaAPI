namespace BibliotecaAPI.DTOs;

public class EstadisticasDTO
{
    public int TotalLibros { get; set; }

    public int Disponibles { get; set; }

    public int Prestados { get; set; }

    public decimal PrecioMedio { get; set; }

    public double PaginasMedias { get; set; }
}