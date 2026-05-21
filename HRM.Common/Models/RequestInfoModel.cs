using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Common.Models
{
    public class RequestInfoModel
    {
        public string RequestUserId{ get; set; }
        public int RequestUserType { get; set; }
        public int? RequestTinhId { get; set; }
        public int? RequestHuyenId { get; set; }
        public int? RequestXaId { get; set; }
    }
}
