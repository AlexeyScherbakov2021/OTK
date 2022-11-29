using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTK.Models
{
    public partial class ActionFiles : IAddFiles, IEntity
    {
        public int id { get; set; }

        [Column("af_ActionId")]
        public int? idParent { get; set; }

        [Column("af_FileName")]
        public string FileName { get; set; }

        public virtual ActionUser ActionUser { get; set; }

        [NotMapped]
        public string FullName { get; set; }

    }
}
