using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Models.Cores.User
{
    public class UserInfoModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        //public string PhoneNumber { get; set; }
        public string Avatar { get; set; }
    }
}
