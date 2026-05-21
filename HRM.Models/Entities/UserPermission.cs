using System;
using System.Collections.Generic;

#nullable disable

namespace HRM.Models.Entities
{
    public partial class UserPermission
    {
        public string Id { get; set; }
        public string FunctionId { get; set; }
        public string UserId { get; set; }
    }
}
