namespace OTK.Models
{
    using OTK.Infrastructure;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Action")]
    public partial class ActionUser : IEntity
    {
        public ActionUser()
        {
            ActionFiles = new HashSet<ActionFiles>();
        }

        public int id { get; set; }

        public int? ActionJobID { get; set; }

        public string ActionName { get; set; }

        public int? ActionUserID { get; set; }

        public DateTime? ActionDateEnd { get; set; }

        public DateTime? ActionDateReal { get; set; }

        public EnumStatus ActionStatus { get; set; }

        public DateTime? ActionTimeSend { get; set; }

        public virtual Jobs Jobs { get; set; }

        public virtual Users User { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ActionFiles> ActionFiles { get; set; }
    }
}
