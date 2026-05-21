using HRM.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Models.Projects.LoaiCa
{
    public class LoaiCaSearchModel : SearchBaseModel
    {
        public string MaLoaiCa { get; set; }
        public string TenLoaiCa { get; set; }
    }
}
