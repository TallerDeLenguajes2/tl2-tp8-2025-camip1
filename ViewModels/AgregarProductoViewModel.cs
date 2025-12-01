using System.ComponentModel.DataAnnotations; //Incluye atributos de validación integrados
using Microsoft.AspNetCore.Mvc.Rendering; //Necesario para SelectList

namespace tl2_tp8_2025_camip1.ViewModels
{
    public class AgregarProductoViewModel
    {
        // ID del presupuesto al que se va a agregar (viene de la URL o campo oculto)
        public int IdPresupuesto { get; set; }
        
        // ID del producto seleccionado en el dropdown
        [Display(Name = "Producto a agregar")]
        [Required(ErrorMessage = "Debe seleccionar un producto.")]
        public int IdProducto { get; set; }

        [Display(Name = "Cantidad")]
        [Required(ErrorMessage = "Cantidad requerida")]
        [Range(1, int.MaxValue, ErrorMessage = "Cantidad ingresada invalida")]
        public int Cantidad { get; set; } 

         // IGNORAR ESTA PROPIEDAD EN EL POST
        // Este SelectList solo se usa para renderizar el Dropdown en la vista GET.
        //  [BindNever]
        public SelectList ListaProductos { get; set; } // Propiedad adicional sin validacion , el controlador carga aquí los productos para el dropdown de la vista.
    }
}