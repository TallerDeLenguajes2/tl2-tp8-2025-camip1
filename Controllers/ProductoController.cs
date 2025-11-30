using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp8_2025_camip1.Models;
using tl2_tp8_2025_camip1.Repository;
using tl2_tp8_2025_camip1.ViewModels;
using tl2_tp8_2025_camip1.Interfaces;

namespace tl2_tp8_2025_camip1.Controllers;

public class ProductoController : Controller
{
    //private readonly ILogger<HomeController> _logger;
    private IProductoRepository _productoRepository;
    public ProductoController(IProductoRepository repoProducto)
    {
        //  _logger = logger;
        _productoRepository = repoProducto;
    }
    //A partir de aquí van todos los Action Methods (Get, Post,etc.)

    [HttpGet]
    public IActionResult Index()
    {
        List<Producto> productos = _productoRepository.GetAll();
        return View(productos);
    }


    [HttpGet]
    public IActionResult Create()
    {
        return View(new ProductoViewModel()); 
    }

    [HttpPost]
    public IActionResult Create(ProductoViewModel productoVM)
    {
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
        var producto = _productoRepository.GetById(id);

        var productoVM = new ProductoViewModel
        {
            IdProducto = producto.IdProducto,
            Descripcion = producto.Descripcion,
            Precio = producto.Precio
        };

        return View(productoVM);
    }

    [HttpPost]
    public IActionResult Edit(int id, ProductoViewModel productoVM)
    {
        if(id != productoVM.IdProducto) return NotFound();

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
        var producto = _productoRepository.GetById(id);
        return View(producto);
    }
    
    [HttpPost]
    public IActionResult DeleteConfirmed(int idProducto)
    {
        if (_productoRepository.GetById(idProducto) == null) return NotFound();
        _productoRepository.Delete(idProducto);
        return RedirectToAction(nameof(Index));
    }

}
