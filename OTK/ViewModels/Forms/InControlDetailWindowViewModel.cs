using OTK.Infrastructure;
using OTK.Models;
using OTK.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTK.ViewModels.Forms
{
    internal class InControlDetailWindowViewModel : Observable
    {
        private readonly RepositoryMSSQL<Jobs> _repo;



        public InControlDetailWindowViewModel()
        {

        }

        public InControlDetailWindowViewModel(RepositoryMSSQL<Jobs> repo)
        {
            _repo= repo;
        }
    }
}
