﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMaz.Common
{
    public class ResetPasswordRequest
    {
        public string UserId { get; set; }
        public string Password { get; set; }
    }
}
