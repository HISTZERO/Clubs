using System;
using System.Threading.Tasks;
using Clubs.DTOs;

namespace Clubs.Services
{
    public interface IClubService
    {
        Task<ClubResponseDto> CreateClubAsync(ClubRequestDto clubDto, int playerId);
        Task AddMemberAsync(Guid clubId, int playerId);
        Task<ClubResponseDto> GetClubByIdAsync(Guid clubId);
        Task<bool> ClubExistsAsync(string name); 
    }

}

