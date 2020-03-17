﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public int Role { get; set; }
    }
}
