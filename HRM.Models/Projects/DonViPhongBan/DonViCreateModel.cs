using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Models.Projects.DonViPhongBan
{
    public class DonViCreateModel
    {
        public string Id { get; set; }
        public string TenDonVi { get; set; }
        public string MaDonVi { get; set; }
        public string ParentId { get; set; }
    }
}
