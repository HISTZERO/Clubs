using System;
using System.Collections.Generic;

namespace Clubs.DTOs
{
    public class ClubRequestDto
    {
        public string Name { get; set; } = string.Empty;
    }

    public class ClubResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<PlayerResponseDto> Members { get; set; } = new();
    }
}
