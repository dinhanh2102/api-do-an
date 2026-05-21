using HRM.Models.Projects.NhanVien;
using HRM.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Services.Projects.NhanVien
{
    public interface INhanVienService
    {
        /// <summary>
        /// Thêm mới nhân viên
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task CreateNhanVienAsync(NhanVienCreateModel model, string userId);

        /// <summary>
        /// Xoá nhân viên
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteNhanVienAsync(string id, string userId);

        /// <summary>
        /// Lấy nhân viên theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<NhanVienSearchResultModel> GetNhanVienByIdAsync(string id);

        /// <summary>
        /// Tìm kiếm nhân viên
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        Task<SearchBaseResultModel<NhanVienSearchResultModel>> SearchNhanVienAsync(NhanVienSearchModel searchModel, string userId);

        /// <summary>
        /// Cập nhật nhân viên
        /// </summary>
        /// <param name="model"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        Task UpdateNhanVienAsync(string id, NhanVienCreateModel model, string userId);
    }
}
