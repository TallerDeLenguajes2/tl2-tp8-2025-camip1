using Microsoft.Data.Sqlite;
using tl2_tp8_2025_camip1.Interfaces;
using tl2_tp8_2025_camip1.Models;


namespace tl2_tp8_2025_camip1.Repository;
public class ProductoRepository: IProductoRepository
{
    private readonly string cadenaConexion = "Data Source=./DB/Tienda.db";

   // Constructor Síncrono (sin parámetros, necesario para la instanciación directa en el Controller)
    public ProductoRepository()
    {
    }


    //Listar todos los Productos registrados. (devuelve un List de Producto)
    public List<Producto> GetAll()
    {
        List<Producto> ListaProductos = new List<Producto>();

        string query = @"SELECT * FROM producto";

        using var connection = new SqliteConnection(cadenaConexion);

        connection.Open();

        using var comando = new SqliteCommand(query, connection);
        // ExecuteReader se usa para consultas que devuelven filas (SELECT)
        using var reader = comando.ExecuteReader();
    
        while (reader.Read())
        {
            var producto = new Producto
            {
                IdProducto = Convert.ToInt32(reader["id_producto"]),
                Descripcion = reader["descripcion"].ToString(),
                Precio = Convert.ToDecimal(reader["precio"])
            };
            ListaProductos.Add(producto);
        }
        //connection.Close();
        return ListaProductos;
    }

    public void Create(Producto prod)
    {
        string queryString = @"INSERT INTO Producto (descripcion, precio) VALUES (@descripcion, @precio)";

        using var conexion = new SqliteConnection(cadenaConexion);
        conexion.Open();

        using var comando = new SqliteCommand(queryString, conexion);

        comando.Parameters.Add(new SqliteParameter("@descripcion", prod.Descripcion));
        comando.Parameters.Add(new SqliteParameter("@precio", prod.Precio));

        comando.ExecuteNonQuery();

        //conexion.Close();
    }

    //Modificar un Producto existente. (recibe un Id y un objeto Producto)
    public void Update(Producto prodModificado)
    {
        string queryString = @"UPDATE Producto SET descripcion = @descripcion, precio = @precio WHERE id_producto = @id_producto";

        using var conexion = new SqliteConnection(cadenaConexion);
        conexion.Open();

        using var comando = new SqliteCommand(queryString, conexion);
        comando.Parameters.Add(new SqliteParameter("@descripcion", prodModificado.Descripcion));
        comando.Parameters.Add(new SqliteParameter("@precio", prodModificado.Precio));
        comando.Parameters.Add(new SqliteParameter("@id_producto", prodModificado.IdProducto));

        comando.ExecuteNonQuery();

        //conexion.Close();
    }

    
    // Obtiene un producto por su ID
    public Producto GetById(int id)
    {
        string query = @"SELECT * FROM Producto WHERE id_producto = @id_producto";

        using var conexion = new SqliteConnection(cadenaConexion);
        conexion.Open();

        using var comando = new SqliteCommand(query, conexion);
        comando.Parameters.Add(new SqliteParameter("@id_producto", id));

        using var lector = comando.ExecuteReader();

        Producto producto = null;

        if (lector.Read())
        {
            producto = new Producto
            {
                IdProducto = Convert.ToInt32(lector["id_producto"]),
                Descripcion = lector["descripcion"].ToString(),
                Precio = Convert.ToDecimal(lector["precio"])
            };
        }
        //conexion.Close();
        return producto; // devuelve null si no lo encontró
    }

    public void Delete(int id)
    {
        string query = @"DELETE FROM Producto WHERE id_producto = @id_producto";

        using var conexion = new SqliteConnection(cadenaConexion);
        conexion.Open();

        using var comando = new SqliteCommand(query, conexion);
        comando.Parameters.Add(new SqliteParameter("@id_producto", id));

        //int filasAfectadas = comando.ExecuteNonQuery();
        comando.ExecuteNonQuery();

        //conexion.Close();
        //return filasAfectadas > 0;
    }
}