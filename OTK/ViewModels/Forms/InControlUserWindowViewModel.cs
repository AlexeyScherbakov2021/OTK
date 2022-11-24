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

        public Jobs CurrentJob { get; set; }

        //--------------------------------------------------------------------------------
        // Конструктор
        //--------------------------------------------------------------------------------
        public InControlUserWindowViewModel(RepositoryMSSQL<Jobs> repo, Jobs job)
        {
            _repoJob= repo;
            CurrentJob= job;
        }


        #region Команды

        //--------------------------------------------------------------------------------
        // Команда Выполнено
        //--------------------------------------------------------------------------------
        public ICommand CommitCommand => new LambdaCommand(OnCommitCommandExecuted, CanCommitCommand);
        private bool CanCommitCommand(object p) => true;
        private void OnCommitCommandExecuted(object p)
        {

        }

        #endregion

    }
}
