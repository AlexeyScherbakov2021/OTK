namespace OTK.Models
{
    using OTK.Infrastructure;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Jobs : IEntity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Jobs()
        {
            Action = new HashSet<Action>();
        }

        [Key]
        [Column("JobId")]
        public int id { get; set; }

        public int? JobType { get; set; }

        public DateTime? JobDate { get; set; }

        public string JobNameProduct { get; set; }

        public int? JobRnoID { get; set; }

        [StringLength(100)]
        public string JobKD { get; set; }

        [StringLength(100)]
        public string JobVendor { get; set; }

        public int JobCountPropduct { get; set; }

        public int JobCountNP { get; set; }

        [StringLength(20)]
        public string JobLocation { get; set; }

        [StringLength(20)]
        public string JobUnitName { get; set; }

        public string JobDescript { get; set; }

        [StringLength(70)]
        public string JobExecutor { get; set; }

        public string JobSolution { get; set; }

        public EnumStatus JobStatus { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Action> Action { get; set; }

        public virtual RnO RnO { get; set; }
    }
}
