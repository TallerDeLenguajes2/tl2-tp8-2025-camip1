namespace tl2_tp8_2025_camip1.Interfaces;

public interface IAuthenticationService
{
    bool Login(string user, string pass);
    void Logout();
    bool isAuthenticated();

    // verifica si el usuario tiene el rol requerido
    bool HasAcesssLevel(string rol); 
}