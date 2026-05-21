using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Models.Projects.ChamCong
{
    public class ChamCongGetModel
    {
        public string Id { get; set; }
        public int Ngay { get; set; }
        public DateTime? ThoiGianVao { get; set; }
        public int GioVao { get;set; }
        public int PhutVao { get;set; }
        public int GioRa { get; set; }
        public int PhutRa { get; set; }
        public DateTime? ThoiGianRa { get; set; }
        public string MaNhanVien { get; set; }
        public string IdLoaiCong { get; set; }
        public string IdKyCong { get; set; }
        public int Thang { get; set; }
        public int Nam { get; set; }
    }
}
