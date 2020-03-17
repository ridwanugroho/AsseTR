using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AsseTS.Models
{
    public class History
    {
        public Guid Id { get; set; }
        public Room Room { get; set; }
        public DateTime InDate { get; set; }
        public DateTime OutDate { get; set; }
        public string InStatus { get; set; }
        public string OutStatus { get; set; }
        public int InCount { get; set; }
        public int OutCount { get; set; }
    }
}
