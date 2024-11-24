using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Project.Data;

public partial class ProjectContext : DbContext
{
    public ProjectContext()
    {
    }

    public ProjectContext(DbContextOptions<ProjectContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BoNhoTrong> BoNhoTrongs { get; set; }

    public virtual DbSet<CtHdBanHang> CtHdBanHangs { get; set; }

    public virtual DbSet<DienThoai> DienThoais { get; set; }

    public virtual DbSet<GioHang> GioHangs { get; set; }

    public virtual DbSet<HdBanHang> HdBanHangs { get; set; }

    public virtual DbSet<Mau> Maus { get; set; }

    public virtual DbSet<PhanQuyen> PhanQuyens { get; set; }

    public virtual DbSet<Ram> Rams { get; set; }

    public virtual DbSet<TaiKhoan> TaiKhoans { get; set; }

    public virtual DbSet<ThuongHieu> ThuongHieus { get; set; }

    public virtual DbSet<TrangThai> TrangThais { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("Data Source=USERMIC-MRQM1HQ;Initial Catalog=Project;Integrated Security=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BoNhoTrong>(entity =>
        {
            entity.HasKey(e => e.MaBoNhoTrong).HasName("PK__BoNhoTro__82A1DC2957893EC6");

            entity.ToTable("BoNhoTrong");

            entity.Property(e => e.MaBoNhoTrong).HasMaxLength(50);
            entity.Property(e => e.DungLuong).HasMaxLength(50);
        });

        modelBuilder.Entity<CtHdBanHang>(entity =>
        {
            // Define composite primary key for CT_HD_BanHang (MaHd, MaSp)
            entity.HasKey(e => new { e.MaHd, e.MaSp });

            entity.ToTable("CT_HD_BanHang");

            // Define properties for MaHd and MaSp
            entity.Property(e => e.MaHd)
                .HasMaxLength(50)
                .HasColumnName("MaHD");

            entity.Property(e => e.MaSp)
                .HasMaxLength(50)
                .HasColumnName("MaSP");

            // Define property for Gia
            entity.Property(e => e.Gia)
                .HasColumnType("decimal(18, 2)");

            // Define relationships for MaHdNavigation (HD_BanHang) and MaSpNavigation (DienThoai)
            entity.HasOne(d => d.MaHdNavigation)
                .WithMany(p => p.CtHdBanHangs)
                .HasForeignKey(d => d.MaHd)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CT_HD_BanH__MaHD__5535A963");

            entity.HasOne(d => d.MaSpNavigation)
                .WithMany(p => p.CtHdBanHangs)
                .HasForeignKey(d => d.MaSp)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CT_HD_BanH__MaSP__5629CD9C");
        });


        modelBuilder.Entity<DienThoai>(entity =>
        {
            entity.HasKey(e => e.MaSp).HasName("PK__DienThoa__2725081C24082BB8");

            entity.ToTable("DienThoai");

            entity.Property(e => e.MaSp)
                .HasMaxLength(50)
                .HasColumnName("MaSP");
            entity.Property(e => e.AnhThongSo).HasMaxLength(255);
            entity.Property(e => e.CameraSau).HasMaxLength(100);
            entity.Property(e => e.CameraTruoc).HasMaxLength(100);
            entity.Property(e => e.ChatLieu).HasMaxLength(500);
            entity.Property(e => e.Cpu)
                .HasMaxLength(100)
                .HasColumnName("CPU");
            entity.Property(e => e.GiaCu).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.GiaMoi).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.HeDieuHanh).HasMaxLength(100);
            entity.Property(e => e.HinhAnh).HasMaxLength(255);
            entity.Property(e => e.Ktkl)
                .HasMaxLength(500)
                .HasColumnName("KTKL");
            entity.Property(e => e.MaBoNhoTrong).HasMaxLength(50);
            entity.Property(e => e.MaMau).HasMaxLength(50);
            entity.Property(e => e.MaRam).HasMaxLength(50);
            entity.Property(e => e.MaThuongHieu).HasMaxLength(50);
            entity.Property(e => e.ManHinh).HasMaxLength(100);
            entity.Property(e => e.MoTa).HasMaxLength(1000);
            entity.Property(e => e.Pin).HasMaxLength(100);
            entity.Property(e => e.Sl).HasColumnName("SL");
            entity.Property(e => e.Tdrm)
                .HasMaxLength(100)
                .HasColumnName("TDRM");
            entity.Property(e => e.TenSp)
                .HasMaxLength(100)
                .HasColumnName("TenSP");

            entity.HasOne(d => d.MaBoNhoTrongNavigation).WithMany(p => p.DienThoais)
                .HasForeignKey(d => d.MaBoNhoTrong)
                .HasConstraintName("FK_DienThoai_BoNhoTrong");

            entity.HasOne(d => d.MaMauNavigation).WithMany(p => p.DienThoais)
                .HasForeignKey(d => d.MaMau)
                .HasConstraintName("FK_DienThoai_Mau");

            entity.HasOne(d => d.MaRamNavigation).WithMany(p => p.DienThoais)
                .HasForeignKey(d => d.MaRam)
                .HasConstraintName("FK_DienThoai_Ram");

            entity.HasOne(d => d.MaThuongHieuNavigation).WithMany(p => p.DienThoais)
                .HasForeignKey(d => d.MaThuongHieu)
                .HasConstraintName("FK_DienThoai_ThuongHieu");
        });

        modelBuilder.Entity<GioHang>(entity =>
        {
            entity.HasKey(e => new { e.MaTaiKhoan, e.MaSp }).HasName("PK__GioHang__7F0E35A8D2A4EBE9");

            entity.ToTable("GioHang");

            entity.Property(e => e.MaTaiKhoan).HasMaxLength(50);
            entity.Property(e => e.MaSp)
                .HasMaxLength(50)
                .HasColumnName("MaSP");
            entity.Property(e => e.Gia).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.MaSpNavigation).WithMany(p => p.GioHangs)
                .HasForeignKey(d => d.MaSp)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__GioHang__MaSP__4CA06362");

            entity.HasOne(d => d.MaTaiKhoanNavigation).WithMany(p => p.GioHangs)
                .HasForeignKey(d => d.MaTaiKhoan)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__GioHang__MaTaiKh__4BAC3F29");
        });

        modelBuilder.Entity<HdBanHang>(entity =>
        {
            // Define primary key for HD_BanHang
            entity.HasKey(e => e.MaHd).HasName("PK__HD_BanHa__2725A6E0C17FE413");

            entity.ToTable("HD_BanHang");

            // Define properties for HD_BanHang
            entity.Property(e => e.MaHd)
                .HasMaxLength(50)
                .HasColumnName("MaHD");

            entity.Property(e => e.DiaChi)
                .HasMaxLength(255);

            entity.Property(e => e.MaTaiKhoan)
                .HasMaxLength(50);

            entity.Property(e => e.MaTrangThai)
                .HasMaxLength(50);

            entity.Property(e => e.NgayBan)
                .HasColumnType("datetime");

            entity.Property(e => e.Sdt)
                .HasMaxLength(15)
                .HasColumnName("SDT");

            entity.Property(e => e.Ten)
                .HasMaxLength(100);

            entity.Property(e => e.TongTien)
                .HasColumnType("decimal(18, 2)");

            // Define relationships for MaTaiKhoanNavigation and MaTrangThaiNavigation
            entity.HasOne(d => d.MaTaiKhoanNavigation)
                .WithMany(p => p.HdBanHangs)
                .HasForeignKey(d => d.MaTaiKhoan)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__HD_BanHan__MaTai__5165187F");

            entity.HasOne(d => d.MaTrangThaiNavigation)
                .WithMany(p => p.HdBanHangs)
                .HasForeignKey(d => d.MaTrangThai)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__HD_BanHan__MaTra__52593CB8");
        });


        modelBuilder.Entity<Mau>(entity =>
        {
            entity.HasKey(e => e.MaMau).HasName("PK__Mau__3A5BBB7DB05BD6E5");

            entity.ToTable("Mau");

            entity.Property(e => e.MaMau).HasMaxLength(50);
            entity.Property(e => e.TenMau).HasMaxLength(50);
        });

        modelBuilder.Entity<PhanQuyen>(entity =>
        {
            entity.HasKey(e => e.MaQuyen).HasName("PK__PhanQuye__1D4B7ED492C99B5C");

            entity.ToTable("PhanQuyen");

            entity.Property(e => e.MaQuyen).ValueGeneratedNever();
            entity.Property(e => e.TenQuyen).HasMaxLength(50);
        });

        modelBuilder.Entity<Ram>(entity =>
        {
            entity.HasKey(e => e.MaRam).HasName("PK__Ram__3961207C19239F3E");

            entity.ToTable("Ram");

            entity.Property(e => e.MaRam).HasMaxLength(50);
            entity.Property(e => e.DungLuong).HasMaxLength(50);
        });

        modelBuilder.Entity<TaiKhoan>(entity =>
        {
            entity.HasKey(e => e.MaTaiKhoan).HasName("PK__TaiKhoan__AD7C652990AB26C9");

            entity.ToTable("TaiKhoan");

            entity.Property(e => e.MaTaiKhoan).HasMaxLength(50);
            entity.Property(e => e.DiaChi).HasMaxLength(255);
            entity.Property(e => e.MatKhau).HasMaxLength(255);
            entity.Property(e => e.Sdt)
                .HasMaxLength(15)
                .HasColumnName("SDT");
            entity.Property(e => e.Ten).HasMaxLength(100);

            entity.HasOne(d => d.MaQuyenNavigation).WithMany(p => p.TaiKhoans)
                .HasForeignKey(d => d.MaQuyen)
                .HasConstraintName("FK_TaiKhoan_PhanQuyen");
        });

        modelBuilder.Entity<ThuongHieu>(entity =>
        {
            entity.HasKey(e => e.MaThuongHieu).HasName("PK__ThuongHi__A3733E2C04EF4457");

            entity.ToTable("ThuongHieu");

            entity.Property(e => e.MaThuongHieu).HasMaxLength(50);
            entity.Property(e => e.TenThuongHieu).HasMaxLength(100);
        });

        modelBuilder.Entity<TrangThai>(entity =>
        {
            entity.HasKey(e => e.MaTrangThai).HasName("PK__TrangTha__AADE413873FEBEE5");

            entity.ToTable("TrangThai");

            entity.Property(e => e.MaTrangThai).HasMaxLength(50);
            entity.Property(e => e.TenTrangThai).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
