using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

namespace POAM.Models
{
    public partial class POAMContext : DbContext
    {
        public POAMContext()
        {
        }

        public POAMContext(DbContextOptions<POAMContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ApplicationNames> ApplicationNames { get; set; }
        public virtual DbSet<CertifiedUserList> CertifiedUserList { get; set; }
        public virtual DbSet<ChangeLog> ChangeLog { get; set; }
        public virtual DbSet<Office> Office { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                //  optionsBuilder.UseSqlServer("Server=ftasqldev,4173;Database=POAM;Trusted_Connection=True;");

                IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();
                optionsBuilder.UseSqlServer(configuration.GetConnectionString("POAMDatabase"));


            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            OnModelCreating2(modelBuilder);
            modelBuilder.Entity<ApplicationNames>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Abbreviation).HasMaxLength(550);

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Url).HasColumnName("URL");
            });

            modelBuilder.Entity<CertifiedUserList>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AccessLevelName).HasMaxLength(2500);

                entity.Property(e => e.ApplicantionNamesId).HasColumnName("ApplicantionNamesID");

                entity.Property(e => e.ChangeLogId).HasColumnName("ChangeLogID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.EMail)
                    .HasColumnName("E_Mail")
                    .HasMaxLength(500);

                entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");

                entity.Property(e => e.FirstName).HasMaxLength(500);

                entity.Property(e => e.LastLoginDate).HasColumnType("datetime");

                entity.Property(e => e.LastName).HasMaxLength(500);

                entity.Property(e => e.OfficeId).HasColumnName("OfficeID");

                entity.Property(e => e.OfficeName).HasMaxLength(50);
            });

            modelBuilder.Entity<ChangeLog>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ApplicantionNamesId).HasColumnName("ApplicantionNamesID");

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Office>(entity =>
            {
                entity.Property(e => e.OfficeId)
                    .HasColumnName("OfficeID")
                    .ValueGeneratedNever();

                entity.Property(e => e.IsParentOffice).HasColumnName("isParentOffice");

                entity.Property(e => e.MainOfficeId).HasColumnName("MainOfficeID");

                entity.Property(e => e.OfficeName)
                    .IsRequired()
                    .HasMaxLength(120)
                    .IsUnicode(false);

                entity.Property(e => e.OrgCode).HasMaxLength(500);

                entity.Property(e => e.RouteSym)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Title).HasMaxLength(500);

                entity.Property(e => e.Updated).HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy).HasColumnName("updatedBy");
            });
        }
    }
}
