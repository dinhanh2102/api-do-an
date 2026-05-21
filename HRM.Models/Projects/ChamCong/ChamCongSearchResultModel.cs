using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Models.Projects.ChamCong
{
    public class ChamCongSearchResultModel
    {
        public string Id { get; set; }
        public string MaChamCong { get; set; }
        public int? Thang { get; set; }
        public int? Nam { get; set; }
        public int? Khoa { get; set; }
        public DateTime? NgayTinhCong { get; set; }
        public double? NgayCongTrongThang { get; set; }
        public string MaCongTy { get; set; }
        public int TrangThai { get; set; }
    }
}
