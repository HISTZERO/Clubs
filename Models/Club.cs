using System;
namespace Clubs.Models
{
    public class Club
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<Player> Members { get; set; } = new();
    }
}
