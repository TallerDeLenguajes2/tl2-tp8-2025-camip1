namespace tl2_tp8_2025_camip1.Models
{
    public class Presupuesto
    {
        public int IdPresupuesto { get; set; }
        public string NombreDestinatario { get; set; }
        public DateOnly FechaCreacion { get; set; }
        public List<PresupuestoDetalle> ListaDetalles { get; set; }

        //METODOS
        public int MontoPresupuesto()
        {
            return ListaDetalles.Sum(d => d.Producto.Precio * d.Cantidad);
        }
        public double MontoPresupuestoConIva() //considerar iva 21
        {
            int presupuesto = (int)(MontoPresupuesto() * 1.21);
            return presupuesto;
        }
        public int CantidadProductos() //contar total de productos (sumador de todas las cantidades del detalle)
        {
            return ListaDetalles.Sum(d => d.Cantidad);
        }
    }
}