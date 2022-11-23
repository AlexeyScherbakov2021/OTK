namespace OTK.Models
{
    using OTK.Infrastructure;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Users : IEntity
    {
        public Users()
        {
            Action = new HashSet<ActionUser>();
            RnO = new HashSet<RnO>();
        }

        public int id { get; set; }

        [StringLength(20)]
        public string UserLogin { get; set; }

        [StringLength(50)]
        public string UserName { get; set; }

        public EnumRoles UserRole { get; set; }

        [StringLength(15)]
        public string UserPass { get; set; }

        [StringLength(50)]
        public string UserOtdel { get; set; }

        [StringLength(50)]
        public string UserEmail { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ActionUser> Action { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RnO> RnO { get; set; }

    }
}
