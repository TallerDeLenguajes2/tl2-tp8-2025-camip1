using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp8_2025_camip1.Models;

namespace tl2_tp8_2025_camip1.Controllers;

public class ProductoController : Controller
{
    private ProductoRepository _productoRepository;
    public ProductoController()
    {
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
        var producto = new Producto();
        return View(producto);
    }
    // [HttpGet]
    // public IActionResult GetAll()
    // {
    //     var productos = _productoRepository.GetAll();
    //     return Ok(productos);
    // }

    // [HttpPost]
    // public IActionResult Create(Producto producto)
    // {
    //     bool realizado = _productoRepository.Create(producto);
    //     if (realizado)
    //     {
    //         return Created($"producto/{producto.IdProducto}", producto);
    //     }
    //     return BadRequest();
    // }

    // [HttpPut("{id}")]
    // public IActionResult Update(int id, Producto producto)
    // {
    //     bool realizado = _productoRepository.Update(id, producto);
    //     if (realizado)
    //     {
    //         return Ok(producto);
    //     }
    //     return NotFound("No se encontro un producto con ese id");
    // }

    // [HttpGet("{id}")]
    // public IActionResult GetById(int id)
    // {
    //     var productoEncontrado = _productoRepository.GetById(id);
    //     if (productoEncontrado != null)
    //     {
    //         return Ok(productoEncontrado);
    //     }
    //     return NotFound("No se encontro un producto con el id ingresado");
    // }

    // [HttpDelete]
    // public IActionResult Delete(int id)
    // {
    //     bool realizado = _productoRepository.Delete(id);
    //     if (realizado)
    //     {
    //         return NoContent();
    //     }
    //     else
    //     {
    //         return NotFound($"No se encontró el producto con ID {id} para eliminar.");
    //     }
    // }
}
