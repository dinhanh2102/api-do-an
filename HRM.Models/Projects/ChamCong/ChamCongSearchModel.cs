using HRM.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Models.Projects.ChamCong
{
    public class ChamCongSearchModel : SearchBaseModel
    {
        public string MaChamCong { get; set; }
        public string TenChamCong { get; set; }
    }
}
