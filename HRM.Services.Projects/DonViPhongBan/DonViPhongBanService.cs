using HRM.Common;
using HRM.Common.Models;
using HRM.Models.Entities;
using HRM.Models.Projects.DonViPhongBan;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRM.Services.Projects.DonViPhongBan
{
    public class DonViPhongBanService : IDonViPhongBanService
    {
        private readonly CoreProjectContext _sqlContext;
        public DonViPhongBanService(CoreProjectContext sqlContext)
        {
            this._sqlContext = sqlContext;
        }

        /// <summary>
        /// Tìm kiếm đơn vị
        /// </summary>
        /// <returns></returns>
        public async Task<SearchBaseResultModel<DonViSearchModel>> SearchDonViAsync()//string name
        {
            SearchBaseResultModel<DonViSearchModel> result = new SearchBaseResultModel<DonViSearchModel>();
            var dataQuery = _sqlContext.DonVi.AsNoTracking()
                            .Select(s => new DonViSearchModel
                            {
                                Id = s.Id,
                                MaDonVi = s.MaDonVi,
                                TenDonVi = s.TenDonVi,
                                ParentId = s.ParentId,
                            }).AsQueryable();

            result.TotalItems = dataQuery.Count();
            var listDonVi = dataQuery.ToList();
            result.DataResults = listDonVi.Where(e => string.IsNullOrEmpty(e.ParentId)).ToList();
            foreach (var item in result.DataResults)
            {
                item.Children = GetSubDonVi(item, listDonVi);
            }
            return result;
        }

        private List<DonViSearchModel> GetSubDonVi(DonViSearchModel parentDonVi, List<DonViSearchModel> listDonVi)
        {
            var children = listDonVi.Where(e => e.ParentId == parentDonVi.Id).ToList();
            foreach (var item in children)
            {
                item.Children = GetSubDonVi(item, listDonVi);
            }
            return children;
        }

        /// <summary>
        /// Tạo mới đơn vị
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task CreateDonViAsync(DonViCreateModel model, string userId)
        {
            var checkName = _sqlContext.DonVi.AsNoTracking().FirstOrDefault(a => a.MaDonVi.Equals(model.MaDonVi));
            if (checkName != null)
            {
                throw HRMException.CreateInstance("Tên đơn vị đã tồn tại!");
            }

            DonVi donVi = new DonVi()
            {
                Id = Guid.NewGuid().ToString(),
                MaDonVi = model.MaDonVi,
                TenDonVi = model.TenDonVi,
                ParentId = model.ParentId,
            };
            _sqlContext.DonVi.Add(donVi);

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
        /// Xoá đơn vị
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteDonViAsync(string id)
        {
            var listDonVi = _sqlContext.DonVi.ToList();
            var listPhongBan = _sqlContext.PhongBan.AsNoTracking().ToList();
            var donVi = _sqlContext.DonVi.FirstOrDefault(e => id.Equals(e.Id));
            if (donVi == null)
            {
                throw HRMException.CreateInstance("Đơn vị không tồn tại");
            }
            var listDonViXoa = GetDonVi(donVi, listDonVi);
            listDonViXoa.Add(donVi);
            foreach (var item in listDonViXoa)
            {
                var checkPhongBan = listPhongBan.FirstOrDefault(e => e.IdDonVi.Equals(item.Id));
                if (checkPhongBan != null)
                {
                    throw HRMException.CreateInstance("Đơn vị đang được phòng ban sử dụng, không thể xoá!");
                }
            }
            _sqlContext.DonVi.RemoveRange(listDonViXoa);
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

        private List<DonVi> GetDonVi(DonVi parentDonVi, List<DonVi> donViList)
        {
            var children = donViList.Where(a => parentDonVi.Id.Equals(a.ParentId)).ToList();
            List<DonVi> donViCon = new List<DonVi>();
            foreach (var item in children)
            {
                donViCon.Add(item);
                donViCon.AddRange(GetDonVi(item, donViList));
            }
            donViCon = donViCon.Distinct().ToList();
            return donViCon;
        }

        /// <summary>
        /// Lấy đơn vị theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<DonViSearchModel> GetDonViByIdAsync(string id)
        {
            var donVi = _sqlContext.DonVi.AsNoTracking().FirstOrDefault(x => x.Id.Equals(id));
            if (donVi == null)
            {
                throw HRMException.CreateInstance("Đơn vị không tồn tại");
            }
            return new DonViSearchModel()
            {
                Id = donVi.Id,
                MaDonVi = donVi.MaDonVi,
                TenDonVi = donVi.TenDonVi,
                ParentId = donVi.ParentId,
            };
        }

        /// <summary>
        /// Cập nhật đơn vị
        /// </summary>
        /// <param name="model"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task UpdateDonViAsync(string id, DonViCreateModel model, string userId)
        {
            var donVi = _sqlContext.DonVi.FirstOrDefault(x => x.Id.Equals(id));
            if (donVi == null)
            {
                throw HRMException.CreateInstance("Đơn vị không tồn tại");
            }
            var checkName = _sqlContext.DonVi.AsNoTracking().FirstOrDefault(a => a.MaDonVi.Equals(model.MaDonVi) && !a.Id.Equals(id));
            if (checkName != null)
            {
                throw HRMException.CreateInstance("Tên đơn vị đã tồn tại!");
            }

            donVi.MaDonVi = model.MaDonVi;
            donVi.TenDonVi = model.TenDonVi;
            donVi.ParentId = model.ParentId;

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
        /// Lấy danh sách đơn vị theo id đơn vị
        /// </summary>
        /// <param name="idDonVi"></param>
        /// <returns></returns>
        public async Task<List<DonViSearchModel>> GetComboboxDonVi(string idDonVi)
        {
            List<DonViSearchModel> listCombobox = new List<DonViSearchModel>();
            try
            {
                listCombobox = _sqlContext.DonVi.AsNoTracking().OrderBy(r => r.TenDonVi).Select(s => new DonViSearchModel()
                {
                    Id = s.Id,
                    MaDonVi = s.MaDonVi,
                    TenDonVi = s.TenDonVi,
                    ParentId = s.ParentId,
                }).ToList();
                if (!string.IsNullOrEmpty(idDonVi))
                {
                    var donVi = listCombobox.FirstOrDefault(e => idDonVi.Equals(e.Id));
                    var subDonVi = GetSubDonVi(donVi, listCombobox);
                    foreach (var item in subDonVi)
                    {
                        listCombobox.Remove(item);
                        foreach (var itm in item.Children)
                            listCombobox.Remove(itm);
                    }
                    listCombobox.Remove(donVi);
                }
                return listCombobox;
            }
            catch { return listCombobox; }
        }

        /// <summary>
        /// Tìm kiếm phòng ban
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        public async Task<SearchBaseResultModel<PhongBanSearchResultModel>> SearchPhongBanAsync(PhongBanSearchModel searchModel)
        {
            SearchBaseResultModel<PhongBanSearchResultModel> result = new SearchBaseResultModel<PhongBanSearchResultModel>();
            var dataQuery = (from a in _sqlContext.PhongBan.AsNoTracking()
                             join b in _sqlContext.DonVi.AsNoTracking() on a.IdDonVi equals b.Id
                             select new PhongBanSearchResultModel
                             {
                                 Id = a.Id,
                                 TenPhongBan = a.TenPhongBan,
                                 IdDonVi = a.IdDonVi,
                                 TenDonVi = b.TenDonVi,
                             }).AsQueryable();
            if (!string.IsNullOrEmpty(searchModel.TenPhongBan))
            {
                dataQuery = dataQuery.Where(x => x.TenPhongBan.Contains(searchModel.TenPhongBan));
            }
            if (!string.IsNullOrEmpty(searchModel.IdDonVi))
            {
                List<DonVi> listDonVi = _sqlContext.DonVi.AsNoTracking().ToList();
                var allDonViCon = GetDonViCon(searchModel.IdDonVi, listDonVi);
                allDonViCon.Add(searchModel.IdDonVi);
                dataQuery = dataQuery.Where(x => allDonViCon.Contains(x.IdDonVi));
            }
            result.TotalItems = dataQuery.Count();
            result.DataResults = dataQuery.OrderBy(o => o.TenPhongBan).Skip((searchModel.PageNumber - 1) * searchModel.PageSize).Take(searchModel.PageSize).ToList();
            return result;
        }

        private List<string> GetDonViCon(string idDonVi, List<DonVi> donVi)
        {
            var children = donVi.Where(x => x.ParentId == idDonVi).Select(a => a.Id).ToList();
            var allChildIds = new List<string>(children);
            foreach (var item in children)
            {
                var granditems = GetDonViCon(item, donVi);
                allChildIds.AddRange(granditems);
            }
            return allChildIds;
        }

        /// <summary>
        /// Thêm mới phòng ban
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task CreatePhongBanAsync(PhongBanCreateModel model, string userId)
        {
            var checkName = _sqlContext.PhongBan.AsNoTracking().Where(a => model.IdDonVi.Equals(a.IdDonVi) && model.TenPhongBan.ToLower().Trim().Equals(a.TenPhongBan.ToLower().Trim())).FirstOrDefault();
            if (checkName != null)
            {
                throw HRMException.CreateInstance("Tên phòng ban đã tồn tại");
            }

            PhongBan phongBan = new PhongBan()
            {
                Id = Guid.NewGuid().ToString(),
                IdDonVi = model.IdDonVi,
                TenPhongBan = model.TenPhongBan,
                MoTa = model.MoTa,
            };
            _sqlContext.PhongBan.Add(phongBan);

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
        /// Xoá phòng ban
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeletePhongBanAsync(string id)
        {
            var phongBan = _sqlContext.PhongBan.FirstOrDefault(e => id.Equals(e.Id));
            if (phongBan == null)
            {
                throw HRMException.CreateInstance("Phòng ban không tồn tại");
            }
            //var checkNhanVien = _sqlContext.NhanVien.AsNoTracking().FirstOrDefault(e => e.IdPhongBan.Equals(id));
            //if (checkNhanVien != null)
            //{
                //throw HRMException.CreateInstance("");
            //}
            _sqlContext.PhongBan.Remove(phongBan);
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
        /// Lấy phòng ban theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<PhongBanSearchResultModel> GetPhongBanByIdAsync(string id)
        {
            var phongBan = _sqlContext.PhongBan.AsNoTracking().FirstOrDefault(x => x.Id.Equals(id));
            if (phongBan == null)
            {
                throw HRMException.CreateInstance("Phòng ban không tồn tại");
            }
            return new PhongBanSearchResultModel()
            {
                Id = phongBan.Id,
                IdDonVi = phongBan.IdDonVi,
                TenPhongBan = phongBan.TenPhongBan,
                MoTa = phongBan.MoTa,
            };
        }

        /// <summary>
        /// Cập nhật phòng ban
        /// </summary>
        /// <param name="model"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task UpdatePhongBanAsync(string id, PhongBanCreateModel model, string userId)
        {
            var phongBan = _sqlContext.PhongBan.FirstOrDefault(x => x.Id.Equals(id));
            if (phongBan == null)
            {
                throw HRMException.CreateInstance("Phòng ban không tồn tại");
            }
            phongBan.IdDonVi = model.IdDonVi;
            phongBan.TenPhongBan = model.TenPhongBan;
            phongBan.MoTa = model.MoTa;

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
