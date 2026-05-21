using HRM.Common.Models;

namespace HRM.Models.Cores.GroupUser
{
    public class GroupUserSearchModel : SearchBaseModel
    {
        public string Name { get; set; }
        public bool? LockoutEnabled { get; set; }
    }
}