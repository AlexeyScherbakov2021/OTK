using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace OTK.Models
{
    public partial class ModelOTK : DbContext
    {
        public ModelOTK()
            : base("name=ModelOTK")
        {
        }

        public virtual DbSet<Action> Action { get; set; }
        public virtual DbSet<Jobs> Jobs { get; set; }
        public virtual DbSet<RnO> RnO { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        //public virtual DbSet<Worker> Worker { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Action>()
                .Property(e => e.ActionName)
                .IsUnicode(false);

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

            //modelBuilder.Entity<Worker>()
            //    .Property(e => e.WorkerName)
            //    .IsUnicode(false);

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
