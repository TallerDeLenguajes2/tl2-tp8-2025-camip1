using tl2_tp8_2025_camip1.Models;
namespace tl2_tp8_2025_camip1.Interfaces;

public interface IUserRepository
{
    Usuario GetUser(string username, string password);
}
