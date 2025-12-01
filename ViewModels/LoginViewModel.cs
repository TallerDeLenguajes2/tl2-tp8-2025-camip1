using System.ComponentModel.DataAnnotations;
namespace tl2_tp8_2025_camip1.ViewModels;

public class LoginViewModel
{
    [Required]
    public string Username {get; set;}

    [Required, DataType(DataType.Password)]
    public string Password {get; set;}
    
    // Esta propiedad se usa para devolver feedback al usuario si el Login falla 
    // (ej: "Usuario o contraseña incorrectos").
    // El controlador llenará esta propiedad antes de retornar la vista.
    public string ErrorMessage {get; set;}

    // Puede usarse para ocultar/mostrar elementos en la vista de Login dependiendo
        // de si el usuario ya intentó loguearse o si ya tiene sesión activa.
    public bool IsAuthenticated { get; set; }
}