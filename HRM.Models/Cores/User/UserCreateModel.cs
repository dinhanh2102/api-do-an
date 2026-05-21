using HRM.Models.Cores.GroupFunction;
using System.Collections.Generic;

namespace HRM.Models.Cores.User
{
    public class UserCreateModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Avatar { get; set; }
        public string Email { get; set; }
        //public string PhoneNumber { get; set; }
        public string ImageLink { get; set; }
        public string Password { get; set; }
        public string PasswordHash { get; set; }
        public bool LockoutEnabled { get; set; }

        public string Description { get; set; }
        public string UserGroupId { get; set; }
        public string NameGroupUser { get; set; }

        /// <summary>
        /// Danh sách nhóm chức năng
        /// </summary>
        public List<GroupFunctionModel> ListGroupFunction { get; set; }
        public UserCreateModel()
        {
            ListGroupFunction = new List<GroupFunctionModel>();
        }
    }
}
