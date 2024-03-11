﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMaz.Entity
{
    public class AuthenticateResponse
    {
        public string FirstName { get; set; }


        public string LastName { get; set; }


        public string Email { get; set; }


        public int Role { get; set; }


        public string Password { get; set; }
    }
}
