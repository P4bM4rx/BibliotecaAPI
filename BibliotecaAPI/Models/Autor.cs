namespace BibliotecaAPI.Models;

public class Autor
{
    public int Id { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public string Nacionalidad { get; set; } = string.Empty;

    public DateTime FechaNacimiento { get; set; }

    // Navegación

    public List<Libro> Libros { get; set; } = new();
}