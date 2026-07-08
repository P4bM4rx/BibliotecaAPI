namespace BibliotecaAPI.DTOs;

public class AutorDTO
{
    public int Id { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public string Nacionalidad { get; set; } = string.Empty;

    public DateTime FechaNacimiento { get; set; }

    public List<LibroResumenDTO> Libros { get; set; } = [];
}