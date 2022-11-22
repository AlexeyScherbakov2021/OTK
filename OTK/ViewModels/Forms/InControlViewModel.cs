using OTK.Commands;
using OTK.Infrastructure;
using OTK.Models;
using OTK.Repository;
using OTK.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

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


        public void CreateForm()
        {
            InControlDetailWindow win = new InControlDetailWindow();
            win.DataContext = new InControlDetailWindowViewModel(_repo);
            win.ShowDialog();
        }


        #region Команды

        //--------------------------------------------------------------------------------
        // Команда Создать
        //--------------------------------------------------------------------------------
        //private readonly ICommand _CreateCommand = null;
        //public ICommand CreateCommand => _CreateCommand ?? new LambdaCommand(OnCreateCommandExecuted, CanCreateCommand);
        //private bool CanCreateCommand(object p) => true;
        //private void OnCreateCommandExecuted(object p)
        //{


        //}

        #endregion


    }
}
