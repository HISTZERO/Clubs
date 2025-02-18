using System;
using System.Threading.Tasks;
using Clubs.Data;
using Clubs.Models;
using Microsoft.EntityFrameworkCore;

namespace Clubs.Repositories
{
    public class ClubRepository : IClubRepository
    {
        private readonly AppDbContext _context;

        public ClubRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Club?> GetClubByIdAsync(Guid clubId)
        {
            return await _context.Clubs.Include(c => c.Members)
                                       .FirstOrDefaultAsync(c => c.Id == clubId);
        }

        public async Task<Club> CreateClubAsync(Club club)
        {
            _context.Clubs.Add(club);
            await _context.SaveChangesAsync();
            return club;
        }

        public async Task AddMemberAsync(Guid clubId, Player player)
        {
            var club = await _context.Clubs.Include(c => c.Members)
                                           .FirstOrDefaultAsync(c => c.Id == clubId);
            if (club != null)
            {
                club.Members.Add(player);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ClubExistsAsync(string name)
        {
            return await _context.Clubs.AnyAsync(c => c.Name == name);
        }

    }

}
