using OTK.Commands;
using OTK.Infrastructure;
using OTK.Models;
using OTK.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OTK.ViewModels.Forms
{
    internal abstract class FormAbstract : Observable
    {
        public Users User { get; set; }
        //protected readonly RepositoryMSSQL<Jobs> _repo;
        public ObservableCollection<Jobs> ListJobs { get; set; }
        public Jobs SelectedJob { get; set; }
        public EnumFormType FormType = EnumFormType.Fail;

        private EnumFilter _LastFilter;


        //--------------------------------------------------------------------------------
        // Конструктор
        //--------------------------------------------------------------------------------
        public FormAbstract()
        {
            User = App.CurrentUser;
            //LoadJobs(EnumStatusJob.ReqConfirm);
        }


        //--------------------------------------------------------------------------------
        // Загрузка списка форм
        //--------------------------------------------------------------------------------
        public void RefreshListJobs()
        {
            LoadListJobs(_LastFilter);
        }


        //--------------------------------------------------------------------------------
        // Загрузка списка форм по фильтру
        //--------------------------------------------------------------------------------
        public void LoadListJobs(EnumFilter filter)
        {
            _LastFilter = filter;

            if (User.UserRole == EnumRoles.Пользователь)
                LoadJobsUser(filter, User.id);
            else
                LoadJobs(filter);
        }


        //--------------------------------------------------------------------------------
        // Загрузка работ для управления
        //--------------------------------------------------------------------------------
        public void LoadJobs(EnumFilter filter)
        {
            using (RepositoryMSSQL<Jobs> repo = new RepositoryMSSQL<Jobs>())
            {
                switch (filter)
                {
                    case EnumFilter.Require:
                        ListJobs = new ObservableCollection<Jobs>(repo.Items.Where(it => it.JobStatus == EnumStatusJob.ReqConfirm
                            && it.JobType == FormType));
                        break;

                    case EnumFilter.Works:
                        ListJobs = new ObservableCollection<Jobs>(repo.Items.Where(it =>
                            (it.JobStatus != EnumStatusJob.Closed)
                            && it.JobType == FormType));
                        break;

                    case EnumFilter.Closed:
                        ListJobs = new ObservableCollection<Jobs>(repo.Items.Where(it => it.JobStatus == EnumStatusJob.Closed
                            && it.JobType == FormType));
                        break;

                }
            }

            OnPropertyChanged(nameof(ListJobs));
        }

        //--------------------------------------------------------------------------------
        // Загрузка работ для пользователя
        //--------------------------------------------------------------------------------
        public void LoadJobsUser(EnumFilter filter, int UserId)
        {
            using (RepositoryMSSQL<Jobs> repo = new RepositoryMSSQL<Jobs>())
            {
                switch (filter)
                {
                    case EnumFilter.Require:
                        ListJobs = new ObservableCollection<Jobs>(repo.Items
                            .Where(it => it.JobType == FormType
                                && it.Action.Where(a => a.User.id == UserId && a.ActionStatus == EnumStatus.CheckedProcess).Any())
                            );
                        break;

                    case EnumFilter.Works:
                        ListJobs = new ObservableCollection<Jobs>(repo.Items
                            .Where(it => (it.JobStatus != EnumStatusJob.Closed)
                                    && it.JobType == FormType
                                    && it.Action.Where(a => a.User.id == UserId).Any())
                            );
                        break;

                    case EnumFilter.Closed:
                        ListJobs = new ObservableCollection<Jobs>(repo.Items
                            .Where(it => it.JobStatus == EnumStatusJob.Closed
                            && it.JobType == FormType
                            && it.Action.Where(a => a.User.id == UserId).Any())
                            );
                        break;
                }
            }

            OnPropertyChanged(nameof(ListJobs));
        }


        //--------------------------------------------------------------------------------
        // Создание формы
        //--------------------------------------------------------------------------------
        public abstract void CreateForm();

        //--------------------------------------------------------------------------------
        // Открытие формы
        //--------------------------------------------------------------------------------
        public abstract void OpenForm();


        //--------------------------------------------------------------------------------
        // Команда Двойной щелчок
        //--------------------------------------------------------------------------------
        private readonly ICommand _DblClickCommand = null;
        public ICommand DblClickCommand => _DblClickCommand ?? new LambdaCommand(OnDblClickCommandExecuted, CanDblClickCommand);
        private bool CanDblClickCommand(object p) => SelectedJob != null;
        private void OnDblClickCommandExecuted(object p)
        {
            OpenForm();
        }

    }


}
