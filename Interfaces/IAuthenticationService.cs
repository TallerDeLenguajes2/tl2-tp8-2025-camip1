namespace tl2_tp8_2025_camip1.Interfaces;

public interface IAuthenticationService
{
    bool Login(string username, string password);
    void Logout();
    bool IsAuthenticated();

    // verifica si el usuario tiene el rol requerido
    bool HasAccessLevel(string rol); 
}