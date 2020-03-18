using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace AsseTS.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        [JsonIgnore]
        public string _address { get; set; }
        public int Role { get; set; }

        public int DataStatus { get; set; } = 1;

        [NotMapped]
        public Address Address
        {
            get { return _address == null ? null : JsonConvert.DeserializeObject<Address>(_address); }
            set { _address = JsonConvert.SerializeObject(value); }
        }
    }
}
