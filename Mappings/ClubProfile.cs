using AutoMapper;
using Clubs.Models;
using Clubs.DTOs;

namespace Clubs.Mappings
{
    public class ClubProfile : Profile
    {
        public ClubProfile()
        {
            CreateMap<Club, ClubResponseDto>();
            CreateMap<Player, PlayerResponseDto>();

            CreateMap<ClubRequestDto, Club>();

            CreateMap<PlayerRequestDto, Player>();
        }
    }
}
