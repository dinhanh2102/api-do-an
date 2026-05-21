using System;
using System.Collections.Generic;

#nullable disable

namespace HRM.Models.Entities
{
    public partial class SystemParams
    {
        public string Id { get; set; }
        public string ParamName { get; set; }
        public string ParamValue { get; set; }
        public string DisplayName { get; set; }
        public int Index { get; set; }
        public int? ControlType { get; set; }
    }
}
