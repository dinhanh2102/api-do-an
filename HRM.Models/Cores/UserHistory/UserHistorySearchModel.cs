using HRM.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Models.Cores.UserHistory
{
    public class UserHistorySearchModel : SearchBaseModel
    {
        public string Name { get; set; }
        public string UserId { get; set; }
        public int? Type { get; set; }
        public bool IsExport { get; set; }
    }
}
