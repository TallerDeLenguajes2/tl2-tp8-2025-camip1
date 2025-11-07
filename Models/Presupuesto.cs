namespace tl2_tp8_2025_camip1.Models
{
    public class Presupuesto
    {
        public int Id_presupuesto { get; set; }
        public string Nombre_destinatario { get; set; }
        public DateTime Fecha_creacion { get; set; }
        public List<PresupuestoDetalle> ListaDetalles { get; set; }

        //METODOS
        double MontoPresupuesto()
        {
            double presupuesto = 0;
            foreach (var d in ListaDetalles)
            {
                double precio = d.Producto.Precio;
                presupuesto += precio * d.Cantidad;
            }
            return presupuesto;
        }
        double MontoPresupuestoConIva() //considerar iva 21
        {
            double presupuesto = MontoPresupuesto() * 1.21;
            return presupuesto;
        }
        int CantidadProductos () //contar total de productos (sumador de todas las cantidades del detalle)
        {
            int cont = 0;
            foreach (var d in ListaDetalles)
            {
                cont += d.Cantidad;
            }
            return cont;
        }
    }
}