using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp8_2025_camip1.Models;
using tl2_tp8_2025_camip1.Repository;
using tl2_tp8_2025_camip1.ViewModels;
using tl2_tp8_2025_camip1.Interfaces;

namespace tl2_tp8_2025_camip1.Controllers;

public class ProductoController : Controller
{
    private IProductoRepository _productoRepository;
    private IAuthenticationService _authService;
    public ProductoController(IProductoRepository repoProducto, IAuthenticationService authService)
    {
        _productoRepository = repoProducto;
        _authService = authService;
    }
    //A partir de aquí van todos los Action Methods (Get, Post,etc.)

    private IActionResult CheckAdminPermissions()
    {
        // 1. No logueado? -> vuelve al login
        if (!_authService.IsAuthenticated())
        {
            return RedirectToAction("Index", "Login");
        }

        // 2. No es Administrador? -> Da Error
        if (!_authService.HasAccessLevel("Administrador"))
        {
        // Llamamos a AccesoDenegado (llama a la vista correspondiente de Productos)
            return RedirectToAction(nameof(AccesoDenegado));
        }
        return null; // Permiso concedido
    }

    [HttpGet]
    public IActionResult Index()
    {
        var securityCheck = CheckAdminPermissions();
        if(securityCheck != null) return securityCheck;

        List<Producto> productos = _productoRepository.GetAll();
        return View(productos);
    }

    // public IActionResult Details(int id)
    // {
    //     // Aplicamos el chequeo de seguridad
    //     var securityCheck = CheckAdminPermissions();
    //     if (securityCheck != null) return securityCheck;

    //     Producto producto = _repo.GetById(id);
    //     if (producto == null) return NotFound();
    //     return View(producto);
    // }

    [HttpGet]
    public IActionResult Create()
    {
        var securityCheck = CheckAdminPermissions();
        if(securityCheck != null) return securityCheck;

        return View(); 
    }

    [HttpPost]
    public IActionResult Create(ProductoViewModel productoVM)
    {
        // Aplicamos el chequeo de seguridad
        // var securityCheck = CheckAdminPermissions();
        // if(securityCheck != null) return securityCheck;

        //CHEQUEO DE SEGURIDAD DEL SERVIDOR
        if(!ModelState.IsValid)
        {
            //Si falla: devuelvo el ViewModel con los datos y errores a la Vista
            return View(productoVM);
        }
        //Si es válido: Mapeo manual de VM a Modelo de Dominio
        var nuevoProducto = new Producto
        {
            Descripcion = productoVM.Descripcion,
            Precio = productoVM.Precio            
        };

        //LLAMADA AL REPOSITORIO
        _productoRepository.Create(nuevoProducto);
        return RedirectToAction(nameof(Index));  //redirige a la vista index de producto      
    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
        var securityCheck = CheckAdminPermissions();
        if(securityCheck != null) return securityCheck;

        var producto = _productoRepository.GetById(id);

        ProductoViewModel productovm = new ProductoViewModel(producto);

        if (producto == null)
        {
            return NotFound();
        }
        return View(productovm);
    }

    [HttpPost]
    public IActionResult Edit(int id, ProductoViewModel productoVM)
    {
        // var securityCheck = CheckAdminPermissions();
        // if(securityCheck != null) return securityCheck;

        if(id != productoVM.IdProducto) return NotFound();

        //CHEQUEO DE SEGURIDAD
        if (!ModelState.IsValid)
        {
            return View(productoVM);
        }

        var productoAEditar = new Producto
        {
            IdProducto = productoVM.IdProducto, //Necesario para el UPDATE
            Descripcion = productoVM.Descripcion,
            Precio = productoVM.Precio
        };

        _productoRepository.Update(productoAEditar);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Delete(int id)
    {
        var securityCheck = CheckAdminPermissions();
        if(securityCheck != null) return securityCheck;

        var producto = _productoRepository.GetById(id);
        if (producto == null) return NotFound();
        return View(producto);
    }
    
    [HttpPost]
    public IActionResult DeleteConfirmed(int idProducto)
    {
        // var securityCheck = CheckAdminPermissions();
        // if(securityCheck != null) return securityCheck;

//        if (_productoRepository.GetById(idProducto) == null) return NotFound();
        _productoRepository.Delete(idProducto);
        return RedirectToAction(nameof(Index));
    }

    public IActionResult AccesoDenegado()
    {
        return View(); // El usuario está logueado, pero no tiene el rol suficiente.
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

}
