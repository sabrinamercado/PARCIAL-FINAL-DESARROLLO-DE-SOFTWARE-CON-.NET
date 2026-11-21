using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using BibliotecaPersonal.Data;
using BibliotecaPersonal.Models;
using BibliotecaPersonal.Services;
using Microsoft.Data.Sqlite;

namespace BibliotecaPersonal.Views;

public partial class MainWindow : Window
{
    private readonly LibroServicio _servicio;
    private Libro? _seleccionado;

    public MainWindow()
    {
        InitializeComponent();

        // La base de datos queda junto al ejecutable y se crea sola la primera vez
        string ruta = Path.Combine(AppContext.BaseDirectory, "biblioteca.db");
        _servicio = new LibroServicio(new LibroDAO(ruta));

        CmbEstado.ItemsSource = Enum.GetValues<EstadoLectura>();
        CmbEstado.SelectedIndex = 0;
        CargarLista();
    }

    private void CargarLista()
    {
        LstLibros.ItemsSource = _servicio.Listar();
    }

    private void Mostrar(string texto, bool esError)
    {
        TxtMensaje.Text = texto;
        TxtMensaje.Foreground = esError ? Brushes.Firebrick : Brushes.SeaGreen;
    }

    private void LimpiarCampos()
    {
        TxtTitulo.Text = "";
        TxtAutor.Text = "";
        TxtAnio.Text = "";
        CmbEstado.SelectedIndex = 0;
        _seleccionado = null;
        LstLibros.SelectedItem = null;
    }

    private EstadoLectura EstadoElegido() => (EstadoLectura)(CmbEstado.SelectedItem ?? EstadoLectura.PorLeer);

    private void Guardar_Click(object? sender, RoutedEventArgs e)
    {
        try
        {
            _servicio.Agregar(TxtTitulo.Text ?? "", TxtAutor.Text ?? "", TxtAnio.Text ?? "", EstadoElegido());
            LimpiarCampos();
            CargarLista();
            Mostrar("✔ Libro guardado correctamente.", false);
        }
        catch (ArgumentException ex) { Mostrar("⚠ " + ex.Message, true); }
        catch (SqliteException ex) { Mostrar("⚠ Error de base de datos: " + ex.Message, true); }
    }

    private void Actualizar_Click(object? sender, RoutedEventArgs e)
    {
        if (_seleccionado == null)
        {
            Mostrar("⚠ Selecciona un libro de la lista para actualizar.", true);
            return;
        }
        try
        {
            _servicio.Modificar(_seleccionado, TxtTitulo.Text ?? "", TxtAutor.Text ?? "", TxtAnio.Text ?? "", EstadoElegido());
            LimpiarCampos();
            CargarLista();
            Mostrar("✔ Libro actualizado correctamente.", false);
        }
        catch (ArgumentException ex) { Mostrar("⚠ " + ex.Message, true); }
        catch (SqliteException ex) { Mostrar("⚠ Error de base de datos: " + ex.Message, true); }
    }

    private void Eliminar_Click(object? sender, RoutedEventArgs e)
    {
        if (_seleccionado == null)
        {
            Mostrar("⚠ Selecciona un libro de la lista para eliminar.", true);
            return;
        }
        try
        {
            _servicio.Eliminar(_seleccionado);
            LimpiarCampos();
            CargarLista();
            Mostrar("✔ Libro eliminado.", false);
        }
        catch (SqliteException ex) { Mostrar("⚠ Error de base de datos: " + ex.Message, true); }
    }

    private void Limpiar_Click(object? sender, RoutedEventArgs e)
    {
        LimpiarCampos();
        Mostrar("", false);
    }

    private void Lista_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (LstLibros.SelectedItem is Libro libro)
        {
            _seleccionado = libro;
            TxtTitulo.Text = libro.Titulo;
            TxtAutor.Text = libro.Autor;
            TxtAnio.Text = libro.Anio.ToString();
            CmbEstado.SelectedItem = libro.Estado;
        }
    }
}
