using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AsseTS.Models
{
    public class Category
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime EditedAt { get; set; }
    }
}
