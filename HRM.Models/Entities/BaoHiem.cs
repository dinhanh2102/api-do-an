using System;
using System.Collections.Generic;

#nullable disable

namespace HRM.Models.Entities
{
    public partial class BaoHiem
    {
        public string Id { get; set; }
        public string SoBH { get; set; }
        public DateTime NgayCap { get; set; }
        public string NoiCap { get; set; }
        public string NoiKhamBenh { get; set; }
    }
}
