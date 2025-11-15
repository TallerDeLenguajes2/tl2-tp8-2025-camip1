using Microsoft.AspNetCore.Mvc.Rendering;

namespace tl2_tp8_2025_camip1.ViewModels
{
    public class AgregarProductoViewModel
    {
        public int IdPresupuesto {get; set; }
        public int IdProducto {get; set; }
        public int Cantidad {get; set; } 
        public SelectList ListaProductos {get; set; } // Propiedad sin validacion, el controlador carga aquí los productos para el dropdown de la vista.

    }
}