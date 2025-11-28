using System;
using Maratonic.Core.Enums;
namespace Maratonic.API.DTOs
{
    public class RaceUpdateDto
    {
        public int RaceId { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public decimal RegistrationFee { get; set; }
        public string? Location { get; set; }
        public string? Description { get; set; }
        public int? Capacity { get; set; }
    }

}

