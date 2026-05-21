using HRM.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Models.Projects.LoaiCong
{
    public class LoaiCongSearchModel : SearchBaseModel
    {
        public string MaLoaiCong { get; set; }
        public string TenLoaiCong { get; set; }
    }
}
