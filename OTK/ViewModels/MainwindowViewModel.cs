using OTK.Commands;
using OTK.Infrastructure;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using OTK.ViewModels.Forms;
using OTK.Models;
using OTK.Repository;

namespace OTK.ViewModels
{
    internal class MainwindowViewModel : Observable
    {
#if DEBUG
        public string Title { get; set; } = "Список задач (отладочная версия)";
#else
        public string Title { get; set; } = "Список задач";
#endif
        private EnumFilter _CurrentFilter;

        public Users User => App.CurrentUser;

        public InControlViewModel vmInControl { get; set; }

        private TabItem _SelectedTab;
        public TabItem SelectedTab 
        { 
            get => _SelectedTab;
            set 
            { 
                if(Set(ref _SelectedTab, value))
                    LoadListJobs();
            } 
        }


        public MainwindowViewModel()
        {
            CheckDateEnd();

            vmInControl = new InControlViewModel();
        }


        //--------------------------------------------------------------------------------
        // Проверка сроков ответов
        //--------------------------------------------------------------------------------
        private void CheckDateEnd()
        {
            DateTime NowDate = DateTime.Now;
            RepositoryMSSQL<Jobs> repoJobs = new RepositoryMSSQL<Jobs>();
            List<Jobs> ListJobs = repoJobs.Items.ToList();

            foreach(Jobs job in ListJobs)
            {
                foreach(var item in job.Action)
                {
                    if(item.ActionStatus == EnumStatus.CheckedProcess)
                    {
                        if(NowDate > item.ActionDateEnd )
                        {
                            item.ActionStatus = EnumStatus.CheckedNone;
                        }
                    }
                }
            }
            repoJobs.Save();
        }


        //--------------------------------------------------------------------------------
        // Загрузка списка работ для текущей вкладки и фильтра
        //--------------------------------------------------------------------------------
        private void LoadListJobs()
        {
            if (SelectedTab.DataContext is FormAbstract form)
                form.LoadListJobs(_CurrentFilter);

        }


        #region Команды

        //--------------------------------------------------------------------------------
        // Команда Фильтровать Требующие рассмотрения
        //--------------------------------------------------------------------------------
        public ICommand FilterNeedCommand => new LambdaCommand(OnFilterNeedCommandExecuted, CanFilterNeedCommand);
        private bool CanFilterNeedCommand(object p) => true;
        private void OnFilterNeedCommandExecuted(object p)
        {
            _CurrentFilter = EnumFilter.Require;
            LoadListJobs();

            //if (SelectedTab.DataContext is FormAbstract form)
            //    form.LoadJobs(_CurrentFilter);

            //if (SelectedTab.DataContext != null)
            //    (SelectedTab.DataContext as FormAbstract).LoadJobs(EnumStatusJob.ReqConfirm);

        }

        //--------------------------------------------------------------------------------
        // Команда Фильтровать Закрытые
        //--------------------------------------------------------------------------------
        public ICommand FilterClosedCommand => new LambdaCommand(OnFilterClosedCommandExecuted, CanFilterClosedCommand);
        private bool CanFilterClosedCommand(object p) => true;
        private void OnFilterClosedCommandExecuted(object p)
        {
            _CurrentFilter = EnumFilter.Closed;
            LoadListJobs();
            //if (SelectedTab.DataContext is FormAbstract form)
            //    form.LoadJobs(_CurrentFilter);

            //if (SelectedTab.DataContext != null)
            //    (SelectedTab.DataContext as FormAbstract).LoadJobs(EnumStatusJob.Closed);

        }

        //--------------------------------------------------------------------------------
        // Команда Фильтровать В работе
        //--------------------------------------------------------------------------------
        public ICommand FilterWorkCommand => new LambdaCommand(OnFilterWorkCommandExecuted, CanFilterWorkCommand);
        private bool CanFilterWorkCommand(object p) => true;
        private void OnFilterWorkCommandExecuted(object p)
        {
            _CurrentFilter = EnumFilter.Works;
            LoadListJobs();

            //if ( SelectedTab.DataContext is FormAbstract form)
            //    form.LoadJobs(_CurrentFilter);
        }


        //--------------------------------------------------------------------------------
        // Команда Удалить заказ
        //--------------------------------------------------------------------------------
        //private readonly ICommand _DeleteCommand = null;
        //public ICommand DeleteCommand => _DeleteCommand ?? new LambdaCommand(OnDeleteCommandExecuted, CanDeleteCommand);
        //private bool CanDeleteCommand(object p) => true;
        //private void OnDeleteCommandExecuted(object p)
        //{

        //}

        //--------------------------------------------------------------------------------
        // Команда Отчет
        //--------------------------------------------------------------------------------
        //private readonly ICommand _ReportCommand = null;
        //public ICommand ReportCommand => _ReportCommand ?? new LambdaCommand(OnReportCommandExecuted, CaReportCommand);
        //private bool CaReportCommand(object p) => true;
        //private void OnReportCommandExecuted(object p)
        //{
        //}


        //--------------------------------------------------------------------------------
        // Команда Двойной щелчок
        //--------------------------------------------------------------------------------
        //private readonly ICommand _DblClickCommand = null;
        //public ICommand DblClickCommand => _DblClickCommand ?? new LambdaCommand(OnDblClickCommandExecuted, CanDblClickCommand);
        //private bool CanDblClickCommand(object p) => true;
        //private void OnDblClickCommandExecuted(object p)
        //{
        //    //SelectedTab.DataContext = new InControlViewModel();
        //    (SelectedTab.DataContext as InControlViewModel).OpenForm();
        //}

        //--------------------------------------------------------------------------------
        // Команда Создать
        //--------------------------------------------------------------------------------
        private readonly ICommand _CreateCommand = null;
        public ICommand CreateCommand => _CreateCommand ?? new LambdaCommand(OnCreateCommandExecuted, CanCreateCommand);
        private bool CanCreateCommand(object p) => true;
        private void OnCreateCommandExecuted(object p)
        {
            (SelectedTab.DataContext as FormAbstract).CreateForm();
        }


        #endregion

    }
}
