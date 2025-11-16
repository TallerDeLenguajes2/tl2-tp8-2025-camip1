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
                    FechaCreacion = Convert.ToDateTime(reader["fecha_creacion"]),
                    ListaDetalles = new List<PresupuestoDetalle>()
                };

                ListaPresupuestos.Add(presupuesto);
            }
        }
        // foreach (var presupuesto in ListaPresupuestos)
        // {
        //     presupuesto.ListaDetalles = GetAllDetalles(presupuesto.IdPresupuesto, connection);
        // }

        connection.Close();
        return ListaPresupuestos;
    }

    public void Create(Presupuesto presupuesto)
    {
        using var conexion = new SqliteConnection(cadenaConexion);
        conexion.Open();

        string sql = @"
                        INSERT INTO Presupuesto (nombre_destinatario, fecha_creacion) 
                        VALUES (@nombre_destinatario, @fecha_creacion)";

        using var comando = new SqliteCommand(sql, conexion);

        comando.Parameters.Add(new SqliteParameter("@nombre_destinatario", presupuesto.NombreDestinatario));
        comando.Parameters.Add(new SqliteParameter("@fecha_creacion", presupuesto.FechaCreacion));

        comando.ExecuteNonQuery();

        conexion.Close();
    }

    public Presupuesto GetById(int id)
    {
        
        using var conexion = new SqliteConnection(cadenaConexion);
        conexion.Open();

        string query = @"SELECT p.id_presupuesto, 
                                p.nombre_destinatario, 
                                p.fecha_creacion, 
                                prod.id_producto, 
                                prod.descripcion, 
                                prod.precio, 
                                d.cantidad
                        FROM Presupuesto AS p
                        LEFT JOIN PresupuestoDetalle AS d ON p.id_presupuesto = d.id_presupuesto
                        LEFT JOIN Producto AS prod ON d.id_producto = prod.id_producto 
                        WHERE p.id_presupuesto = @id_presupuesto";

        using var comando = new SqliteCommand(query, conexion);
        comando.Parameters.Add(new SqliteParameter("@id_presupuesto", id));

        using var lector = comando.ExecuteReader();

        Presupuesto presupuesto = null;
        
        while (lector.Read())
        {
            if(presupuesto == null)
            {
                presupuesto = new Presupuesto
                {
                    IdPresupuesto = Convert.ToInt32(lector["id_presupuesto"]),
                    NombreDestinatario = lector["nombre_destinatario"].ToString(),
                    FechaCreacion = Convert.ToDateTime(lector["fecha_creacion"]),
                    ListaDetalles = new List<PresupuestoDetalle>()
                };                
            }

            string idProductoString = lector["id_producto"].ToString();

            if (!string.IsNullOrEmpty(idProductoString))
            {
                var presupuestoDetalle = new PresupuestoDetalle
                {
                    Producto = new Producto
                    {
                        IdProducto = Convert.ToInt32(idProductoString),
                        Descripcion = lector["descripcion"].ToString(),
                        Precio = Convert.ToDecimal(lector["precio"])
                    },
                    Cantidad = Convert.ToInt32(lector["cantidad"])
                };
                presupuesto.ListaDetalles.Add(presupuestoDetalle);
            }   
        }
        conexion.Close();
        return presupuesto;
    }

    public void Update(int id, Presupuesto nuevoPresupuesto)
    {
        using var conexion = new SqliteConnection(cadenaConexion);
        conexion.Open();

        string sql = @"
                        UPDATE Presupuesto 
                        SET nombre_destinatario = @nombre_destinatario
                        WHERE id_presupuesto = @id_presupuesto";

        using var comando = new SqliteCommand(sql, conexion);

        comando.Parameters.Add(new SqliteParameter("@nombre_destinatario", nuevoPresupuesto.NombreDestinatario));
        comando.Parameters.Add(new SqliteParameter("@id_presupuesto", id));

        comando.ExecuteNonQuery();

        conexion.Close();
    }


    public void AgregarProducto(int idPresupuesto, int idProducto, int cantidad)
    {
        using var conexion = new SqliteConnection(cadenaConexion);
        conexion.Open();

        string sql = @"
                        INSERT INTO PresupuestoDetalle (id_presupuesto, id_producto, cantidad) 
                        VALUES (@id_presupuesto, @id_producto, @cantidad)";

        using var comando = new SqliteCommand(sql, conexion);

        comando.Parameters.Add(new SqliteParameter("@id_presupuesto", idPresupuesto));
        comando.Parameters.Add(new SqliteParameter("@id_producto", idProducto));
        comando.Parameters.Add(new SqliteParameter("@cantidad", cantidad));

        comando.ExecuteNonQuery();
        conexion.Close();
    }

    public void Delete(int id)
    {
        string query = "DELETE FROM Presupuesto WHERE id_presupuesto = @id_presupuesto";

        using var conexion = new SqliteConnection(cadenaConexion);
        conexion.Open();

        using var comando = new SqliteCommand(query, conexion);
        comando.Parameters.Add(new SqliteParameter("@id_presupuesto", id));

        comando.ExecuteNonQuery();
        conexion.Close();
    }
}

