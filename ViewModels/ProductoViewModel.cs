using System.ComponentModel.DataAnnotations;
using tl2_tp8_2025_camip1.Models;

namespace tl2_tp8_2025_camip1.ViewModels
{
    public class ProductoViewModel
    {
        public ProductoViewModel()
        { 

        }

        public ProductoViewModel(Producto producto)
        {
            Descripcion = producto.Descripcion;
            IdProducto = producto.IdProducto;
            Precio = producto.Precio;
        }

        //Este VM manejará la creación y edición de productos.
        
        //Para accion de edicion 
        public int IdProducto { get; set; }

        //opcion por defecto sin Required, con longitud maxima de 250 caracteres
        [Display(Name = "Descripcion del Producto")] //atributo de presentacion
        [StringLength(250, ErrorMessage = "La descripcion no puede superar los 250 caracteres")]
        public string Descripcion { get; set; }

        //Debe ser requerido y positivo
        [Display(Name = "Precio Unitario")] 
        [Required(ErrorMessage = "El precio es obligatorio")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Precio invalido")]
        public decimal Precio { get; set; }

    }
}