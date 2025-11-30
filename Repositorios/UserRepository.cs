using Microsoft.Data.Sqlite;
using tl2_tp8_2025_camip1.Interfaces;
using tl2_tp8_2025_camip1.Models;

namespace tl2_tp8_2025_camip1.Repository;

public class UsuarioRepository: IUserRepository
{
    private readonly string cadenaConexion = "Data Source = DB/Tienda.db";
    public Usuario GetUser(string usuario, string contrasena)
    {
        Usuario user = null;

        const string sql = @"
                        SELECT id, nombre, user, pass, rol
                        FROM Usuario
                        WHERE user = @usuario AND pass = @contrasena";
        
        using var conexion = new SqliteConnection(cadenaConexion);
        conexion.Open();

        using var comando = new SqliteCommand(sql, conexion);
        
        // Se usan parámetros para prevenir inyección SQL
        comando.Parameters.AddWithValue(@usuario, usuario);
        comando.Parameters.AddWithValue(@contrasena, contrasena);

        using var reader = comando.ExecuteReader();

        if(reader.Read())
        {
            // Si el lector encuentra una fila, el usuario
            //existe y las credenciales son correctas

            user = new Usuario
            {
                Id = reader.GetInt32(0),
                Nombre = reader.GetString(1),
                User = reader.GetString(2),
                Pass = reader.GetString(3),
                Rol = reader.GetString(4)
            };
        }
        return user;
    }
}