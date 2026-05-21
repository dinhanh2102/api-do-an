using HRM.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Models.Projects.KyCong
{
    public class KyCongSearchModel : SearchBaseModel
    {
        public string MaKyCong { get; set; }
        public string TenKyCong { get; set; }
    }
}
