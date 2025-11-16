namespace tl2_tp8_2025_camip1.Models
{
    public class Presupuesto
    {
        public int IdPresupuesto { get; set; }
        public string NombreDestinatario { get; set; }
        public DateTime FechaCreacion { get; set; }
        public List<PresupuestoDetalle> ListaDetalles { get; set; }

        //METODOS
        public decimal MontoPresupuesto()
        {
            return ListaDetalles.Sum(d => d.Producto.Precio * d.Cantidad);
        }
        public decimal MontoPresupuestoConIva() //considerar iva 21
        {
            decimal presupuesto = MontoPresupuesto() * (decimal)1.21;
            return presupuesto;
        }
        public int CantidadProductos() //contar total de productos (sumador de todas las cantidades del detalle)
        {
            return ListaDetalles.Sum(d => d.Cantidad);
        }
    }
}