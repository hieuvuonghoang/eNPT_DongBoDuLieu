using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace eNPT_DongBoDuLieu.Models.DataBases.EVNNPT
{
    public partial class ModelContext : DbContext
    {
        public ModelContext()
        {
        }

        public ModelContext(DbContextOptions<ModelContext> options)
            : base(options)
        {
        }

        public virtual DbSet<EN_COTDIEN> EN_COTDIENs { get; set; }
        public virtual DbSet<EN_DAYDAN> EN_DAYDANs { get; set; }
        public virtual DbSet<EN_DUONGDAY> EN_DUONGDAYs { get; set; }
        public virtual DbSet<EN_DUONGDAY_COT> EN_DUONGDAY_COTs { get; set; }
        public virtual DbSet<EN_FULLTEXTSEARCH> EN_FULLTEXTSEARCHes { get; set; }
        public virtual DbSet<EN_TRAMBIENAP> EN_TRAMBIENAPs { get; set; }
        public virtual DbSet<EN_TRAMBIENAP_DUONGDAY> EN_TRAMBIENAP_DUONGDAYs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("EVNNPT")
                .HasAnnotation("Relational:Collation", "USING_NLS_COMP");

            modelBuilder.Entity<EN_COTDIEN>(entity =>
            {
                entity.HasKey(e => e.MA_COT)
                    .HasName("EN_COTDIEN_PK");

                entity.ToTable("EN_COTDIEN");

                entity.Property(e => e.MA_COT)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CAPDA)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CHIEUCAO).HasColumnType("NUMBER(38)");

                entity.Property(e => e.CONGDUNGCOT)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.HUYEN)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.KEMONG)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.LAT).HasColumnType("FLOAT");

                entity.Property(e => e.LOAI_BLNM)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.LOAI_MC)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.LOAI_TD)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.LONG_).HasColumnType("FLOAT");

                entity.Property(e => e.MACT)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.MADVQL)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.MAKVHC)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.MATIEPDIA)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.MATTDKV)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.MAVITRI)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.MA_DCS)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.OBJECTID).HasColumnType("NUMBER(38)");

                entity.Property(e => e.SOHUU)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.SOMACH_DCS).HasColumnType("NUMBER(38)");

                entity.Property(e => e.SOMACH_DD).HasColumnType("NUMBER(38)");

                entity.Property(e => e.TENCT)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.TENVITRI)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.TEN_COT)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.TEN_TTD)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.THUTU_PHA)
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.TINH)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.TRONGLUONG).HasColumnType("FLOAT");

                entity.Property(e => e.XA)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<EN_DAYDAN>(entity =>
            {
                entity.HasKey(e => e.MA_DAY)
                    .HasName("EN_DAYDAN_PK");

                entity.ToTable("EN_DAYDAN");

                entity.HasIndex(e => new { e.MA_COT, e.MA_DAY }, "EN_DAYDAN_COT_IDX");

                entity.Property(e => e.MA_DAY)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CAUTAO_CD)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.CHIEUDAI_DD).HasColumnType("FLOAT");

                entity.Property(e => e.CHIEUDAI_DRCD).HasColumnType("FLOAT");

                entity.Property(e => e.DK_DAY).HasColumnType("FLOAT");

                entity.Property(e => e.DK_LOI).HasColumnType("FLOAT");

                entity.Property(e => e.DONGDIEN_DM).HasColumnType("FLOAT");

                entity.Property(e => e.DUONGDAY)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.GHICHU)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.HANG_SX)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.HANG_SX_CD)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.KYHIEU)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LOAI_CD)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MADUONGDAY)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.MADVQL)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.MATTDKV)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.MA_CD)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MA_COT)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.MA_DCS)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.NAM_VH).HasColumnType("NUMBER(38)");

                entity.Property(e => e.NGAY_SX)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.NUOC_SX)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.NUOC_SX_CD)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.SOLUONG).HasColumnType("NUMBER(38)");

                entity.Property(e => e.SOLUONG_KDV).HasColumnType("NUMBER(38)");

                entity.Property(e => e.SOLUONG_MN).HasColumnType("NUMBER(38)");

                entity.Property(e => e.SOLUONG_TBCB).HasColumnType("NUMBER(38)");

                entity.Property(e => e.TEN_CONGTY)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.TEN_DAY)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.TEN_TTD)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.VITRIDAT)
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<EN_DUONGDAY>(entity =>
            {
                entity.HasKey(e => e.MADUONGDAY)
                    .HasName("EN_CONGTRINH_PK");

                entity.ToTable("EN_DUONGDAY");

                entity.Property(e => e.MADUONGDAY)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CAPDA)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.CAUTAO_CD)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CHIEUDAI_DD).HasColumnType("FLOAT");

                entity.Property(e => e.CHIEUDAI_DRCD).HasColumnType("FLOAT");

                entity.Property(e => e.DENTRAM)
                    .HasMaxLength(106)
                    .IsUnicode(false);

                entity.Property(e => e.DK_DAY).HasColumnType("FLOAT");

                entity.Property(e => e.DK_LOI).HasColumnType("FLOAT");

                entity.Property(e => e.DONGDIEN_DM).HasColumnType("FLOAT");

                entity.Property(e => e.DUONGDAY)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.GHICHU)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.HANG_SX)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.HANG_SX_CD)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.KYHIEU)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.LOAI_CD)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.MADUONGDAYCHINH)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.MADVQL)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.MATTDKV)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.MA_CD)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.MA_DCS)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.NAM_VH).HasColumnType("DATE");

                entity.Property(e => e.NUOC_SX)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.NUOC_SX_CD)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.OBJECTID).HasColumnType("NUMBER(38)");

                entity.Property(e => e.SOLUONG).HasColumnType("NUMBER(38)");

                entity.Property(e => e.SOLUONG_KDV).HasColumnType("NUMBER(38)");

                entity.Property(e => e.SOLUONG_MN).HasColumnType("NUMBER(38)");

                entity.Property(e => e.SOLUONG_TBCB).HasColumnType("NUMBER(38)");

                entity.Property(e => e.TENDUONGDAY)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.TENDUONGDAYCHINH)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.TEN_CONGTY)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.TEN_TTD)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.TUTRAM)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.VITRIDAT)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<EN_DUONGDAY_COT>(entity =>
            {
                entity.HasKey(e => new { e.MADUONGDAY, e.MA_COT })
                    .HasName("EN_CONGTRINH_COT_PK");

                entity.ToTable("EN_DUONGDAY_COT");

                entity.Property(e => e.MADUONGDAY)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.MA_COT)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.MADVQL)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.MATTDKV)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.MAVITRI)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.STT).HasColumnType("NUMBER(38)");

                entity.Property(e => e.TEN_TTD)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<EN_FULLTEXTSEARCH>(entity =>
            {
                entity.HasKey(e => e.FTSID);

                entity.ToTable("EN_FULLTEXTSEARCH");

                entity.HasIndex(e => e.DATA, "EN_FULLTEXTSEARCH_IDX");

                entity.HasIndex(e => e.MADVQL, "EN_FULLTEXTSEARCH_MADV_IDX");

                entity.HasIndex(e => new { e.LOAIDT, e.OBJECTID }, "EN_FULLTEXTSEARCH_UPDATE_IDX");

                entity.Property(e => e.FTSID)
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.DATA).HasColumnType("CLOB");

                entity.Property(e => e.LOAIDT)
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.MADT)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.MADVQL)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.OBJECTID)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.TENDT)
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<EN_TRAMBIENAP>(entity =>
            {
                entity.HasKey(e => e.MATRAM)
                    .HasName("EN_TRAMBIENAP_PK");

                entity.ToTable("EN_TRAMBIENAP");

                entity.Property(e => e.MATRAM)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CAPDA)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.HUYEN)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.KIEU_TRAM)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.LAT).HasColumnType("FLOAT");

                entity.Property(e => e.LOAI_TRAM)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.LONG_).HasColumnType("FLOAT");

                entity.Property(e => e.MADVQL)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.MAKVHC)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.MATTDKV)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.NAM_VH).HasColumnType("NUMBER(38)");

                entity.Property(e => e.OBJECTID).HasColumnType("NUMBER(38)");

                entity.Property(e => e.SOHUU)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.TEN_CONGTY)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.TEN_TRAM)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.TEN_TTD)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.TINH)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.XA)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<EN_TRAMBIENAP_DUONGDAY>(entity =>
            {
                entity.HasKey(e => new { e.MATRAM, e.MADUONGDAY })
                    .HasName("EN_TRAMBIENAP_DUONGDAY_PK");

                entity.ToTable("EN_TRAMBIENAP_DUONGDAY");

                entity.Property(e => e.MATRAM)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.MADUONGDAY)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.MADVQL)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.MATTDKV)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.TEN_TTD)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
