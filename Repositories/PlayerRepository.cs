using System.Threading.Tasks;
using System;
using Clubs.Data;
using Clubs.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Clubs.Repositories
{
    public class PlayerRepository : IPlayerRepository
    {
        private readonly AppDbContext _context;

        public PlayerRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Player> GetPlayerByIdAsync(int playerId)
        {
            return await _context.Players.FirstOrDefaultAsync(p => p.Id == playerId);
        }
    }
}
