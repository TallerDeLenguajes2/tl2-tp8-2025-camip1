using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp8_2025_camip1.Models;
using tl2_tp8_2025_camip1.Repository;

namespace tl2_tp8_2025_camip1.Controllers;

public class ProductoController : Controller
{
    //private readonly ILogger<HomeController> _logger;
    private ProductoRepository _productoRepository;
    public ProductoController()
    {
        //  _logger = logger;
        _productoRepository = new ProductoRepository();
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
        return View(); //me lleva a la vista crear producto con el formulario
    }

    [HttpPost]
    public IActionResult Create(Producto nuevoProducto)
    {
        _productoRepository.Create(nuevoProducto);
        return RedirectToAction("Index");  //redirige a la vista index de producto      
    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
        var producto = _productoRepository.GetById(id);
        return View(producto);
    }

    [HttpPost]
    public IActionResult Edit(Producto producto)
    {
        _productoRepository.Update(producto.IdProducto, producto);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Delete(int id)
    {
        var producto = _productoRepository.GetById(id);
        return View(producto);
    }
    
    [HttpPost]
    public IActionResult DeleteConfirmed(int id)
    {
        if (_productoRepository.GetById(id) == null) return NotFound();
        _productoRepository.Delete(id);
        return RedirectToAction("Index");
    }

    // [HttpGet]
    // public IActionResult Details(int id)
    // {
    //     var producto = _productoRepository.GetById(id);
    //     if (producto == null) return NotFound();

    //     return View(producto);
    // }
}
