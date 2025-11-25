using System;
namespace Maratonic.API.DTOs
{
    public class RaceResponseDto
    {
        public int RaceId { get; set; }
        public string Title { get; set; }
        public string Location { get; set; }
        public DateTime StartDate { get; set; }
        public decimal Price { get; set; }
        public string? Description { get; set; }
    }
}

