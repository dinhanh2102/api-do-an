using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Models.Projects.ChucDanh
{
    public class ChucDanhCreateModel
    {
        public string Id { get; set; }
        public string TenChucDanh { get; set; }
        public string MoTa { get; set; }
        public string IdPhongBan { get; set; }
        public float LuongCoBan { get; set; }
        public string ViTri { get; set; }
    }
}
