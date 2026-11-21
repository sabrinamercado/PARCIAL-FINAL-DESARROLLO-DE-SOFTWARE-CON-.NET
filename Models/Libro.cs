namespace BibliotecaPersonal.Models;

/// <summary>Representa un libro de la biblioteca personal (clase del dominio).</summary>
public class Libro
{
    // Encapsulamiento: las propiedades solo se modifican desde la propia clase
    public int Id { get; private set; }
    public string Titulo { get; private set; }
    public string Autor { get; private set; }
    public int Anio { get; private set; }
    public EstadoLectura Estado { get; private set; }

    public Libro(int id, string titulo, string autor, int anio, EstadoLectura estado)
    {
        Id = id;
        Titulo = titulo;
        Autor = autor;
        Anio = anio;
        Estado = estado;
    }

    public void Actualizar(string titulo, string autor, int anio, EstadoLectura estado)
    {
        Titulo = titulo;
        Autor = autor;
        Anio = anio;
        Estado = estado;
    }

    // Texto que se muestra en la lista de la interfaz
    public override string ToString() => $"{Titulo}  |  {Autor}  |  {Anio}  |  {Estado}";
}
