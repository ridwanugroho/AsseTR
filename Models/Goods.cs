using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;


namespace AsseTS.Models
{
    public class Goods
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Brand Brand { get; set; }
        public Category Category { get; set; }
        public double UnitPrice { get; set; }

        [JsonIgnore]
        public string _images { get; set; }
        public string Status { get; set; }
        public Room Locations { get; set; }
        public string SerialNumber { get; set; }
        public string Barcode { get; set; }

        public int DataStatus { get; set; } = 1;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime EditedAt { get; set; }
        public DateTime DeletedAt { get; set; }

        [JsonIgnore]
        public string _histories { get; set; } = "[]";

        [NotMapped]
        public List<History> Histories
        {
            get { return _histories == null ? null : JsonConvert.DeserializeObject<List<History>>(_histories); }
            set { _histories = JsonConvert.SerializeObject(value); }
        }

        [NotMapped]
        public List<string> Images
        {
            get { return _images == null ? null : JsonConvert.DeserializeObject<List<string>>(_images); }
            set { _images = JsonConvert.SerializeObject(value); }
        }
    }

}
