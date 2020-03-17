using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AsseTS.Models
{
    public class Room
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Building { get; set; }
        public string Unit { get; set; }
        public string Number { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime EditedAt { get; set; }
    }
}
