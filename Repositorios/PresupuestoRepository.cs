using Microsoft.Data.Sqlite;
using Microsoft.VisualBasic;
using tl2_tp8_2025_camip1.Models;

namespace tl2_tp8_2025_camip1.Repository;
public class PresupuestoRepository
{
    private readonly string cadenaConexion = "Data Source = DB/Tienda.db";

    public List<Presupuesto> GetAll()
    {
        List<Presupuesto> ListaPresupuestos = new List<Presupuesto>();

        string query = @"SELECT * FROM Presupuesto";

        using var connection = new SqliteConnection(cadenaConexion);
        connection.Open();

        using var command = new SqliteCommand(query, connection);

        using (SqliteDataReader reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                var presupuesto = new Presupuesto
                {
                    IdPresupuesto = Convert.ToInt32(reader["id_presupuesto"]),
                    NombreDestinatario = reader["nombre_destinatario"].ToString(),
                    FechaCreacion = DateOnly.Parse(Convert.ToString(reader["fecha_creacion"])),
                    ListaDetalles = new List<PresupuestoDetalle>()
                };

                ListaPresupuestos.Add(presupuesto);
            }
        }
        foreach (var presupuesto in ListaPresupuestos)
        {
            presupuesto.ListaDetalles = GetAllDetalles(presupuesto.IdPresupuesto, connection);
        }

        connection.Close();
        return ListaPresupuestos;
    }

    //Obtener detalles del presupuesto
    private List<PresupuestoDetalle> GetAllDetalles(int id, SqliteConnection connection)
    {
        List<PresupuestoDetalle> ListaDetalles = new List<PresupuestoDetalle>();

        string query = @"SELECT pd.id_producto, p.descripcion, p.precio, pd.cantidad 
                        FROM PresupuestoDetalle pd 
                        INNER JOIN Producto p ON p.id_producto = pd.id_producto 
                        WHERE pd.id_presupuesto = @id_presupuesto";

        var command = new SqliteCommand(query, connection);
        command.Parameters.Add(new SqliteParameter("@id_presupuesto", id));

        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            var producto = new Producto
            {
                IdProducto = Convert.ToInt32(reader["id_producto"]),
                Descripcion = reader["descripcion"].ToString(),
                Precio = Convert.ToInt32(reader["precio"])
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


    public bool Create(Presupuesto presupuesto)
    {
        using var conexion = new SqliteConnection(cadenaConexion);
        conexion.Open();

        string sql = @"
            INSERT INTO Presupuesto (nombre_destinatario, fecha_creacion)
            VALUES (@nombre_destinatario, @fecha_creacion);
            SELECT last_insert_rowid();
        ";

        using var comando = new SqliteCommand(sql, conexion);
        comando.Parameters.Add(new SqliteParameter("@nombre_destinatario", presupuesto.NombreDestinatario));
        comando.Parameters.Add(new SqliteParameter("@fecha_creacion", presupuesto.FechaCreacion));

        long idPresupuesto = (long)comando.ExecuteScalar();
        presupuesto.IdPresupuesto = (int)idPresupuesto;

        // Inserto los detalles si existen
        if (presupuesto.ListaDetalles != null)
        {
            foreach (var detalle in presupuesto.ListaDetalles)
            {
                bool agregado = CreateDetalle(presupuesto.IdPresupuesto, detalle);

                if (!agregado)
                {
                    Delete(presupuesto.IdPresupuesto);
                    return false;
                }
            }
        }

        return true;
    }

    public Presupuesto GetById(int id)
    {
        string query = @"SELECT * FROM Presupuesto AS p
                 LEFT JOIN PresupuestoDetalle AS d ON p.id_presupuesto = d.id_presupuesto
                 LEFT JOIN Producto AS prod ON d.id_producto = prod.id_producto 
                 WHERE p.id_presupuesto = @id_presupuesto";

        using var conexion = new SqliteConnection(cadenaConexion);
        conexion.Open();

        using var comando = new SqliteCommand(query, conexion);
        comando.Parameters.Add(new SqliteParameter("@id_presupuesto", id));

        using var lector = comando.ExecuteReader();

        Presupuesto presupuesto;
        
        if (lector.Read())
        {
            presupuesto = new Presupuesto
            {
                IdPresupuesto = Convert.ToInt32(lector["id_presupuesto"]),
                NombreDestinatario = lector["nombre_destinatario"].ToString(),
                FechaCreacion = DateOnly.Parse(Convert.ToString(lector["fecha_creacion"])),
                ListaDetalles = new List<PresupuestoDetalle>()
            };
        }
        else
        {
            conexion.Close();
            return null;
        }

        presupuesto.ListaDetalles = GetAllDetalles(id, conexion);
        conexion.Close();
        return presupuesto;
    }

    public bool CreateDetalle(int idPresupuesto, PresupuestoDetalle detalle)
    {
        var productoRepo = new ProductoRepository();

        // Verifico que el producto exista antes de insertar
        if (!productoRepo.ExisteProducto(detalle.Producto))
        {
            return false;
        }

        using var conexion = new SqliteConnection(cadenaConexion);
        conexion.Open();

        string sql = @"
                        INSERT INTO PresupuestoDetalle (id_presupuesto, id_producto, cantidad) 
                        VALUES (@id_presupuesto, @id_producto, @cantidad)";

        using var comando = new SqliteCommand(sql, conexion);

        comando.Parameters.Add(new SqliteParameter("@id_presupuesto", idPresupuesto));
        comando.Parameters.Add(new SqliteParameter("@id_producto", detalle.Producto.IdProducto));
        comando.Parameters.Add(new SqliteParameter("@cantidad", detalle.Cantidad));

        int filasAfectadas = comando.ExecuteNonQuery();
        conexion.Close();
        return filasAfectadas > 0;
    }

    public bool Delete(int id)
    {
        //si no tengo metodo de borrado cascada debo eliminar primero los detalles que referencian 
        //al presupuesto a eliminar 
        string query = "DELETE FROM Presupuesto WHERE id_presupuesto = @id_presupuesto";

        using var conexion = new SqliteConnection(cadenaConexion);
        conexion.Open();

        using var comando = new SqliteCommand(query, conexion);
        comando.Parameters.Add(new SqliteParameter("@id_presupuesto", id));

        int filasAfectadas = comando.ExecuteNonQuery();

        conexion.Close();
        return filasAfectadas > 0;
    }

}