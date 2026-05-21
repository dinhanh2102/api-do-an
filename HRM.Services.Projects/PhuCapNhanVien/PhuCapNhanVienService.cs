using HRM.Common;
using HRM.Common.Models;
using HRM.Models.Entities;
using HRM.Models.Projects.PhuCapNhanVien;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRM.Services.Projects.PhuCapNhanVien
{
    public class PhuCapNhanVienService: IPhuCapNhanVienService
    {
        private readonly CoreProjectContext _sqlContext;
        public PhuCapNhanVienService(CoreProjectContext sqlContext)
        {
            this._sqlContext = sqlContext;
        }

        /// <summary>
        /// Tìm kiếm phụ cấp nhân viên
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        public async Task<SearchBaseResultModel<PhuCapNhanVienSearchResultModel>> SearchPhuCapNhanVienAsync(PhuCapNhanVienSearchModel searchModel)
        {
            SearchBaseResultModel<PhuCapNhanVienSearchResultModel> result = new SearchBaseResultModel<PhuCapNhanVienSearchResultModel>();
            var dataQuery = (from a in _sqlContext.NhanVien_PhuCap.AsNoTracking()
                             join b in _sqlContext.NhanVien.AsNoTracking() on a.IdNhanVien equals b.Id
                             select new PhuCapNhanVienSearchResultModel
                             {
                                 Id = a.Id,
                                 TenNhanVien = b.TenNhanVien,
                                 IdNhanVien = a.IdNhanVien,
                                 IdPhuCap = a.IdPhuCap,
                                 NoiDung = a.NoiDung,
                             }).AsQueryable();

            if (!string.IsNullOrEmpty(searchModel.IdNhanVien))
            {
                dataQuery = dataQuery.Where(x => x.IdNhanVien.Contains(searchModel.IdNhanVien));
            }
            if (!string.IsNullOrEmpty(searchModel.IdPhuCap))
            {
                dataQuery = dataQuery.Where(x => x.IdPhuCap.Contains(searchModel.IdPhuCap));
            }
            result.TotalItems = dataQuery.Count();
            result.DataResults = dataQuery.OrderBy(o => o.TenNhanVien).Skip((searchModel.PageNumber - 1) * searchModel.PageSize).Take(searchModel.PageSize).ToList();
            return result;
        }

        /// <summary>
        /// Thêm mới phụ cấp nhân viên
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task CreatePhuCapNhanVienAsync(PhuCapNhanVienCreateModel model, string userId)
        {
            //var checkName = _sqlContext.NhanVien_PhuCap.AsNoTracking().Where(a => model.Name.ToLower().Trim().Equals(a.Name.ToLower().Trim())).FirstOrDefault();
            //if (checkName != null)
            //{
            //    throw HRMException.CreateInstance("Tên phụ cấp nhân viên đã tồn tại!");
            //}
            NhanVien_PhuCap phuCapNhanVien = new NhanVien_PhuCap()
            {
                Id = Guid.NewGuid().ToString(),
                IdNhanVien = model.IdNhanVien,
                IdPhuCap = model.IdPhuCap,
                NoiDung = model.NoiDung,
            };

            _sqlContext.NhanVien_PhuCap.Add(phuCapNhanVien);

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
        /// Xoá phụ cấp nhân viên
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeletePhuCapNhanVienAsync(string id)
        {
            var phuCapNhanVien = _sqlContext.NhanVien_PhuCap.FirstOrDefault(e => id.Equals(e.Id));
            if (phuCapNhanVien == null)
            {
                throw HRMException.CreateInstance("Phụ cấp của nhân viên không tồn tại");
            }
            _sqlContext.NhanVien_PhuCap.Remove(phuCapNhanVien);
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
        /// Lấy phụ cấp nhân viên theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<PhuCapNhanVienSearchResultModel> GetPhuCapNhanVienByIdAsync(string id)
        {
            var phuCapNhanVien = _sqlContext.NhanVien_PhuCap.AsNoTracking().FirstOrDefault(x => x.Id.Equals(id));
            if (phuCapNhanVien == null)
            {
                throw HRMException.CreateInstance("Phụ cấp của nhân viên không tồn tại");
            }
            return new PhuCapNhanVienSearchResultModel()
            {
                Id = phuCapNhanVien.Id,
                IdNhanVien = phuCapNhanVien.IdNhanVien,
                IdPhuCap = phuCapNhanVien.IdPhuCap,
                NoiDung = phuCapNhanVien.NoiDung,
            };
        }

        /// <summary>
        /// Cập nhật phụ cấp nhân viên
        /// </summary>
        /// <param name="model"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task UpdatePhuCapNhanVienAsync(string id, PhuCapNhanVienCreateModel model, string userId)
        {
            var phuCapNhanVien = _sqlContext.NhanVien_PhuCap.FirstOrDefault(x => x.Id.Equals(id));
            if (phuCapNhanVien == null)
            {
                throw HRMException.CreateInstance("Phụ cấp của nhân viên không tồn tại");
            }
            //var checkName = _sqlContext.NhanVien_PhuCap.AsNoTracking().FirstOrDefault(a => model.Name.ToLower().Trim().Equals(a.Name.ToLower().Trim()) && !a.Id.Equals(id));
            //if (checkName != null)
            //{
            //    throw HRMException.CreateInstance("Tên phụ cấp nhân viên đã tồn tại!");
            //}
            phuCapNhanVien.IdNhanVien = model.IdNhanVien;
            phuCapNhanVien.IdPhuCap = model.IdPhuCap;
            phuCapNhanVien.NoiDung = model.NoiDung;
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
