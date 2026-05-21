using HRM.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Models.Projects.BangLuong
{
    public class BangLuongSearchModel : SearchBaseModel
    {
        public int Thang { get; set; }
        public int Nam { get; set; }
        public string IdKyCong { get; set; }
        public string TenNhanVien { get; set; }
    }
}
