using Clubs.Models;
using System.Threading.Tasks;

namespace Clubs.Repositories
{
    public interface IPlayerRepository
    {
        Task<Player?> GetPlayerByIdAsync(int playerId);
    }
}
