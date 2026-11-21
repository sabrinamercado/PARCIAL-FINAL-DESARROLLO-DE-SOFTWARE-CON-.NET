-- Esquema de referencia. La aplicación ejecuta esto automáticamente al iniciar (LibroDAO.InicializarBaseDatos).
CREATE TABLE IF NOT EXISTS Libros (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Titulo TEXT NOT NULL,
    Autor TEXT NOT NULL,
    Anio INTEGER NOT NULL,
    Estado TEXT NOT NULL
);
