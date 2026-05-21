using System;
using System.Collections.Generic;

#nullable disable

namespace HRM.Models.Entities
{
    public partial class UserGroupFunction
    {
        public string Id { get; set; }
        public string FunctionId { get; set; }
        public string UserGroupId { get; set; }
    }
}
