using HRM.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Models.Projects.NhanVien
{
    public class NhanVienSearchModel : SearchBaseModel
    {
        public string MaNhanVien { get; set; }
        public string TenNhanVien { get; set; }
    }
}
