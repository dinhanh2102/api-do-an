using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Models.Projects.ChamCong
{
    public class ChamCongCreateHandle
    {
        public int Ngay { get; set; }
        public int PhutVao { get; set; }
        public int PhutRa { get; set; }
        public int GioVao { get; set; }
        public int GioRa { get; set; }
        public string MaNhanVien { get; set; }
        public string IdKyCong { get; set; }
        public int Thang { get; set; }
        public int Nam { get; set; }
    }
}
