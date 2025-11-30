using Microsoft.AspNetCore.Mvc;
using tl2_tp8_2025_camip1.Interfaces;
using tl2_tp8_2025_camip1.ViewModels;
using tl2_tp8_2025_camip1.Services;

public class LoginController: Controller
{
    private readonly IAuthenticationService _authenticationService;

    public LoginController(IAuthenticationService authenticaacionService)
    {
        _authenticationService = authenticaacionService;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View(new LoginViewModel());
    }

    [HttpPost]
    public IActionResult Login(LoginViewModel model)
    {
        if(string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password))
        {
            model.ErrorMessage  = "Debe ingresar usuario y contraseña.";
            return View("Index", model);
        }

        if(_authenticationService.Login(model.Username, model.Password))
        {
            return RedirectToAction("Index", "Home");
        }

        model.ErrorMessage = "Credenciales inválidas";
        return View("Index", model);
    }

    [HttpGet]
    public IActionResult Logout()
    {
        _authenticationService.Logout();
        return RedirectToAction("Index");
    }
}