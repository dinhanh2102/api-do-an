using HRM.Common.Resource;
using HRM.Common;
using HRM.Models.Cores.Combobox;
using HRM.Models.Entities;
using HRM.Models.Projects.NhanVien;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRM.Services.Cores.Combobox
{
    public class ComboboxService : IComboboxService
    {
        private CoreProjectContext _sqlContext;

        public ComboboxService(CoreProjectContext sqlContext)
        {
            _sqlContext = sqlContext;
        }

        /// <summary>
        /// Lixt fix giới tính
        /// </summary>
        /// <returns></returns>
        public async Task<List<ComboboxModel>> GetAllGioiTinh()
        {
            ComboboxModel comboboxModel;
            List<ComboboxModel> listCombobox = new List<ComboboxModel>();
            comboboxModel = new ComboboxModel();
            comboboxModel.Id = "1";
            comboboxModel.Name = "Nam";
            listCombobox.Add(comboboxModel);

            comboboxModel = new ComboboxModel();
            comboboxModel.Id = "2";
            comboboxModel.Name = "Nữ";
            listCombobox.Add(comboboxModel);

            comboboxModel = new ComboboxModel();
            comboboxModel.Id = "3";
            comboboxModel.Name = "Khác";
            listCombobox.Add(comboboxModel);
            return listCombobox;
        }

        /// <summary>
        /// Danh sách nhóm người dùng
        /// </summary>
        /// <returns></returns>
        public async Task<List<ComboboxModel>> GetAllGroupUser()
        {
            List<ComboboxModel> listCombobox = new List<ComboboxModel>();
            try
            {
                listCombobox = _sqlContext.UserGroup.OrderBy(r => r.Name).Select(s => new ComboboxModel()
                {
                    Id = s.Id,
                    Name = s.Name
                }).ToList();
                return listCombobox;
            }
            catch { return listCombobox; }

        }

        /// <summary>
        /// Danh sách tài khoản
        /// </summary>
        /// <returns></returns>
        public async Task<List<ComboboxModel>> GetAllUser()
        {
            List<ComboboxModel> listCombobox = new List<ComboboxModel>();
            try
            {
                listCombobox = _sqlContext.User.OrderBy(r => r.UserName).Select(s => new ComboboxModel()
                {
                    Id = s.Id,
                    Name = s.UserName
                }).ToList();
                return listCombobox;
            }
            catch { return listCombobox; }
        }

        /// <summary>
        /// Danh sách menu
        /// </summary>
        /// <returns></returns>
        public async Task<List<ComboboxModel>> GetMenu()
        {
            List<ComboboxModel> listCombobox = new List<ComboboxModel>();
            try
            {
                var listAllMenu = _sqlContext.MenuSystem.OrderBy(r => r.Index).Select(s => new ComboboxModel()
                {
                    Id = s.Id,
                    Name = s.TitleDefault,
                    IdParent = s.ParentId
                }).ToList();


                var listParent = listAllMenu.Where(s => string.IsNullOrEmpty(s.IdParent)).ToList();
                var listAllSubMenu = listAllMenu.Where(s => !string.IsNullOrEmpty(s.IdParent)).ToList();
                foreach (var item in listParent)
                {
                    listCombobox.Add(item);
                    this.GetSubMenu(item.Id, listCombobox, listAllSubMenu);
                }

                return listCombobox;
            }
            catch { return listCombobox; }
        }

        /// <summary>
        /// Lấy danh sách menu con
        /// </summary>
        /// <param name="parentId">Id menu cha</param>
        /// <param name="listMenu">Danh sách menu</param>
        /// <param name="listAllSubMenu">Danh sách menu con</param>
        private void GetSubMenu(string parentId, List<ComboboxModel> listMenu, List<ComboboxModel> listAllSubMenu)
        {
            var listSubMenu = listAllSubMenu.Where(s => parentId.Equals(s.IdParent)).ToList();
            foreach (var item in listSubMenu)
            {
                listMenu.Add(item);
                this.GetSubMenu(item.Id, listMenu, listAllSubMenu);
            }
        }

        /// <summary>
        /// Danh sách nhân viên
        /// </summary>
        /// <returns></returns>
        public async Task<List<ComboboxModel>> GetNhanVien()
        {
            List<ComboboxModel> listCombobox = new List<ComboboxModel>();
            try
            {
                listCombobox = _sqlContext.NhanVien.AsNoTracking().OrderBy(r => r.TenNhanVien).Select(s => new ComboboxModel()
                {
                    Id = s.Id,
                    Name = s.TenNhanVien
                }).ToList();
                return listCombobox;
            }
            catch { return listCombobox; }
        }

        /// <summary>
        /// Danh sách đơn vị
        /// </summary>
        /// <returns></returns>
        public async Task<List<ComboboxModel>> GetDonVi()
        {
            List<ComboboxModel> listCombobox = new List<ComboboxModel>();
            try
            {
                listCombobox = _sqlContext.DonVi.AsNoTracking().OrderBy(r => r.TenDonVi).Select(s => new ComboboxModel()
                {
                    Id = s.Id,
                    Name = s.TenDonVi
                }).ToList();
                return listCombobox;
            }
            catch { return listCombobox; }
        }

        /// <summary>
        /// Lấy danh sách phòng ban theo id đơn vị
        /// </summary>
        /// <returns></returns>
        public async Task<List<ComboboxModel>> GetPhongBanByIdDonVi(string idDonVi)
        {
            List<ComboboxModel> listCombobox = new List<ComboboxModel>();
            try
            {
                listCombobox = _sqlContext.PhongBan.AsNoTracking().OrderBy(r => r.TenPhongBan).Select(s => new ComboboxModel()
                {
                    Id = s.Id,
                    Name = s.TenPhongBan,
                    IdParent = s.IdDonVi,
                }).ToList();
                if (!string.IsNullOrEmpty(idDonVi))
                {
                    listCombobox = listCombobox.Where(e => idDonVi.Equals(e.IdParent)).ToList();
                }
                return listCombobox;
            }
            catch { return listCombobox; }
        }


        /// <summary>
        /// lấy ra danh sách chức danh theo phòng ban
        /// </summary>
        /// <returns></returns>
        public async Task<List<ComboboxModel>> GetChucDanhByIdPhongBan(string idPhongBan)
        {
            List<ComboboxModel> listCombobox = new List<ComboboxModel>();
            try
            {
                listCombobox = _sqlContext.ChucDanh.AsNoTracking().OrderBy(r => r.TenChucDanh).Select(s => new ComboboxModel()
                {
                    Id = s.Id,
                    Name = s.TenChucDanh,
                    IdParent = s.IdPhongBan,
                }).ToList();
                if (!string.IsNullOrEmpty(idPhongBan))
                {
                    listCombobox = listCombobox.Where(e => idPhongBan.Equals(e.IdParent)).ToList();
                }
                return listCombobox;
            }
            catch { return listCombobox; }
        }

        /// <summary>
        /// Lấy danh sách ngân hàng 
        /// </summary>
        /// <returns></returns>
        public async Task<List<ComboboxModel>> GetNganHang()
        {
            List<ComboboxModel> listCombobox = new List<ComboboxModel>();
            try
            {
                listCombobox = _sqlContext.NganHang.AsNoTracking().OrderBy(r => r.TenNganHang).Select(s => new ComboboxModel()
                {
                    Id = s.Id,
                    Name = s.TenNganHang,
                }).ToList();
                return listCombobox;
            }
            catch { return listCombobox; }
        }

        /// <summary>
        /// Danh sách trình độ
        /// </summary>
        /// <returns></returns>
        public async Task<List<ComboboxModel>> GetTrinhDo()
        {
            List<ComboboxModel> listCombobox = new List<ComboboxModel>();
            try
            {
                listCombobox = _sqlContext.TrinhDo.OrderBy(r => r.Name).Select(s => new ComboboxModel()
                {
                    Id = s.Id,
                    Name = s.Name
                }).ToList();
                return listCombobox;
            }
            catch { return listCombobox; }
        }

        /// <summary>
        /// Lấy danh sách bảo hiểm
        /// </summary>
        /// <returns></returns>
        public async Task<List<ComboboxModel>> GetBaoHiem()
        {
            List<ComboboxModel> listCombobox = new List<ComboboxModel>();
            try
            {
                //listCombobox = _sqlContext.BaoHiem.AsNoTracking().OrderBy(r => r.B).Select(s => new ComboboxModel()
                //{
                //    Id = s.Id,
                //    Name = s.TenNganHang,
                //}).ToList();
                return listCombobox;
            }
            catch { return listCombobox; }
        }

        /// <summary>
        /// Danh sách phụ cấp
        /// </summary>
        /// <returns></returns>
        public async Task<List<ComboboxModel>> GetPhuCap()
        {
            List<ComboboxModel> listCombobox = new List<ComboboxModel>();
            try
            {
                listCombobox = _sqlContext.PhuCap.AsNoTracking().OrderBy(r => r.Name).Select(s => new ComboboxModel()
                {
                    Id = s.Id,
                    Name = s.Name
                }).ToList();
                return listCombobox;
            }
            catch { return listCombobox; }
        }

        public async Task<NhanVienSearchResultModel> GetNhanVienByCodeAsync(string code)
        {
            var nhanVien = _sqlContext.NhanVien.AsNoTracking().FirstOrDefault(x => x.MaNhanVien.Equals(code));
            if (nhanVien == null)
            {
                throw HRMException.CreateInstance(MessageResourceKey.MSG0031);
            }
            var query = (from a in _sqlContext.NhanVien.AsNoTracking()
                         join b in _sqlContext.ChucDanh.AsNoTracking() on a.IdChucDanh equals b.Id
                         join c in _sqlContext.PhongBan.AsNoTracking() on b.IdPhongBan equals c.Id
                         where a.MaNhanVien.Equals(code)
                         select new NhanVienSearchResultModel
                         {
                             Id = a.Id,
                             MaNhanVien = a.MaNhanVien,
                             TenNhanVien = a.TenNhanVien,
                             SoDienThoai = a.SoDienThoai,
                             Email = a.Email,
                             DiaChi = a.DiaChi,
                             UserId = a.UserId,
                             DOB = a.DOB,
                             CanCuocCongDan = a.CanCuocCongDan,
                             IdChucDanh = a.IdChucDanh,
                             TenChucDanh = b.TenChucDanh,
                             SoTKNganHang = a.SoTKNganHang,
                             //NganHang = a.NganHang,
                             GioiTinh = a.GioiTinh,
                             IdTrinhDo = a.IdTrinhDo,
                             IdDonVi = c.IdDonVi,
                             IdPhongBan = b.IdPhongBan,
                             Avatar = a.Avatar,
                         }).FirstOrDefault();
            return query;
        }
        public async Task<List<ComboboxModel>> GetGroupUser(string id)
        {
            List<ComboboxModel> listCombobox = new List<ComboboxModel>();
            try
            {
                listCombobox = _sqlContext.UserGroup.AsNoTracking().Where(s => s.Id == id).OrderBy(r => r.Name).Select(s => new ComboboxModel()
                {
                    Id = s.Id,
                    Name = s.Name.ToLower()
                }).ToList();

                if(string.IsNullOrEmpty(id))
                {
                    listCombobox = listCombobox.Where(s => s.Id.Equals(id)).ToList();
                }
                return listCombobox;
            }
            catch { return listCombobox; }
        }
    }
}
