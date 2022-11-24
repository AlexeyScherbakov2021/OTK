using OTK.Commands;
using OTK.Infrastructure;
using OTK.Models;
using OTK.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OTK.ViewModels.Forms
{
    internal abstract class FormAbstract : Observable
    {
        public Users User { get; set; }
        public readonly RepositoryMSSQL<Jobs> _repo;
        public ObservableCollection<Jobs> ListJobs { get; set; }
        public Jobs SelectedJob { get; set; }
        public EnumFormType FormType = EnumFormType.Fail;


        public FormAbstract()
        {
            User = App.CurrentUser;
            _repo = new RepositoryMSSQL<Jobs>();
            LoadJobs(EnumStatusJob.ReqConfirm);
        }


        public void LoadJobs(EnumStatusJob status)
        {
            ListJobs = new ObservableCollection<Jobs>(_repo.Items.Where(it => it.JobStatus == status && it.JobType == FormType));
            OnPropertyChanged(nameof(ListJobs));
        }

        public abstract void CreateForm();
        public abstract void OpenForm();


        private readonly ICommand _DblClickCommand = null;
        public ICommand DblClickCommand => _DblClickCommand ?? new LambdaCommand(OnDblClickCommandExecuted, CanDblClickCommand);
        private bool CanDblClickCommand(object p) => true;
        private void OnDblClickCommandExecuted(object p)
        {
            OpenForm();
        }


    }


}
