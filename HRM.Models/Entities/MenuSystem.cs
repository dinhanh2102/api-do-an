using System;
using System.Collections.Generic;

#nullable disable

namespace HRM.Models.Entities
{
    public partial class MenuSystem
    {
        public string Id { get; set; }
        public string TitleDefault { get; set; }
        public string Icon { get; set; }
        public string Url { get; set; }
        public string ParentId { get; set; }
        public bool IsDisable { get; set; }
        public int Index { get; set; }
        public bool IsDefaultMenu { get; set; }
    }
}
