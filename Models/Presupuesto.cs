using System;
using System.Collections.Generic;
using System.Linq; // Necesario para usar .Sum()

namespace tl2_tp8_2025_camip1.Models
{
    public class Presupuesto
    {
        private const decimal IVA = 0.21m; // 21% de IVA   

        // Propiedades de la Entidad     
        public int IdPresupuesto { get; set; }
        public string NombreDestinatario { get; set; }
        public DateTime FechaCreacion { get; set; }

        // Propiedad Relacional (Colección de ítems)
        public List<PresupuestoDetalle> ListaDetalles { get; set; } = new List<PresupuestoDetalle>();

        //METODOS

         // ------------------------------------------------------------------
        // MÉTODOS DE LÓGICA DE NEGOCIO REQUERIDOS
        // ------------------------------------------------------------------
        
        /// <summary>
        /// Calcula el monto total del presupuesto SIN IVA.
        /// </summary>
        /// <returns>Monto total base.</returns>
        public decimal MontoPresupuesto()
        {
            return ListaDetalles.Sum(d => d.Producto.Precio * d.Cantidad);
        }

        /// <summary>
        /// Calcula el monto total del presupuesto CON IVA (21%).
        /// </summary>
        /// <returns>Monto total con IVA incluido.</returns>
        public decimal MontoPresupuestoConIva() //considerar iva 21
        {
            decimal presupuesto = MontoPresupuesto();
            return presupuesto * (1 + IVA);
        }

        /// <summary>
        /// Cuenta el total de productos sumando las cantidades de todos los ítems.
        /// </summary>
        /// <returns>Cantidad total de unidades de productos.</returns>
        public int CantidadProductos() //contar total de productos (sumador de todas las cantidades del detalle)
        {
            // Suma la propiedad Cantidad de cada elemento en la lista Detalle
            return ListaDetalles.Sum(d => d.Cantidad);
        }
    }
}