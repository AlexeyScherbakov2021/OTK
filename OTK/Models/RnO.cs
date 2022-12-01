namespace OTK.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RnO")]
    public partial class RnO : IEntity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public RnO()
        {
            Jobs = new HashSet<Jobs>();
        }

        //[DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        //[Column("RnoID")]
        public int id { get; set; }

        public DateTime RnoDate { get; set; }

        public int? RnoUserID { get; set; }

        [StringLength(50)]
        public string RnoStage { get; set; }

        public string RnoItem { get; set; }

        public string RnoFakt { get; set; }

        public string RnoNorma { get; set; }

        [StringLength(10)]
        public string RnoNumberPermiss { get; set; }

        public int? RnoCountProd { get; set; }

        public string RnoTerm { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Jobs> Jobs { get; set; }

        public virtual Users User { get; set; }
    }
}
