using OTK.Commands;
using OTK.Infrastructure;
using OTK.Models;
using OTK.Repository;
using OTK.Views;
using OTK.Views.Forms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OTK.ViewModels.Forms
{
    internal class InControlViewModel : FormAbstract
    {
        //private readonly RepositoryMSSQL<Jobs> _repo;

        //public ObservableCollection<Jobs> ListJobs { get; set; }
        //public Jobs SelectedJob { get; set; }



        //--------------------------------------------------------------------------------
        // Конструктор
        //--------------------------------------------------------------------------------
        public InControlViewModel() : base()
        {
            FormType = EnumFormType.VK;
            //_repo = new RepositoryMSSQL<Jobs>();
            //LoadJobs(EnumStatusJob.ReqConfirm);
        }


        //--------------------------------------------------------------------------------
        // Загрузка работ
        //--------------------------------------------------------------------------------
        //public override void LoadJobs(EnumStatusJob status)
        //{
        //    ListJobs = new ObservableCollection<Jobs>(_repo.Items.Where(it => it.JobStatus == status && it.JobType == FormType));
        //    OnPropertyChanged(nameof(ListJobs));
        //}


        //--------------------------------------------------------------------------------
        // Создание формы
        //--------------------------------------------------------------------------------
        public override void CreateForm()
        {
            InControlDetailWindow win = new InControlDetailWindow();
            Jobs job = new Jobs();
            job.JobType = FormType;
            win.DataContext = new InControlDetailWindowViewModel(_repo, job);
            if(win.ShowDialog() == true)
            {
                _repo.Add(job);
                _repo.Save();
                ListJobs.Add(job);
            }
        }


        //--------------------------------------------------------------------------------
        // Открытие формы
        //--------------------------------------------------------------------------------
        public override void OpenForm()
        {
            InControlDetailWindow win = new InControlDetailWindow();
            win.DataContext = new InControlDetailWindowViewModel(_repo, SelectedJob);
            if (win.ShowDialog() == true)
            {
                //_repo.Save();
            }

        }


        #region Команды

        //--------------------------------------------------------------------------------
        // Команда Двойной щелчок
        //--------------------------------------------------------------------------------
        //private readonly ICommand _DblClickCommand = null;
        //public ICommand DblClickCommand => _DblClickCommand ?? new LambdaCommand(OnDblClickCommandExecuted, CanDblClickCommand);
        //private bool CanDblClickCommand(object p) => true;
        //private void OnDblClickCommandExecuted(object p)
        //{
        //    OpenForm();
        //}


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
