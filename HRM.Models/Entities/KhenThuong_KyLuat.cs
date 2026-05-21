using System;
using System.Collections.Generic;

#nullable disable

namespace HRM.Models.Entities
{
    public partial class KhenThuong_KyLuat
    {
        public string Id { get; set; }
        public int SoKTKL { get; set; }
        public string NoiDung { get; set; }
        public DateTime Ngay { get; set; }
        public string IdNhanVien { get; set; }
        public int Loai { get; set; }
    }
}
