﻿using System;
using System.Collections.Generic;

namespace Message_Daemon.Models
{
    public partial class TokenUser
    {
        public int IdToken { get; set; }
        public string Token { get; set; }
        public string SignToken { get; set; }
        public bool? Activo { get; set; }
        public string Email { get; set; }
        public int IdUser { get; set; }
    }
}