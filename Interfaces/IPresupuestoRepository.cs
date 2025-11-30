using tl2_tp8_2025_camip1.Models;

namespace tl2_tp8_2025_camip1.Interfaces;
public interface IPresupuestoRepository
{
    public List<Presupuesto> GetAll();
    public void Create(Presupuesto presupuesto);
    public Presupuesto GetById(int id);
    public void Update(int id, Presupuesto nuevoPresupuesto);
    public void AgregarProducto(int idPresupuesto, int idProducto, int cantidad);
    public void Delete(int id);
}

