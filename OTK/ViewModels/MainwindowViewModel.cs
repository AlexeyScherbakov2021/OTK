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


        //--------------------------------------------------------------------------------
        // конструктор
        //--------------------------------------------------------------------------------
        public MainwindowViewModel()
        {
            vmInControl = new InControlViewModel();
        }


        //--------------------------------------------------------------------------------
        // Проверка сроков ответов
        //--------------------------------------------------------------------------------

        private async void CheckDateEndAsync()
        {
            await Task.Run(() =>
            {
                DateTime NowDate = DateTime.Now;
                using (RepositoryMSSQL<Jobs> repoJobs = new RepositoryMSSQL<Jobs>())
                {
                    List<Jobs> ListJobs = repoJobs.Items.ToList();
                    foreach (Jobs job in ListJobs)
                    {
                        foreach (var item in job.Action)
                        {
                            if (item.ActionStatus == EnumStatus.CheckedProcess)
                            {
                                if (NowDate > item.ActionDateEnd.Value.AddDays(1))
                                {
                                    item.ActionStatus = EnumStatus.OverTime;
                                }
                            }
                        }
                    }
                    repoJobs.Save();

                }
                return Task.FromResult(true);

            });
        }



        //--------------------------------------------------------------------------------
        // Загрузка списка работ для текущей вкладки и фильтра
        //--------------------------------------------------------------------------------
        private void LoadListJobs()
        {
            if (SelectedTab.DataContext is FormAbstract form)
                form.LoadListJobs(_CurrentFilter);

            if (SelectedTab.DataContext is RnOViewModel rno)
                rno.LoadRnO(_CurrentFilter);

        }

        #region Команды

        //--------------------------------------------------------------------------------
        // Событие загрузки формы
        //--------------------------------------------------------------------------------
        public ICommand LoadedCommand =>  new LambdaCommand(OnLoadedCommandExecuted, CanLoadedCommand);
        private bool CanLoadedCommand(object p) => true;
        private void OnLoadedCommandExecuted(object p)
        {
            CheckDateEndAsync();

        }


        //--------------------------------------------------------------------------------
        // Команда Фильтровать Требующие рассмотрения
        //--------------------------------------------------------------------------------
        public ICommand FilterNeedCommand => new LambdaCommand(OnFilterNeedCommandExecuted, CanFilterNeedCommand);
        private bool CanFilterNeedCommand(object p) => true;
        private void OnFilterNeedCommandExecuted(object p)
        {
            _CurrentFilter = EnumFilter.Require;
            LoadListJobs();
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

        }

        //--------------------------------------------------------------------------------
        // Команда Создать
        //--------------------------------------------------------------------------------
        private readonly ICommand _CreateCommand = null;
        public ICommand CreateCommand => _CreateCommand ?? new LambdaCommand(OnCreateCommandExecuted, CanCreateCommand);
        private bool CanCreateCommand(object p) => true;
        private void OnCreateCommandExecuted(object p)
        {
            if(SelectedTab.DataContext is FormAbstract form)
            {
                form.CreateForm();
            }

            if (SelectedTab.DataContext is RnOViewModel FormRno)
            {
                FormRno.CreateForm();
            }

        }


        #endregion

    }
}
