using System.ComponentModel.DataAnnotations;
namespace tl2_tp8_2025_camip1.ViewModels;

public class LoginViewModel
{
    [Required(ErrorMessage = "El username es obligatorio.")]
    [Display(Name = "Nombre de Usuario")]
    public string Username {get; set;}

    [Required(ErrorMessage = "La password es obligatorio.")]
    [Display(Name = "Contraseña")]
    public string Password {get; set;}
    public string ErrorMessage {get; set;}
}