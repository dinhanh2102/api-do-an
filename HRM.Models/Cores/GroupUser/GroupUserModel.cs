using HRM.Common.Resource;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HRM.Models.Cores.GroupFunction;

namespace HRM.Models.Cores.GroupUser
{
    public class GroupUserModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<GroupFunctionModel> ListPermission { get; set; }
        public GroupUserModel()
        {
            ListPermission = new List<GroupFunctionModel>();
        }
    }
}
