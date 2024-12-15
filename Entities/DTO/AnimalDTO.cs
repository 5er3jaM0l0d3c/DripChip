using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTO
{
    public class AnimalDTO
    {
        public long Id { get; set; }
        public float Weight { get; set; }
        public float Length { get; set; }
        public float Height { get; set; }
        public string Gender { get; set; } = null!;
        public string LifeStatus { get; set; } = null!;
        public int ChipperId { get; set; }
        public long ChippingLocationId { get; set; }
    }
}
