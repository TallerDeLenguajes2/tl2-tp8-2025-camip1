using Microsoft.Data.Sqlite;
using Microsoft.VisualBasic;
using tl2_tp8_2025_camip1.Models;

public class PresupuestoRepository
{
    private string cadenaConexion = "Data Source = DB/Tienda.db";

    //metodos

    //Listar todos los Presupuestos registrados. (devuelve un List de Presupuestos)
    public List<Presupuesto> GetAll()
    {
        List<Presupuesto> ListaPresupuestos = new List<Presupuesto>();

        string query = "SELECT * FROM Presupuesto";

        using var connection = new SqliteConnection(cadenaConexion);
        connection.Open();
        var command = new SqliteCommand(query, connection);

        using (SqliteDataReader reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                var presupuesto = new Presupuesto
                {
                    Id_presupuesto = Convert.ToInt32(reader["id_presupuesto"]),
                    Nombre_destinatario = reader["nombre_destinatario"].ToString(),
                    Fecha_creacion = Convert.ToDateTime(reader["fecha_creacion"]),
                    ListaDetalles = new List<PresupuestoDetalle>()
                };

                presupuesto.ListaDetalles = GetAllDetalles(presupuesto.Id_presupuesto, connection);
                ListaPresupuestos.Add(presupuesto);
            }
        }
        //connection.Close();

        return ListaPresupuestos;
    }

    //Obtener detalles del presupuesto
    private List<PresupuestoDetalle> GetAllDetalles(int id, SqliteConnection connection)
    {
        List<PresupuestoDetalle> ListaDetalles = new List<PresupuestoDetalle>();

        string query = "SELECT pd.id_producto, p.descripcion, p.precio, pd.cantidad FROM PresupuestoDetalle pd INNER JOIN Producto p ON p.id_producto = pd.id_Producto WHERE pd.id_presupuesto = @id_presupuesto";

        var command = new SqliteCommand(query, connection);
        command.Parameters.Add(new SqliteParameter("@id_producto", id));

        using (SqliteDataReader reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                var producto = new Producto
                {
                    IdProducto = Convert.ToInt32(reader["id_producto"]),
                    Descripcion = reader["descripcion"].ToString(),
                    Precio = Convert.ToDouble(reader["precio"])
                };

                var detalle = new PresupuestoDetalle
                {
                    Producto = producto,
                    Cantidad = Convert.ToInt32(reader["cantidad"])
                };

                ListaDetalles.Add(detalle);
            }

            return ListaDetalles;
        }
    }

    // Crear un nuevo Presupuesto. (recibe un objeto Presupuesto)
    public bool Create(Presupuesto presupuesto)
    {
        string queryString = "INSERT INTO Presupuesto (nombre_destinatario, fecha_creacion) VALUES (@nombre_destinatario, @fecha_creacion)";

        try
        {
            using var conexion = new SqliteConnection(cadenaConexion);
            conexion.Open();

            using var comando = new SqliteCommand(queryString, conexion);
            comando.Parameters.Add(new SqliteParameter("@nombre_destinatario", presupuesto.Nombre_destinatario));
            comando.Parameters.Add(new SqliteParameter("@fecha_creacion", presupuesto.Fecha_creacion));

            comando.ExecuteNonQuery();

            //conexion.Close();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al insertar presupuesto: {ex.Message}");
            return false;
        }
    }

    // Obtener detalles de un Presupuesto por su ID. (recibe un Id y devuelve un Presupuesto con sus productos y cantidades)
    public Presupuesto GetById(int id)
    {
        string query = "SELECT id_presupuesto, nombre_destinatario, fecha_creacion FROM Presupuesto WHERE id_presupuesto = @id_presupuesto";

        using var conexion = new SqliteConnection(cadenaConexion);
        conexion.Open();

        using var comando = new SqliteCommand(query, conexion);
        comando.Parameters.Add(new SqliteParameter("@id_presupuesto", id));

        using var lector = comando.ExecuteReader();

        Presupuesto presupuesto = null;

        if (lector.Read())
        {
            presupuesto = new Presupuesto
            {
                Id_presupuesto = Convert.ToInt32(lector["id_presupuesto"]),
                Nombre_destinatario = lector["nombre_destinatario"].ToString(),
                Fecha_creacion = Convert.ToDateTime(lector["fecha_creacion"]),
                ListaDetalles = new List<PresupuestoDetalle>()
            };

            presupuesto.ListaDetalles = GetAllDetalles(presupuesto.Id_presupuesto, conexion);
        }

        return presupuesto; // devuelve null si no lo encontró
    }

    //Agregar un producto y una cantidad a un presupuesto (recibe un Id)
    // public bool agregarProducto(int id)
    // {
    //     string query = "INSERT INTO Presupuesto ()";
    //     return true;
    // }

    //Eliminar un Presupuesto por ID
    public bool Delete(int id)
    {
        string query = "DELETE FROM Presupuesto WHERE id_presupuesto = @id_presupuesto";

        using var conexion = new SqliteConnection(cadenaConexion);
        conexion.Open();

        using var comando = new SqliteCommand(query, conexion);
        comando.Parameters.Add(new SqliteParameter("@id_presupuesto", id));

        int filasAfectadas = comando.ExecuteNonQuery();

        return filasAfectadas > 0;
    }

}