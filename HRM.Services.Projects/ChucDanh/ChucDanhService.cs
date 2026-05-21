using HRM.Common;
using HRM.Common.Models;
using HRM.Common.Resource;
using HRM.Models.Entities;
using HRM.Models.Projects.ChucDanh;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRM.Services.Projects.ChucDanh
{
    public class ChucDanhService : IChucDanhService
    {
        private readonly CoreProjectContext _sqlContext;
        public ChucDanhService(CoreProjectContext sqlContext)
        {
            this._sqlContext = sqlContext;
        }

        /// <summary>
        /// Tìm kiếm chức danh
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        public async Task<SearchBaseResultModel<ChucDanhSearchResultModel>> SearchChucDanhAsync(ChucDanhSearchModel searchModel)
        {
            SearchBaseResultModel<ChucDanhSearchResultModel> result = new SearchBaseResultModel<ChucDanhSearchResultModel>();
            var dataQuery = (from a in _sqlContext.ChucDanh.AsNoTracking()
                             join b in _sqlContext.PhongBan.AsNoTracking() on a.IdPhongBan equals b.Id
                             join c in _sqlContext.DonVi.AsNoTracking() on b.IdDonVi equals c.Id
                             select new ChucDanhSearchResultModel
                             {
                                 Id = a.Id,
                                 TenChucDanh = a.TenChucDanh,
                                 IdPhongBan = a.IdPhongBan,
                                 TenPhongBan= b.TenPhongBan,
                                 IdDonVi = b.IdDonVi,
                                 LuongCoBan = a.LuongCoBan,
                                 TenDonVi = c.TenDonVi,
                             }).AsQueryable();

            if (!string.IsNullOrEmpty(searchModel.TenChucDanh))
            {
                dataQuery = dataQuery.Where(x => x.TenChucDanh.Contains(searchModel.TenChucDanh));
            }
            if (!string.IsNullOrEmpty(searchModel.IdDonVi))
            {
                dataQuery = dataQuery.Where(x => x.IdDonVi.Equals(searchModel.IdDonVi));
            }
            if (!string.IsNullOrEmpty(searchModel.IdPhongBan))
            {
                dataQuery = dataQuery.Where(x => x.IdPhongBan.Equals(searchModel.IdPhongBan));
            }
            result.TotalItems = dataQuery.Count();
            result.DataResults = dataQuery.OrderBy(o => o.TenChucDanh).Skip((searchModel.PageNumber - 1) * searchModel.PageSize).Take(searchModel.PageSize).ToList();
            return result;
        }

        /// <summary>
        /// Thêm mới chức danh
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task CreateChucDanhAsync(ChucDanhCreateModel model, string userId)
        {
            var checkName = _sqlContext.ChucDanh.AsNoTracking().Where(a => model.IdPhongBan.Equals(a.IdPhongBan) && model.TenChucDanh.ToLower().Trim().Equals(a.TenChucDanh.ToLower().Trim())).FirstOrDefault();
            if (checkName != null)
            {
                throw HRMException.CreateInstance("Tên chức danh đã tồn tại trong phòng ban!");
            }
            HRM.Models.Entities.ChucDanh chucDanh = new HRM.Models.Entities.ChucDanh()
            {
                Id = Guid.NewGuid().ToString(),
                IdPhongBan = model.IdPhongBan,
                TenChucDanh = model.TenChucDanh,
                LuongCoBan = model.LuongCoBan,
                
            };

            _sqlContext.ChucDanh.Add(chucDanh);

            using (var trans = _sqlContext.Database.BeginTransaction())
            {
                try
                {
                    _sqlContext.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    _sqlContext.ChangeTracker.Clear();
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Xoá chức danh
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteChucDanhAsync(string id)
        {
            var chucDanh = _sqlContext.ChucDanh.FirstOrDefault(e => id.Equals(e.Id));
            if (chucDanh == null)
            {
                throw HRMException.CreateInstance("Chức danh không tồn tại");
            }
            var check = _sqlContext.NhanVien.AsNoTracking().FirstOrDefault(x => x.IdChucDanh.Equals(id));
            if (check != null)
            {
                throw HRMException.CreateInstance("Chức danh đã được sử dụng không thể xoá!");
            }
            _sqlContext.ChucDanh.Remove(chucDanh);
            using (var trans = _sqlContext.Database.BeginTransaction())
            {
                try
                {
                    _sqlContext.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    _sqlContext.ChangeTracker.Clear();
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Lấy chức danh theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ChucDanhSearchResultModel> GetChucDanhByIdAsync(string id)
        {
            var chucDanh = _sqlContext.ChucDanh.AsNoTracking().FirstOrDefault(x => x.Id.Equals(id));
            if (chucDanh == null)
            {
                throw HRMException.CreateInstance("Chức danh không tồn tại");
            }
            var idDonVi = _sqlContext.PhongBan.AsNoTracking().FirstOrDefault(x => x.Id.Equals(chucDanh.IdPhongBan)).IdDonVi;
            return new ChucDanhSearchResultModel()
            {
                Id = chucDanh.Id,
                IdPhongBan = chucDanh.IdPhongBan,
                TenChucDanh = chucDanh.TenChucDanh,
                IdDonVi = idDonVi,
                LuongCoBan = chucDanh.LuongCoBan
                
            };
        }

        /// <summary>
        /// Cập nhật chức danh
        /// </summary>
        /// <param name="model"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task UpdateChucDanhAsync(string id, ChucDanhCreateModel model, string userId)
        {
            var chucDanh = _sqlContext.ChucDanh.FirstOrDefault(x => x.Id.Equals(id));
            if (chucDanh == null)
            {
                throw HRMException.CreateInstance("Chức danh không tồn tại");
            }
            var checkName = _sqlContext.ChucDanh.AsNoTracking().FirstOrDefault(a => model.TenChucDanh.ToLower().Trim().Equals(a.TenChucDanh.ToLower().Trim()) && model.IdPhongBan.Equals(a.IdPhongBan) && !a.Id.Equals(id));
            if (checkName != null)
            {
                throw HRMException.CreateInstance("Tên chức danh đã tồn tại trong phòng ban!");
            }
            chucDanh.IdPhongBan = model.IdPhongBan;
            chucDanh.TenChucDanh = model.TenChucDanh;
            chucDanh.LuongCoBan = model.LuongCoBan;
            using (var trans = _sqlContext.Database.BeginTransaction())
            {
                try
                {
                    _sqlContext.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    _sqlContext.ChangeTracker.Clear();
                    throw ex;
                }
            }
        }
    }
}
