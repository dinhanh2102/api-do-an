using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace HRM.Models.Entities
{
    public partial class CoreProjectContext : DbContext
    {
        public CoreProjectContext()
        {
        }

        public CoreProjectContext(DbContextOptions<CoreProjectContext> options)
            : base(options)
        {
        }

        public virtual DbSet<BangCap> BangCap { get; set; }
        public virtual DbSet<BangCong> BangCong { get; set; }
        public virtual DbSet<BangLuong> BangLuong { get; set; }
        public virtual DbSet<BaoHiem> BaoHiem { get; set; }
        public virtual DbSet<ChucDanh> ChucDanh { get; set; }
        public virtual DbSet<ChuyenNganhHoc> ChuyenNganhHoc { get; set; }
        public virtual DbSet<DonVi> DonVi { get; set; }
        public virtual DbSet<Function> Function { get; set; }
        public virtual DbSet<HopDongLaoDong> HopDongLaoDong { get; set; }
        public virtual DbSet<KhenThuong_KyLuat> KhenThuong_KyLuat { get; set; }
        public virtual DbSet<KyCong> KyCong { get; set; }
        public virtual DbSet<KyCong_NhanVien> KyCong_NhanVien { get; set; }
        public virtual DbSet<LoaiCa> LoaiCa { get; set; }
        public virtual DbSet<LoaiCong> LoaiCong { get; set; }
        public virtual DbSet<MenuSystem> MenuSystem { get; set; }
        public virtual DbSet<MenuSystemPermission> MenuSystemPermission { get; set; }
        public virtual DbSet<NgachLuong> NgachLuong { get; set; }
        public virtual DbSet<NganHang> NganHang { get; set; }
        public virtual DbSet<NhanVien> NhanVien { get; set; }
        public virtual DbSet<NhanVien_GiaDinh> NhanVien_GiaDinh { get; set; }
        public virtual DbSet<NhanVien_PhuCap> NhanVien_PhuCap { get; set; }
        public virtual DbSet<NhanVien_TangCa> NhanVien_TangCa { get; set; }
        public virtual DbSet<NhanVien_TruongHoc> NhanVien_TruongHoc { get; set; }
        public virtual DbSet<PhongBan> PhongBan { get; set; }
        public virtual DbSet<PhuCap> PhuCap { get; set; }
        public virtual DbSet<RefreshToken> RefreshToken { get; set; }
        public virtual DbSet<SystemParams> SystemParams { get; set; }
        public virtual DbSet<TaiLieuDinhKem> TaiLieuDinhKem { get; set; }
        public virtual DbSet<TangCa> TangCa { get; set; }
        public virtual DbSet<TrinhDo> TrinhDo { get; set; }
        public virtual DbSet<TruongHoc> TruongHoc { get; set; }
        public virtual DbSet<UngLuong> UngLuong { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserGroup> UserGroup { get; set; }
        public virtual DbSet<UserGroupFunction> UserGroupFunction { get; set; }
        public virtual DbSet<UserPermission> UserPermission { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=14.248.84.128,1445;Database=HRM2AI;User Id=qlnshh;Password=123@qlns;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<BangCap>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.ChungChi).HasMaxLength(500);

                entity.Property(e => e.HinhThucDaoTao).HasMaxLength(500);

                entity.Property(e => e.IdNhanVien)
                    .IsRequired()
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.MoTa).HasMaxLength(500);

                entity.Property(e => e.NgayBatDau).HasColumnType("datetime");

                entity.Property(e => e.NgayKetThuc).HasColumnType("datetime");

                entity.Property(e => e.TenBangCap)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.TenChuyenNganh)
                    .IsRequired()
                    .HasMaxLength(500);
            });

            modelBuilder.Entity<BangCong>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.IdKyCong)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.IdLoaiCong)
                    .IsRequired()
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.MaNhanVien)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ThoiGianRa).HasColumnType("datetime");

                entity.Property(e => e.ThoiGianVao).HasColumnType("datetime");
            });

            modelBuilder.Entity<BangLuong>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.IdKyCong)
                    .IsRequired()
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.MaNhanVien)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TenChucDanh).HasMaxLength(500);

                entity.Property(e => e.TenNhanVien).HasMaxLength(500);
            });

            modelBuilder.Entity<BaoHiem>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.NgayCap).HasColumnType("datetime");

                entity.Property(e => e.NoiCap).HasMaxLength(500);

                entity.Property(e => e.NoiKhamBenh)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.SoBH)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ChucDanh>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.IdPhongBan)
                    .IsRequired()
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.TenChucDanh)
                    .IsRequired()
                    .HasMaxLength(500);
            });

            modelBuilder.Entity<ChuyenNganhHoc>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.IdTruongHoc)
                    .IsRequired()
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.TenChuyenNganh)
                    .IsRequired()
                    .HasMaxLength(500);
            });

            modelBuilder.Entity<DonVi>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.MaDonVi)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.ParentId)
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.TenDonVi)
                    .IsRequired()
                    .HasMaxLength(500);
            });

            modelBuilder.Entity<Function>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(500);
            });

            modelBuilder.Entity<HopDongLaoDong>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.IdNgachLuong)
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.IdNhanVien)
                    .IsRequired()
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.NgayCoHieuLuc).HasColumnType("datetime");

                entity.Property(e => e.NgayHetHan).HasMaxLength(100);

                entity.Property(e => e.NgayKy).HasColumnType("datetime");

                entity.Property(e => e.SoHopDong)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.TenHopDong)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.ThoiHanHopDong).HasMaxLength(200);
            });

            modelBuilder.Entity<KhenThuong_KyLuat>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.IdNhanVien)
                    .IsRequired()
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.Ngay).HasColumnType("datetime");

                entity.Property(e => e.NoiDung)
                    .IsRequired()
                    .HasMaxLength(500);
            });

            modelBuilder.Entity<KyCong>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.MaKyCong)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.NgayTinhCong).HasColumnType("datetime");
            });

            modelBuilder.Entity<KyCong_NhanVien>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.IdKyCong)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MaNhanVien)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<LoaiCa>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(500);
            });

            modelBuilder.Entity<LoaiCong>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(36)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<MenuSystem>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.Icon).HasMaxLength(500);

                entity.Property(e => e.ParentId)
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.TitleDefault)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.Url).HasMaxLength(500);
            });

            modelBuilder.Entity<MenuSystemPermission>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.FunctionId)
                    .IsRequired()
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.MenuSystemId)
                    .IsRequired()
                    .HasMaxLength(36)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<NgachLuong>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.HeSo)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.TenNgach)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<NganHang>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.MaNganHang)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.TenNganHang)
                    .IsRequired()
                    .HasMaxLength(500);
            });

            modelBuilder.Entity<NhanVien>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.Avatar).HasMaxLength(500);

                entity.Property(e => e.CanCuocCongDan)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.CreateBy)
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.DOB).HasColumnType("datetime");

                entity.Property(e => e.DiaChi)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.IdBaoHiem)
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.IdChucDanh)
                    .IsRequired()
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.IdTrinhDo)
                    .IsRequired()
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.MaNhanVien)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SoDienThoai)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.SoTKNganHang).HasMaxLength(500);

                entity.Property(e => e.TenNhanVien)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.UpdateBy)
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(36)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<NhanVien_GiaDinh>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.HoVaTen)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.IdNhanVien)
                    .IsRequired()
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.MoiQuanHe)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.NamSinh)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.NgheNghiep)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.QueQuan)
                    .IsRequired()
                    .HasMaxLength(500);
            });

            modelBuilder.Entity<NhanVien_PhuCap>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.IdNhanVien)
                    .IsRequired()
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.IdPhuCap)
                    .IsRequired()
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.NoiDung).HasMaxLength(500);
            });

            modelBuilder.Entity<NhanVien_TangCa>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.IdKyCong)
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.IdNhanVien)
                    .IsRequired()
                    .HasMaxLength(36)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<NhanVien_TruongHoc>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.IdChuyenNganh)
                    .IsRequired()
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.IdNhanVien)
                    .IsRequired()
                    .HasMaxLength(36)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<PhongBan>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.IdDonVi)
                    .IsRequired()
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.TenPhongBan)
                    .IsRequired()
                    .HasMaxLength(500);
            });

            modelBuilder.Entity<PhuCap>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(500);
            });

            modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.ExpireAt).HasColumnType("datetime");

                entity.Property(e => e.IssueAt).HasColumnType("datetime");

                entity.Property(e => e.Token)
                    .IsRequired()
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(36)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<SystemParams>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.DisplayName).HasMaxLength(500);

                entity.Property(e => e.ParamName)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.ParamValue).HasMaxLength(500);
            });

            modelBuilder.Entity<TaiLieuDinhKem>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.CreateBy)
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Extention)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.FileName)
                    .IsRequired()
                    .HasMaxLength(300);

                entity.Property(e => e.FilePath)
                    .IsRequired()
                    .HasMaxLength(300);

                entity.Property(e => e.HashValue)
                    .IsRequired()
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.ObjectId)
                    .IsRequired()
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.Thumbnail).HasMaxLength(300);

                entity.Property(e => e.UpdateBy)
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<TangCa>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.IdLoaiCa)
                    .IsRequired()
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.IdNhanVien)
                    .IsRequired()
                    .HasMaxLength(36)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TrinhDo>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(500);
            });

            modelBuilder.Entity<TruongHoc>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.MaTruong)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.TenTruongHoc)
                    .IsRequired()
                    .HasMaxLength(500);
            });

            modelBuilder.Entity<UngLuong>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.IdNhanVien)
                    .IsRequired()
                    .HasMaxLength(36)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.Avatar).HasMaxLength(500);

                entity.Property(e => e.CreateBy)
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.FullName).HasMaxLength(500);

                entity.Property(e => e.LockoutEndateUtc).HasColumnType("datetime");

                entity.Property(e => e.PasswordHash).IsRequired();

                entity.Property(e => e.SecurityStamp).IsRequired();

                entity.Property(e => e.UpdateBy)
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.Property(e => e.UserGroupId)
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(256);
            });

            modelBuilder.Entity<UserGroup>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.CreateBy)
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.UpdateBy)
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<UserGroupFunction>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.FunctionId)
                    .IsRequired()
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.UserGroupId)
                    .IsRequired()
                    .HasMaxLength(36)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<UserPermission>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.FunctionId)
                    .IsRequired()
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(36)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
