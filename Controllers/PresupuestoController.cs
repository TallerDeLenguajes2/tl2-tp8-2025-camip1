using Microsoft.AspNetCore.Mvc;
using tl2_tp8_2025_camip1.Models;
using tl2_tp8_2025_camip1.Repository;

namespace tl2_tp8_2025_camip1.Controllers;

public class PresupuestoController : Controller
{
    private readonly PresupuestoRepository _repoPresupuesto;
    public PresupuestoController()
    {
        _repoPresupuesto = new PresupuestoRepository();
    }

    [HttpGet]
    public IActionResult Index()
    {
        List<Presupuesto> presupuestos = _repoPresupuesto.GetAll();
        return View(presupuestos);
    }

    [HttpGet]
    public IActionResult Details(int id)
    {
        var presupuesto = _repoPresupuesto.GetById(id);
        return View(presupuesto);
    }
    
    [HttpGet]
    public IActionResult Create()
    {
        return View(); //me lleva a la vista crear presupuesto con el formulario
    }

    [HttpPost]
    public IActionResult Create(Presupuesto nuevoPresupuesto)
    {
        _repoPresupuesto.Create(nuevoPresupuesto);
        return RedirectToAction("Index");  
    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
        var presupuesto = _repoPresupuesto.GetById(id);
        return View(presupuesto);
    }

    [HttpPost]
    public IActionResult Edit(Presupuesto presupuesto)
    {
        _repoPresupuesto.Update(presupuesto.IdPresupuesto, presupuesto);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Delete(int id)
    {
        var presupuesto = _repoPresupuesto.GetById(id);
        return View(presupuesto);
    }
    
    [HttpPost]
    public IActionResult DeleteConfirmed(int id)
    {
        if (_repoPresupuesto.GetById(id) == null) return NotFound();
        _repoPresupuesto.Delete(id);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult CreateDetalle(int idPresupuesto)
    {
        return View(idPresupuesto);
    }

    [HttpPost]
    public IActionResult CreateDetalle(int idPresupuesto, int idProducto, int cantidad)
    {
        _repoPresupuesto.CreateDetalle(idPresupuesto, idProducto, cantidad);
        return RedirectToAction("Index");
    }
}