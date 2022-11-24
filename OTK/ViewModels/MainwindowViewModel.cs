﻿using OTK.Commands;
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

        public InControlViewModel vmInControl { get; set; }

        public TabItem SelectedTab { get; set; }


        public MainwindowViewModel()
        {
            vmInControl = new InControlViewModel();
        }


        #region Команды

        //--------------------------------------------------------------------------------
        // Команда Фильтровать Требующие рассмотрения
        //--------------------------------------------------------------------------------
        public ICommand FilterNeedCommand => new LambdaCommand(OnFilterNeedCommandExecuted, CanFilterNeedCommand);
        private bool CanFilterNeedCommand(object p) => true;
        private void OnFilterNeedCommandExecuted(object p)
        {
            if (SelectedTab.DataContext != null)
                (SelectedTab.DataContext as FormAbstract).LoadJobs(EnumStatusJob.ReqConfirm);

        }

        //--------------------------------------------------------------------------------
        // Команда Фильтровать Закрытые
        //--------------------------------------------------------------------------------
        public ICommand FilterClosedCommand => new LambdaCommand(OnFilterClosedCommandExecuted, CanFilterClosedCommand);
        private bool CanFilterClosedCommand(object p) => true;
        private void OnFilterClosedCommandExecuted(object p)
        {
            if (SelectedTab.DataContext != null)
                (SelectedTab.DataContext as FormAbstract).LoadJobs(EnumStatusJob.Closed);

        }

        //--------------------------------------------------------------------------------
        // Команда Фильтровать В работе
        //--------------------------------------------------------------------------------
        public ICommand FilterWorkCommand => new LambdaCommand(OnFilterWorkCommandExecuted, CanFilterWorkCommand);
        private bool CanFilterWorkCommand(object p) => true;
        private void OnFilterWorkCommandExecuted(object p)
        {
            if (SelectedTab.DataContext != null)
                (SelectedTab.DataContext as FormAbstract).LoadJobs(EnumStatusJob.InWork);
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
        private readonly ICommand _DblClickCommand = null;
        public ICommand DblClickCommand => _DblClickCommand ?? new LambdaCommand(OnDblClickCommandExecuted, CanDblClickCommand);
        private bool CanDblClickCommand(object p) => true;
        private void OnDblClickCommandExecuted(object p)
        {
            //SelectedTab.DataContext = new InControlViewModel();
            (SelectedTab.DataContext as InControlViewModel).OpenForm();
        }

        //--------------------------------------------------------------------------------
        // Команда Создать
        //--------------------------------------------------------------------------------
        private readonly ICommand _CreateCommand = null;
        public ICommand CreateCommand => _CreateCommand ?? new LambdaCommand(OnCreateCommandExecuted, CanCreateCommand);
        private bool CanCreateCommand(object p) => true;
        private void OnCreateCommandExecuted(object p)
        {
            //SelectedTab.DataContext = new InControlViewModel();
            (SelectedTab.DataContext as InControlViewModel).CreateForm();
        }


        #endregion

    }
}
