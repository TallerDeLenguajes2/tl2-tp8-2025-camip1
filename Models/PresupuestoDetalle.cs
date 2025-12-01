namespace tl2_tp8_2025_camip1.Models
{
    public class PresupuestoDetalle
    {
         // No siempre es necesario, pero es bueno si la tabla lo tiene
        public int IdPresupuestoDetalle { get; set; }

        // Relación con Producto 
        public Producto Producto { get; set; }

        // Cantidad de ese producto en este presupuesto
        public int Cantidad { get; set; }
        
    }
}