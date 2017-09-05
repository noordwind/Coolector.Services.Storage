using System.Threading.Tasks;
using Collectively.Services.Storage.Models.Users;

namespace Collectively.Services.Storage.Services
{
    public interface IUserCache
    {
        Task AddAsync(User user);
        Task DeleteAsync(string userId);                  
    }
}