using tl2_tp8_2025_camip1.Models;
namespace tl2_tp8_2025_camip1.Interfaces;
public interface IPresupuestoRepository
{
    List<Presupuesto> GetAll();
    void Create(Presupuesto presupuesto);
    Presupuesto GetById(int id);
    void Update(int id, Presupuesto nuevoPresupuesto);
    
    // Método clave del TP para la relación N:M
    void AgregarProducto(int idPresupuesto, int idProducto, int cantidad);
    void Delete(int id);
}

