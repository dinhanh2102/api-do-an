using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Models.Projects.ChucDanh
{
    public class ChucDanhSearchResultModel
    {
        public string Id { get; set; }
        public string TenChucDanh { get; set; }
        public string MoTa { get; set; }
        public string IdPhongBan { get; set; }
        public string IdDonVi{ get; set; }
        public string TenPhongBan { get; set; }
        public string TenDonVi{ get; set; }
        public double LuongCoBan { get; set; }
    }
}
