using tl2_tp8_2025_camip1.Interfaces;

namespace tl2_tp8_2025_camip1.Services;

public class AuthenticationService: IAuthenticationService
{
    private readonly IUserRepository _userRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthenticationService(IUserRepository repoUsuario, IHttpContextAccessor httpContextAccessor)
    {
        _userRepository = repoUsuario;
        _httpContextAccessor = httpContextAccessor;
    }

    public bool Login(string username, string password)
    {
        var context = _httpContextAccessor.HttpContext;
        var user = _userRepository.GetUser(username,password); //Busco usuario en la BD
        if (user != null)
        {
            if (context == null)
            {
                throw new InvalidOperationException("HttpContext no está disponible.");
            }
            //Guardo datos en la sesión
            context.Session.SetString("IsAuthenticated", "true");
            context.Session.SetString("User", user.User);
            context.Session.SetString("UserNombre", user.Nombre);
            context.Session.SetString("Rol", user.Rol);
            //es el tipo de acceso/rol admin o cliente
            return true;
        }
        return false;
    }
    public void Logout() //Salida del sistema, borra info del usuario guardadaen sesión
    {
        var context = _httpContextAccessor.HttpContext; //para acceder a la sesión
        if (context == null)
        {
           throw new InvalidOperationException("HttpContext no está disponible.");
        }
        /* context.Session.Remove("IsAuthenticated");
        context.Session.Remove("User");
        context.Session.Remove("UserNombre");
        context.Session.Remove("Rol");
        */
        context.Session.Clear();
    }

    public bool IsAuthenticated()
    {
        var context = _httpContextAccessor.HttpContext;
        if (context == null)
        {
            throw new InvalidOperationException("HttpContext no está disponible.");
        }
        return context.Session.GetString("IsAuthenticated") == "true";
 
    }
    
    // verifica si el usuario tiene el rol requerido
    public bool HasAccessLevel(string requiredAccessLevel)
    {
        var context = _httpContextAccessor.HttpContext;
        if (context == null)
        {
            throw new InvalidOperationException("HttpContext no está disponible.");
        }
        return context.Session.GetString("Rol") == requiredAccessLevel;
    }
}
