using BibliotecaPersonal.Models;
using Microsoft.Data.Sqlite;

namespace BibliotecaPersonal.Data;

/// <summary>Acceso a datos con ADO.NET (Microsoft.Data.Sqlite). Aquí están las 4 operaciones CRUD.</summary>
public class LibroDAO
{
    private readonly string _cadenaConexion;

    public LibroDAO(string rutaBaseDatos)
    {
        _cadenaConexion = $"Data Source={rutaBaseDatos}";
        InicializarBaseDatos();
    }

    // Crea la base de datos y la tabla automáticamente si no existen
    private void InicializarBaseDatos()
    {
        using var conexion = new SqliteConnection(_cadenaConexion);
        conexion.Open();
        using var comando = conexion.CreateCommand();
        comando.CommandText = @"
            CREATE TABLE IF NOT EXISTS Libros (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Titulo TEXT NOT NULL,
                Autor TEXT NOT NULL,
                Anio INTEGER NOT NULL,
                Estado TEXT NOT NULL
            );";
        comando.ExecuteNonQuery();
    }

    // CREATE
    public void Insertar(Libro libro)
    {
        using var conexion = new SqliteConnection(_cadenaConexion);
        conexion.Open();
        using var comando = conexion.CreateCommand();
        comando.CommandText =
            "INSERT INTO Libros (Titulo, Autor, Anio, Estado) VALUES ($titulo, $autor, $anio, $estado)";
        comando.Parameters.AddWithValue("$titulo", libro.Titulo);
        comando.Parameters.AddWithValue("$autor", libro.Autor);
        comando.Parameters.AddWithValue("$anio", libro.Anio);
        comando.Parameters.AddWithValue("$estado", libro.Estado.ToString());
        comando.ExecuteNonQuery();
    }

    // READ
    public List<Libro> ObtenerTodos()
    {
        var lista = new List<Libro>();
        using var conexion = new SqliteConnection(_cadenaConexion);
        conexion.Open();
        using var comando = conexion.CreateCommand();
        comando.CommandText = "SELECT Id, Titulo, Autor, Anio, Estado FROM Libros ORDER BY Titulo";
        using var lector = comando.ExecuteReader();
        while (lector.Read())
        {
            lista.Add(new Libro(
                lector.GetInt32(0),
                lector.GetString(1),
                lector.GetString(2),
                lector.GetInt32(3),
                Enum.Parse<EstadoLectura>(lector.GetString(4))));
        }
        return lista;
    }

    // UPDATE
    public void Actualizar(Libro libro)
    {
        using var conexion = new SqliteConnection(_cadenaConexion);
        conexion.Open();
        using var comando = conexion.CreateCommand();
        comando.CommandText =
            "UPDATE Libros SET Titulo=$titulo, Autor=$autor, Anio=$anio, Estado=$estado WHERE Id=$id";
        comando.Parameters.AddWithValue("$titulo", libro.Titulo);
        comando.Parameters.AddWithValue("$autor", libro.Autor);
        comando.Parameters.AddWithValue("$anio", libro.Anio);
        comando.Parameters.AddWithValue("$estado", libro.Estado.ToString());
        comando.Parameters.AddWithValue("$id", libro.Id);
        comando.ExecuteNonQuery();
    }

    // DELETE
    public void Eliminar(int id)
    {
        using var conexion = new SqliteConnection(_cadenaConexion);
        conexion.Open();
        using var comando = conexion.CreateCommand();
        comando.CommandText = "DELETE FROM Libros WHERE Id=$id";
        comando.Parameters.AddWithValue("$id", id);
        comando.ExecuteNonQuery();
    }
}
