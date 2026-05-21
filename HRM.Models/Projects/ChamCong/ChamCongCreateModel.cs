using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Models.Projects.ChamCong
{
    public class ChamCongCreateModel
    {
        public string MaNhanVien {  get; set; }
        public int Ngay { get; set; }
        public DateTime? ThoiGian { get; set; }
    }
}
