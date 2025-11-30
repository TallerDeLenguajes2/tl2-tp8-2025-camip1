using Microsoft.AspNetCore.Mvc;
using tl2_tp8_2025_camip1.Models;
using tl2_tp8_2025_camip1.Repository;
using tl2_tp8_2025_camip1.ViewModels;
using tl2_tp8_2025_camip1.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace tl2_tp8_2025_camip1.Controllers;

public class PresupuestoController : Controller
{
    private readonly IPresupuestoRepository _repoPresupuesto;
    private readonly IProductoRepository _repoProducto; 
    public PresupuestoController(IPresupuestoRepository repoPresupuesto, IProductoRepository repoProducto)
    {
        _repoPresupuesto = repoPresupuesto;
        _repoProducto = repoProducto;
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
        return View(new PresupuestoViewModel()); //me lleva a la vista crear presupuesto con el formulario
    }

    [HttpPost]
    public IActionResult Create(PresupuestoViewModel presupuestoVM)
    {
        //Control de seguridad del servidor
        if (!ModelState.IsValid)
        {
            return View(presupuestoVM);
        }

        //Mapeo manual de VM a Modelo de Dominio
        var nuevoPresupuesto = new Presupuesto
        {
            NombreDestinatario = presupuestoVM.NombreDestinatario,
            FechaCreacion = DateTime.Now
        };

        //Llamada al repositorio
        _repoPresupuesto.Create(nuevoPresupuesto);
        return RedirectToAction(nameof(Index));  
    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
        var presupuesto = _repoPresupuesto.GetById(id);

        //Mapeo manual a VM desde Modelo de Dominio
        var presupuestoVM = new PresupuestoViewModel
        {
            IdPresupuesto = presupuesto.IdPresupuesto,
            NombreDestinatario = presupuesto.NombreDestinatario,
            FechaCreacion = presupuesto.FechaCreacion
        };
        return View(presupuestoVM);
    }

    [HttpPost]
    public IActionResult Edit(int id, PresupuestoViewModel presupuestoVM)
    {
        if(id != presupuestoVM.IdPresupuesto) return NotFound();

        //Chequeo de seguridad del servidor
        if (!ModelState.IsValid)
        {
            return View(presupuestoVM);
        }

        //Mapeo manual a Modelo de Dominio
        var presupuestoAEditar = new Presupuesto
        {
            IdPresupuesto = presupuestoVM.IdPresupuesto,
            NombreDestinatario = presupuestoVM.NombreDestinatario,
            FechaCreacion = presupuestoVM.FechaCreacion
        };

        _repoPresupuesto.Update(id, presupuestoAEditar);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Delete(int id)
    {
        var presupuesto = _repoPresupuesto.GetById(id);
        return View(presupuesto);
    }
    
    [HttpPost]
    public IActionResult DeleteConfirmed(int idPresupuesto)
    {
        if (_repoPresupuesto.GetById(idPresupuesto) == null) return NotFound();
        _repoPresupuesto.Delete(idPresupuesto);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult AgregarProducto(int id)
    {
        //Obtengo productos para el SelectList
        List<Producto> productos = _repoProducto.GetAll();
        
        //Creo el VM
        AgregarProductoViewModel model = new AgregarProductoViewModel
        {
            IdPresupuesto = id,  //Paso el ID del presupuesto actual

            //Creo el SelectList
            ListaProductos = new SelectList(productos, "IdProducto", "Descripcion")
        };
        return View(model);
    }

    [HttpPost]
    public IActionResult AgregarProducto(AgregarProductoViewModel model)
    {
        // 1. Chequeo de Seguridad para la Cantidad
        if (!ModelState.IsValid)
        {
        // LÓGICA CRÍTICA DE RECARGA: Si falla la validación,
        // debemos recargar el SelectList porque se pierde en el POST.
            var productos = _repoProducto.GetAll();
            model.ListaProductos = new SelectList(productos, "IdProducto", "Descripcion");

        // Devolvemos el modelo con los errores y el dropdown recargado
            return View(model);
        }

        // 2. Si es VÁLIDO: Llamamos al repositorio para guardar la relación
        _repoPresupuesto.AgregarProducto(model.IdPresupuesto, model.IdProducto, model.Cantidad);

        // 3. Redirigimos al detalle del presupuesto
        return RedirectToAction(nameof(Details), new { id = model.IdPresupuesto });
    }
}