namespace OTK.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Action")]
    public partial class Action
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ActionID { get; set; }

        public int? ActionJobID { get; set; }

        public string ActionName { get; set; }

        public int? ActionUserID { get; set; }

        public DateTime? ActionDateEnd { get; set; }

        public DateTime? ActionDateReal { get; set; }

        public int? ActionStatus { get; set; }

        public virtual Jobs Jobs { get; set; }

        public virtual Users User { get; set; }
    }
}
