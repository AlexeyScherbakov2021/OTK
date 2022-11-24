using OTK.Commands;
using OTK.Infrastructure;
using OTK.Models;
using OTK.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OTK.ViewModels.Forms
{
    internal class InControlUserWindowViewModel : Observable
    {
        private readonly RepositoryMSSQL<Jobs> _repoJob;
        public Users User => App.CurrentUser;

        public Jobs CurrentJob { get; set; }

        public ActionUser CurrentActor { get; set; }

        //--------------------------------------------------------------------------------
        // Конструктор
        //--------------------------------------------------------------------------------
        public InControlUserWindowViewModel(RepositoryMSSQL<Jobs> repo, Jobs job)
        {
            _repoJob= repo;
            CurrentJob= job;
            CurrentActor = job.Action.FirstOrDefault(it => it.User.id == User.id);
        }

        public InControlUserWindowViewModel()
        {

        }


        #region Команды

        //--------------------------------------------------------------------------------
        // Команда Выполнено
        //--------------------------------------------------------------------------------
        public ICommand CommitCommand => new LambdaCommand(OnCommitCommandExecuted, CanCommitCommand);
        private bool CanCommitCommand(object p) => true;
        private void OnCommitCommandExecuted(object p)
        {
            CurrentActor.ActionStatus = EnumStatus.Checked;
            CurrentActor.Jobs.JobStatus = EnumStatusJob.ReqConfirm;
            _repoJob.Save();
        }

        #endregion

    }
}
