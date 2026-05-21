using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Models.Projects.DonViPhongBan
{
    public class PhongBanCreateModel
    {
        public string Id { get; set; }
        public string IdDonVi { get; set; }
        public string TenPhongBan { get; set; }
        public string MoTa { get; set; }
        public string IdNguoiPhuTrach { get; set; }

        public List<string> ViTri { get; set; }
        public PhongBanCreateModel()
        {
            ViTri = new List<string>();
        }
    }
}
