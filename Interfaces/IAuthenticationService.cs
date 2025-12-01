namespace tl2_tp8_2025_camip1.Interfaces;
public interface IAuthenticationService
{
    // Valida las credenciales del usuario y, si son correctas, crea la sesión.
        // Retorna true si el login fue exitoso, false si falló.
    bool Login(string username, string password);

    // Cierra la sesión actual y limpia los datos del usuario.
    void Logout();

    // Verifica si hay un usuario logueado actualmente.
    // Se usa en los controladores para proteger rutas (si devuelve false -> redirigir a Login).
    bool IsAuthenticated();

    // Verifica si el usuario actual tiene un Rol específico.
    // Se usa para proteger acciones críticas (ej: Crear/Borrar productos).
    bool HasAccessLevel(string rol); // verifica si el usuario tiene el rol requerido
}