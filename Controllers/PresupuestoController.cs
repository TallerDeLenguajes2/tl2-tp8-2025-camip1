using Microsoft.AspNetCore.Mvc;
using tl2_tp8_2025_camip1.Models;
using tl2_tp8_2025_camip1.ViewModels;
using tl2_tp8_2025_camip1.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;

namespace tl2_tp8_2025_camip1.Controllers;

public class PresupuestoController : Controller
{
    private readonly IPresupuestoRepository _repoPresupuesto;
    
     // Se necesita el repositorio de Productos para llenar los Dropdowns
    private readonly IProductoRepository _repoProducto; 
    private IAuthenticationService _authService;
    public PresupuestoController(IPresupuestoRepository repoPresupuesto, IProductoRepository repoProducto, IAuthenticationService authService)
    {
        _repoPresupuesto = repoPresupuesto;
        _repoProducto = repoProducto;
        _authService = authService;
    }

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
        // Comprobación de si está logueado manual
        if (!_authService.IsAuthenticated())
        {
            return RedirectToAction("Index", "Login");
        }

        // Verifica Nivel de acceso que necesite validar
        if (_authService.HasAccessLevel("Administrador") ||
        _authService.HasAccessLevel("Cliente"))
        {
            //Si es admin o cliente entra
            List<Presupuesto> presupuestos = _repoPresupuesto.GetAll();
            return View(presupuestos);        
        }
        else
        {
            return RedirectToAction("Index", "Login");
        }
    }

    [HttpGet]
    public IActionResult Details(int id)
    {
        // Comprobación de si está logueado
        if (!_authService.IsAuthenticated())
        {
            return RedirectToAction("Index", "Login");
        }
        // Verifica Nivel de acceso que necesite validar
        if (_authService.HasAccessLevel("Administrador") ||
        _authService.HasAccessLevel("Cliente"))
        {
            var presupuesto = _repoPresupuesto.GetById(id);
            if (presupuesto == null)
            {
                return NotFound();
            }
            return View(presupuesto);        
        }
        else
        {
            return RedirectToAction("Index", "Login");
        }
    }
    
    [HttpGet]
    public IActionResult Create()
    {
        var securityCheck = CheckAdminPermissions();
        if (securityCheck != null) return securityCheck;
        // Se retorna un VM vacío para el formulario
        return View(new PresupuestoViewModel());
    }


    [HttpPost]
    public IActionResult Create(PresupuestoViewModel presupuestoVM)
    {
        // var securityCheck = CheckAdminPermissions();
        // if (securityCheck != null) return securityCheck;

        // VALIDACIÓN DE REGLA DE NEGOCIO ESPECÍFICA (Fecha no Futura)
        if (presupuestoVM.FechaCreacion > DateTime.Today)
        {
            // Se añade un error al ModelState
            ModelState.AddModelError("FechaCreacion", "La fecha de creación no puede ser una fecha futura.");
        }

        // //Control de seguridad del servidor
        if (!ModelState.IsValid)
        {
            return View(presupuestoVM);
        }

        //Mapeo manual de VM a Modelo de Dominio
        var nuevoPresupuesto = new Presupuesto
        {
            NombreDestinatario = presupuestoVM.NombreDestinatario,
            FechaCreacion = presupuestoVM.FechaCreacion
        };

        //Llamada al repositorio
        _repoPresupuesto.Create(nuevoPresupuesto);
        return RedirectToAction(nameof(Index));  
    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
        var securityCheck = CheckAdminPermissions();
        if (securityCheck != null) return securityCheck;

        var presupuesto = _repoPresupuesto.GetById(id);
        if (presupuesto == null) return NotFound();

        //Mapeo manual a VM desde Modelo de Dominio
        var presupuestoVM = new PresupuestoViewModel(presupuesto);

        return View(presupuestoVM);
    }

    [HttpPost]
    public IActionResult Edit(int id, PresupuestoViewModel presupuestoVM)
    {
        // var securityCheck = CheckAdminPermissions();
        // if (securityCheck != null) return securityCheck;

        if(id != presupuestoVM.IdPresupuesto) return NotFound();

        if (presupuestoVM.FechaCreacion > DateTime.Today)
        {
            ModelState.AddModelError("FechaCreacion", "La fecha de creación no puede ser una fecha futura.");
        }

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
        var securityCheck = CheckAdminPermissions();
        if (securityCheck != null) return securityCheck;
        
        var presupuesto = _repoPresupuesto.GetById(id);
        if (presupuesto == null)
        {
            return NotFound();
        }
        return View(presupuesto);
    }
    
    [HttpPost]
    public IActionResult DeleteConfirmed(int idPresupuesto)
    {
        // var securityCheck = CheckAdminPermissions();
        // if (securityCheck != null) return securityCheck;
        
//        if (_repoPresupuesto.GetById(idPresupuesto) == null) return NotFound();
        
        _repoPresupuesto.Delete(idPresupuesto);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult AgregarProducto(int id)
    {
        var securityCheck = CheckAdminPermissions();
        if (securityCheck != null) return securityCheck;
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
        // var securityCheck = CheckAdminPermissions();
        // if (securityCheck != null) return securityCheck;
        // // 1. Chequeo de Seguridad para la Cantidad
        if (!ModelState.IsValid)
        {
             // ❌ LÓGICA CRÍTICA DE RECARGA: Si la validación falla (ej. Cantidad < 1),
            // Muestra todos los errores en la Consola/Debug Output de Visual Studio
            // foreach (var modelStateKey in ModelState.Keys)
            // {
            //     var modelStateVal = ModelState[modelStateKey];
            //     foreach (var error in modelStateVal.Errors)
            //     {
            //         // Imprime el nombre del campo y el error de validación exacto.
            //         Console.WriteLine($"Error en el campo '{modelStateKey}': {error.ErrorMessage}");
            //     }
            // }
     
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


//Nueva Acción: Simplemente devuelve una vista estática con el mensaje.
    [HttpGet]
    public IActionResult AccesoDenegado()
    {
        // El usuario está logueado, pero no tiene el rol suficiente.
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    
}