﻿using OTK.Commands;
using OTK.Infrastructure;
using OTK.Models;
using OTK.Repository;
using OTK.Views;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace OTK.ViewModels.Forms
{
    internal class InControlDetailWindowViewModel : Observable
    {
        private readonly RepositoryMSSQL<Jobs> _repo;
        private readonly RepositoryMSSQL<Users> _repoUsers;
        public Users User => App.CurrentUser;
        //private RespUserWindow win;

        public string Title { get; set; } = "форма Входной контроль";

        public List<Users> ListUsers { get; set; }

        public List<string> Status => ClassStatus.NameStatus;

        public Jobs CurrentJob { get; set; }
        public ActionUser SelectedAction { get; set; }

        public ActionUser NewAction { get; set; }

        public Visibility IsCreateVisible { get; set; } = Visibility.Collapsed;
        public Visibility IsEditVisible { get; set; } = Visibility.Visible;

        //--------------------------------------------------------------------------------
        // конструктор для этапа разработки
        //--------------------------------------------------------------------------------
        public InControlDetailWindowViewModel()
        {

        }

        //--------------------------------------------------------------------------------
        // Конструктор
        //--------------------------------------------------------------------------------
        public InControlDetailWindowViewModel(RepositoryMSSQL<Jobs> repo, Jobs job )
        {
            _repo = repo;
            _repoUsers = new RepositoryMSSQL<Users>();
            ListUsers = _repoUsers.Items.OrderBy(o => o.UserName).ToList();

            if (job.id == 0)
            {
                Title = "Создание формы Входной контроль";
                IsEditVisible = Visibility.Collapsed;
                IsCreateVisible= Visibility.Visible;
            }

            CurrentJob = job;

        }


        //--------------------------------------------------------------------------------
        // установка статуса формы
        //--------------------------------------------------------------------------------
        private void SetStatusActionToJob()
        {

            // если есть на проверке 
            if (CurrentJob.Action.Any(it => it.ActionStatus == EnumStatus.Checked))
                CurrentJob.JobStatus = EnumStatusJob.ReqConfirm;

            // если все выполнены
            if (CurrentJob.Action.All(it => it.ActionStatus == EnumStatus.Finish))
                CurrentJob.JobStatus = EnumStatusJob.Complete;


        }



        #region Команды

        //--------------------------------------------------------------------------------
        // Команда Добавить ответственного
        //--------------------------------------------------------------------------------
        public ICommand AddCommand => new LambdaCommand(OnAddCommandExecuted, CanAddCommand);
        private bool CanAddCommand(object p) => true;
        private void OnAddCommandExecuted(object p)
        {
            RespUserWindow win = new RespUserWindow();
            win.DataContext = this;
            NewAction = new ActionUser();
            if(win.ShowDialog() == true)
            {
                CurrentJob.Action.Add(NewAction);
            }

        }

        //--------------------------------------------------------------------------------
        // Команда удалить ответственного
        //--------------------------------------------------------------------------------
        public ICommand DelCommand => new LambdaCommand(OnDelCommandExecuted, CanDelCommand);
        private bool CanDelCommand(object p) => SelectedAction != null;
        private void OnDelCommandExecuted(object p)
        {
            if(MessageBox.Show($"Удалить {SelectedAction.User.UserName}", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                CurrentJob.Action.Remove(SelectedAction);
            }
        }



        //--------------------------------------------------------------------------------
        // Команда Отправить
        //--------------------------------------------------------------------------------
        public ICommand SendCommand => new LambdaCommand(OnSendCommandExecuted, CanSendCommand);
        private bool CanSendCommand(object p) => true;
        private void OnSendCommandExecuted(object p)
        {
            DateTime CurrentTime = DateTime.Now;

            foreach(var item in CurrentJob.Action)
            {
                // прошло часов после последней отправки
                var times = CurrentTime - item.ActionTimeSend;

                // осталось часов до выполнения
                var left = item.ActionDateEnd - CurrentTime;
                bool IsSend = left.Value.TotalHours < 24 && times.Value.TotalHours > 24;

                // если оповещения еще не было или было более суток назад 
                if (item.ActionTimeSend is null || IsSend )
                {
                    SenderToEmail senderEmail = new SenderToEmail(item.User);
                    senderEmail.SendMail("Сообщение");
                    // сохраняем время отправки
                    item.ActionTimeSend = CurrentTime;
                    _repo.Save();
                }
            }

        }

        //--------------------------------------------------------------------------------
        // Команда Проверене
        //--------------------------------------------------------------------------------
        public ICommand AcceptCommand => new LambdaCommand(OnAcceptCommandExecuted, CanAcceptCommand);
        private bool CanAcceptCommand(object p) => SelectedAction?.ActionStatus == EnumStatus.Checked;
        private void OnAcceptCommandExecuted(object p)
        {
            SelectedAction.ActionStatus = EnumStatus.Finish;
            SetStatusActionToJob();
            _repo.Save();
        }

        //--------------------------------------------------------------------------------
        // Команда Отказать
        //--------------------------------------------------------------------------------
        public ICommand RejectCommand => new LambdaCommand(OnRejectCommandExecuted, CanRejectCommand);
        private bool CanRejectCommand(object p) => SelectedAction?.ActionStatus == EnumStatus.Checked;
        private void OnRejectCommandExecuted(object p)
        {
            SelectedAction.ActionStatus = EnumStatus.CheckedProcess;
            SetStatusActionToJob();
            _repo.Save();
            SenderToEmail senderEmail = new SenderToEmail(SelectedAction.User);
            senderEmail.SendMail("Не принято.");
        }


        #endregion


    }
}
