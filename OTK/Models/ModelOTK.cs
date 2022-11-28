using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
using System.Data.Entity;
using System.Linq;

namespace OTK.Models
{
    public partial class ModelOTK : DbContext
    {
        public ModelOTK(string cs)
            : base(cs)
        {
        }

        //public static ModelOTK BaseDB = null;
        public static ModelOTK CreateDB()
        {
            //if (BaseDB != null)
            //    return BaseDB;

            string ConnectString;

#if DEBUG
            ConnectString = ConfigurationManager.ConnectionStrings["ModelOTK"].ConnectionString;
            ConnectString += ";user id=fpLoginName;password=ctcnhjt,s";
#else
            ConnectString = ConfigurationManager.ConnectionStrings["ModelOTK"].ConnectionString;
            ConnectString += ";user id=fpLoginName;password=ctcnhjt,s";
#endif
            ModelOTK BaseDB = new ModelOTK(ConnectString);

            return BaseDB/* = new ModelOTK()*/;
        }


        public virtual DbSet<ActionUser> Action { get; set; }
        public virtual DbSet<Jobs> Jobs { get; set; }
        public virtual DbSet<RnO> RnO { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<ActionFiles> ActionFiles { get; set; }
        public virtual DbSet<ActFiles> ActFiles { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ActionUser>()
                .Property(e => e.ActionName)
                .IsUnicode(false);

            modelBuilder.Entity<ActionUser>()
                .HasMany(e => e.ActionFiles)
                .WithOptional(e => e.ActionUser)
                .HasForeignKey(e => e.af_ActionId);

            modelBuilder.Entity<Jobs>()
                .HasMany(e => e.ActFiles)
                .WithOptional(e => e.ListJobs)
                .HasForeignKey(e => e.af_JobId);

            modelBuilder.Entity<Jobs>()
                .Property(e => e.JobNameProduct)
                .IsUnicode(false);

            modelBuilder.Entity<Jobs>()
                .Property(e => e.JobKD)
                .IsUnicode(false);

            modelBuilder.Entity<Jobs>()
                .Property(e => e.JobVendor)
                .IsUnicode(false);

            modelBuilder.Entity<Jobs>()
                .Property(e => e.JobLocation)
                .IsUnicode(false);

            modelBuilder.Entity<Jobs>()
                .Property(e => e.JobUnitName)
                .IsUnicode(false);

            modelBuilder.Entity<Jobs>()
                .Property(e => e.JobDescript)
                .IsUnicode(false);

            modelBuilder.Entity<Jobs>()
                .Property(e => e.JobExecutor)
                .IsUnicode(false);

            modelBuilder.Entity<Jobs>()
                .Property(e => e.JobSolution)
                .IsUnicode(false);

            modelBuilder.Entity<Jobs>()
                .HasMany(e => e.Action)
                .WithOptional(e => e.Jobs)
                .HasForeignKey(e => e.ActionJobID)
                .WillCascadeOnDelete();

            modelBuilder.Entity<RnO>()
                .Property(e => e.RnoStage)
                .IsUnicode(false);

            modelBuilder.Entity<RnO>()
                .Property(e => e.RnoFakt)
                .IsUnicode(false);

            modelBuilder.Entity<RnO>()
                .Property(e => e.RnoNorma)
                .IsUnicode(false);

            modelBuilder.Entity<RnO>()
                .Property(e => e.RnoNumberPermiss)
                .IsUnicode(false);

            modelBuilder.Entity<RnO>()
                .Property(e => e.RnoTerm)
                .IsUnicode(false);

            modelBuilder.Entity<RnO>()
                .HasMany(e => e.Jobs)
                .WithOptional(e => e.RnO)
                .HasForeignKey(e => e.JobRnoID);

            modelBuilder.Entity<Users>()
                .Property(e => e.UserLogin)
                .IsUnicode(false);

            modelBuilder.Entity<Users>()
                .Property(e => e.UserName)
                .IsUnicode(false);

            modelBuilder.Entity<Users>()
                .Property(e => e.UserPass)
                .IsUnicode(false);

            modelBuilder.Entity<Users>()
                .Property(e => e.UserOtdel)
                .IsUnicode(false);

            modelBuilder.Entity<Users>()
                .Property(e => e.UserEmail)
                .IsUnicode(false);

            modelBuilder.Entity<Users>()
                .HasMany(e => e.Action)
                .WithOptional(e => e.User)
                .HasForeignKey(e => e.ActionUserID);

            modelBuilder.Entity<Users>()
                .HasMany(e => e.RnO)
                .WithOptional(e => e.User)
                .HasForeignKey(e => e.RnoUserID);
        }
    }
}
