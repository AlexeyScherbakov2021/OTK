using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTK.Models
{
    public partial class ActFiles : IAddFiles, IEntity
    {
        public int id { get; set; }

        [Column("af_JobId")]
        public int? idParent { get;  set; }

        [Column("af_FileName")]
        public string FileName { get; set; }

        public virtual Jobs ListJobs { get; set; }

        [NotMapped]
        public string FullName { get; set; }

    }
}
