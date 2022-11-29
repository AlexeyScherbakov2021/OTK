using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTK.Models
{
    public interface IAddFiles
    {
        int? idParent { get; set; }

        string FileName { get; set; }

        string FullName { get; set; }


    }
}
