using HRM.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Models.Projects.PhuCapNhanVien
{
    public class PhuCapNhanVienSearchModel : SearchBaseModel
    {
        public string Id { get; set; }
        public string IdNhanVien { get; set; }
        public string IdPhuCap { get; set; }
        public string NoiDung { get; set; }

    }
}
