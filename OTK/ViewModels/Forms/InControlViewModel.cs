using OTK.Infrastructure;
using OTK.Models;
using OTK.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTK.ViewModels.Forms
{
    internal class InControlViewModel : Observable
    {
        private readonly RepositoryMSSQL<Jobs> _repo;

        public ObservableCollection<Jobs> ListJobs { get; set; }
        public Jobs SelectedJob { get; set; }

        public InControlViewModel()
        {
            _repo = new RepositoryMSSQL<Jobs>();

        }


        private void LoadJobs(EnumStatus status)
        {
            ListJobs = new ObservableCollection<Jobs>(_repo.Items.Where(it => it.JobStatus == status));
            OnPropertyChanged(nameof(ListJobs));

        }

    }
}
