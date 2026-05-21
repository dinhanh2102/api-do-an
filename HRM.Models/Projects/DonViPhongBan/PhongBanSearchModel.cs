using HRM.Common.Models;

namespace HRM.Models.Projects.DonViPhongBan
{
    public class PhongBanSearchModel: SearchBaseModel
    {
        public string Id { get; set; }
        public string IdDonVi { get; set; }
        public string TenPhongBan { get; set; }
        public string MoTa { get; set; }
    }
}
