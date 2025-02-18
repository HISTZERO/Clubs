using System.Threading.Tasks;
using System;
using AutoMapper;
using Clubs.Models;
using Clubs.DTOs;
using Clubs.Repositories;
using System.Linq;
using Microsoft.EntityFrameworkCore;


namespace Clubs.Services
{
    public class ClubService : IClubService
    {
        private readonly IClubRepository _clubRepository;
        private readonly IPlayerRepository _playerRepository;
        private readonly IMapper _mapper;

        public ClubService(IClubRepository clubRepository,IPlayerRepository playerRepository, IMapper mapper)
        {
            _clubRepository = clubRepository;
            _playerRepository = playerRepository;
            _mapper = mapper;
        }

        public async Task<ClubResponseDto> GetClubByIdAsync(Guid clubId)
        {
            var club = await _clubRepository.GetClubByIdAsync(clubId);
            return club == null ? null : _mapper.Map<ClubResponseDto>(club);
        }

        public async Task<ClubResponseDto> CreateClubAsync(ClubRequestDto clubDto, int playerId)
        {
            if (await _clubRepository.ClubExistsAsync(clubDto.Name))
                throw new Exception("Club name already exists.");

            var club = _mapper.Map<Club>(clubDto);

            var player = await _playerRepository.GetPlayerByIdAsync(playerId);
            if (player == null)
                throw new Exception("Player not found.");

            club.Members.Add(player);
            var createdClub = await _clubRepository.CreateClubAsync(club);
            return _mapper.Map<ClubResponseDto>(createdClub);
        }

        public async Task AddMemberAsync(Guid clubId, int playerId)
        {
            var club = await _clubRepository.GetClubByIdAsync(clubId);
            if (club == null)
                throw new Exception("Club not found.");

            // Prevent duplicate memberships
            if (club.Members.Any(m => m.Id == playerId))
                throw new Exception("Player is already a member of this club.");

            var player = await _playerRepository.GetPlayerByIdAsync(playerId);
            if (player == null)
                throw new Exception("Player not found.");

            await _clubRepository.AddMemberAsync(clubId, player);
        }

        public Task<bool> ClubExistsAsync(string name)
        {
            return _clubRepository.ClubExistsAsync(name);
        }
    }
}
