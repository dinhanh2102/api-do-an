using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Linq;

namespace HRM.Models.Cores.GroupUser
{
    public class GroupUserResultModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool? LockoutEnabled { get; set; }
        public string Description { get; set; }
    }
}