using tl2_tp8_2025_camip1.Models;

namespace tl2_tp8_2025_camip1.Interfaces;

public interface IProductoRepository
{
    public List<Producto> GetAll();
    public void Create(Producto prod);
    public void Update(Producto prodModificado);
    public Producto GetById(int id);
    public void Delete(int id);
}