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
    }
}
