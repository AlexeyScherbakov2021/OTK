using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTK.Models
{
    [Table("Vendor")]
    public partial class Vendor : IEntity
    {
        public Vendor()
        {
            ListJob = new HashSet<Jobs>();
        }

        public int id { get; set; }
        public string VendName { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Jobs> ListJob { get; set; }
    }
}
