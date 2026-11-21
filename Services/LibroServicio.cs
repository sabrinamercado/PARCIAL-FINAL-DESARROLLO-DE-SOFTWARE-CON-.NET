using BibliotecaPersonal.Data;
using BibliotecaPersonal.Models;

namespace BibliotecaPersonal.Services;

/// <summary>Lógica del programa: validaciones y reglas. Usa el DAO para guardar.</summary>
public class LibroServicio
{
    private readonly LibroDAO _dao;

    public LibroServicio(LibroDAO dao) => _dao = dao;

    public List<Libro> Listar() => _dao.ObtenerTodos();

    public void Agregar(string titulo, string autor, string anioTexto, EstadoLectura estado)
    {
        var (t, a, anio) = Validar(titulo, autor, anioTexto);
        _dao.Insertar(new Libro(0, t, a, anio, estado));
    }

    public void Modificar(Libro libro, string titulo, string autor, string anioTexto, EstadoLectura estado)
    {
        var (t, a, anio) = Validar(titulo, autor, anioTexto);
        libro.Actualizar(t, a, anio, estado);
        _dao.Actualizar(libro);
    }

    public void Eliminar(Libro libro) => _dao.Eliminar(libro.Id);

    // Lanza ArgumentException con un mensaje comprensible si algo está mal
    private static (string, string, int) Validar(string titulo, string autor, string anioTexto)
    {
        if (string.IsNullOrWhiteSpace(titulo))
            throw new ArgumentException("El título es obligatorio.");
        if (string.IsNullOrWhiteSpace(autor))
            throw new ArgumentException("El autor es obligatorio.");
        if (!int.TryParse(anioTexto, out int anio))
            throw new ArgumentException("El año debe ser un número entero.");
        if (anio < 1000 || anio > DateTime.Now.Year)
            throw new ArgumentException($"El año debe estar entre 1000 y {DateTime.Now.Year}.");
        return (titulo.Trim(), autor.Trim(), anio);
    }
}
