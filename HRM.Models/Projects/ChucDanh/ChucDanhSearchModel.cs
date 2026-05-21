using HRM.Common.Models;

namespace HRM.Models.Projects.ChucDanh
{
    public class ChucDanhSearchModel : SearchBaseModel
    {
        public string Id { get; set; }
        public string TenChucDanh { get; set; }
        public string IdDonVi{ get; set; }
        public string IdPhongBan { get; set; }
    }
}
