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
using System.Windows;
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
            LoadListJobs(EnumFilter.Require);
        }


        //--------------------------------------------------------------------------------
        // Загрузка работ
        //--------------------------------------------------------------------------------
        //public override void LoadJobs(EnumFilter filter)
        //{
        //    switch (filter)
        //    {
        //        case EnumFilter.Require:
        //            ListJobs = new ObservableCollection<Jobs>(_repo.Items.Where(it => it.JobStatus == EnumStatusJob.ReqConfirm && it.JobType == FormType));
        //            break;

        //        case EnumFilter.Works:
        //            ListJobs = new ObservableCollection<Jobs>(_repo.Items.Where(it =>
        //                (it.JobStatus == EnumStatusJob.InWork
        //                || it.JobStatus == EnumStatusJob.Complete
        //                || it.JobStatus == EnumStatusJob.ReqConfirm)
        //                && it.JobType == FormType));
        //            break;

        //        case EnumFilter.Closed:
        //            ListJobs = new ObservableCollection<Jobs>(_repo.Items.Where(it => it.JobStatus != EnumStatusJob.Closed
        //                && it.JobType == FormType));
        //            break;

        //    }

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
                _repo.Add(job, true);
                //_repo.Save();
                ListJobs.Add(job);

                // отправить оповещения для всех
                foreach (var item in job.Action)
                {
                    SenderToEmail senderEmail = new SenderToEmail(item.User);
                    senderEmail.SendMail("Создана форма.");
                }
            }
        }


        //--------------------------------------------------------------------------------
        // Открытие формы
        //--------------------------------------------------------------------------------
        public override void OpenForm()
        {
            Window win = new InControlDetailWindow();
            if (User.UserRole == EnumRoles.Пользователь)
            {
                win = new InControlUserWindow();
                win.DataContext = new InControlUserWindowViewModel(_repo, SelectedJob);
            }
            else
            {
                win = new InControlDetailWindow();
                win.DataContext = new InControlDetailWindowViewModel(_repo, SelectedJob);
            }
            if (win.ShowDialog() == true)
            {
                //_repo.Save();
            }
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
