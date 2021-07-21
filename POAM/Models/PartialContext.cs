using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POAM.Models
{
    public partial class POAMContext : DbContext
    {


        // call this method on OnModelCreating(ModelBuilder modelBuilder)
        //OnModelCreating2(modelBuilder);

        //public DbQuery<VUserAccountExternalAndInternalListOAsisWithUrlAndCertifiedDate> VoasisUserNames { get; set; }
        public DbSet<VUserAccountExternalAndInternalListOAsisWithUrlAndCertifiedDate> VoasisUserNames { get; set; }


        // public DbQuery<VUserAccountListAdminsOASIS>  vUserAccountListAdminsOASIS { get; set; }
        public DbSet<VUserAccountListAdminsOASIS> vUserAccountListAdminsOASIS { get; set; }


        //public DbQuery<VChangeLog> VChangeLog { get; set; }
        public DbSet<VChangeLog> VChangelog { get; set; }
        
        //entity.Property(e => e.id).ValueGeneratedOnAdd();
        //if you get any identity exception for change log
        partial void OnModelCreating2(ModelBuilder modelBuilder);
        // OnModelCreating2(modelBuilder);

        partial void OnModelCreating2(ModelBuilder modelBuilder)
        {
            //            modelBuilder.Query<VUserAccountExternalAndInternalListOAsisWithUrl>().ToView("VUserAccountExternalAndInternalListOAsisWithUrl");
            // modelBuilder.Query<VUserAccountExternalAndInternalListOAsisWithUrlAndCertifiedDate>().ToView("VUserAccountExternalAndInternalListOAsisWithUrlAndCertifiedDate");
            modelBuilder.Entity<VUserAccountExternalAndInternalListOAsisWithUrlAndCertifiedDate>().HasNoKey();
            //modelBuilder.Query<VUserAccountListAdminsOASIS>().ToView("VUserAccountListAdminsOASIS");
            modelBuilder.Entity<VUserAccountListAdminsOASIS>().HasNoKey();
            // modelBuilder.Query<VChangeLog>().ToView("VChangeLog");
            modelBuilder.Entity<VChangeLog>().HasNoKey();
        }
    }
}

