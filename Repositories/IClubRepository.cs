using System;
using System.Threading.Tasks;
using Clubs.Models;

namespace Clubs.Repositories
{
    public interface IClubRepository
    {
        Task<Club?> GetClubByIdAsync(Guid clubId);
        Task<Club> CreateClubAsync(Club club);
        Task AddMemberAsync(Guid clubId, Player player);
        Task<bool> ClubExistsAsync(string name);
    }
}
