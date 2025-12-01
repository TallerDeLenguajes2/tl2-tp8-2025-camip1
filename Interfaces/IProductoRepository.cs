using tl2_tp8_2025_camip1.Models;
namespace tl2_tp8_2025_camip1.Interfaces;

public interface IProductoRepository
{
    // El método GetAll devuelve una lista de Producto
    public List<Producto> GetAll();
    
    // El método Add recibe un Producto para dar de alta
    public void Create(Producto prod);
    
    // El método Update recibe un Producto para modificar
    public void Update(Producto prodModificado);
    
    // El método GetById devuelve un único Producto o null
    public Producto GetById(int id);
    
    // El método Delete recibe el ID del producto a eliminar
    public void Delete(int id);
}