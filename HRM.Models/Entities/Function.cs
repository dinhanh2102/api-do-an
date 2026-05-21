using System;
using System.Collections.Generic;

#nullable disable

namespace HRM.Models.Entities
{
    public partial class Function
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int Index { get; set; }
    }
}
