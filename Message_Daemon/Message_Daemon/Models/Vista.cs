using System;
using System.Collections.Generic;

namespace Message_Daemon.Models
{
    public partial class Vista
    {
        public int IdVista { get; set; }
        public string Route { get; set; }
        public string ComponentClass { get; set; }
        public string ComponentName { get; set; }
        public bool? Activa { get; set; }
        public int IdRol { get; set; }

        public Rol IdRolNavigation { get; set; }
    }
}
