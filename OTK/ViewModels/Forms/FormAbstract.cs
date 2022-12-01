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
using System.Windows;
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

        protected string NameForm;

        public bool IsOverTimeAction { get; set; }

        //--------------------------------------------------------------------------------
        // Конструктор
        //--------------------------------------------------------------------------------
        public FormAbstract()
        {
            User = App.CurrentUser;
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

            // получение просроченных форм
            using (RepositoryMSSQL<ActionUser> repo = new RepositoryMSSQL<ActionUser>())
            {
                IsOverTimeAction = repo.Items.Any(it => it.Jobs.JobType == FormType && it.ActionStatus == EnumStatus.OverTime);
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
                                && it.Action.Where(a => a.User.id == UserId 
                                    && (a.ActionStatus == EnumStatus.CheckedProcess
                                    || a.ActionStatus == EnumStatus.OverTime)).Any())
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

            // получение просроченных форм
            using (RepositoryMSSQL<ActionUser> repo = new RepositoryMSSQL<ActionUser>())
            {
                IsOverTimeAction = repo.Items.Any(it => 
                        it.Jobs.JobType == FormType 
                        && it.ActionStatus == EnumStatus.OverTime
                        && it.User.id == UserId);
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
