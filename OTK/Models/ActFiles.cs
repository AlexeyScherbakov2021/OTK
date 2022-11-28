using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTK.Models
{
    public partial class ActFiles : IEntity
    {
        public int id { get; set; }

        public int? af_JobId { get;  set; }

        public string af_FileName { get; set; }

        public virtual Jobs ListJobs { get; set; }

        [NotMapped]
        public string FullName;

    }
}
