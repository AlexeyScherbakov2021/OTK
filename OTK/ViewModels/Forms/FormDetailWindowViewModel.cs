using Microsoft.Win32;
using OTK.Commands;
using OTK.Infrastructure;
using OTK.Models;
using OTK.Repository;
using OTK.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;
using Microsoft.SqlServer.Server;

namespace OTK.ViewModels.Forms
{
    internal class FormDetailWindowViewModel : Observable
    {
        private readonly RepositoryMSSQL<Jobs> _repo;
        private readonly RepositoryMSSQL<Users> _repoUsers;
        public Users User => App.CurrentUser;

        //private string NameForm = "";
        public UserControl FormUC { get; set; }

        public string Title { get; set; }

        public List<Users> ListUsers { get; set; }
        public IEnumerable<Users> ListUsersRestrict { get; set; }

        public List<string> Status => ClassStatus.NameStatus;

        public Jobs CurrentJob { get; set; }
        public ActionUser SelectedAction { get; set; }

        public ActionUser NewAction { get; set; }

        public Visibility IsCreateVisible { get; set; }
        public Visibility IsEditVisible { get; set; }
        public Visibility IsCloseVisible { get; set; }

        public bool IsReadOnlyField { get; set; }

        public bool IsEnabledButton { get; set; } = true;
        //public ObservableCollection<ActFiles> ListActFiles { get; set; }

        public AttachListFiles<ActFiles> FilesAct { get; set; }



        //--------------------------------------------------------------------------------
        // конструктор для этапа разработки
        //--------------------------------------------------------------------------------
        public FormDetailWindowViewModel()
        {
        }

        //--------------------------------------------------------------------------------
        // Конструктор
        //--------------------------------------------------------------------------------
        public FormDetailWindowViewModel(Jobs job, string Name, UserControl form)
        {

            FormUC = form;

            _repo = new RepositoryMSSQL<Jobs>();
            _repoUsers = new RepositoryMSSQL<Users>(_repo.GetDB());
            ListUsers = _repoUsers.Items.OrderBy(o => o.UserName).ToList();

            if (job.id == 0)    // если это было создание
            {
                Title = "Создание формы " + Name;
                IsEditVisible = Visibility.Collapsed;
                IsCreateVisible = Visibility.Visible;
                IsCloseVisible = Visibility.Collapsed;
                IsReadOnlyField = false;
                IsEnabledButton = true;
                CurrentJob = job;
            }
            else
            {
                Title = "Форма " + Name;
                CurrentJob = _repo.Get(job.id);
                IsReadOnlyField = true;
                IsEnabledButton = false;
                IsCloseVisible = Visibility.Visible;
                IsCreateVisible = Visibility.Collapsed;

                IsEditVisible = CurrentJob.JobStatus == EnumStatusJob.Closed
                    ? IsEditVisible = Visibility.Collapsed
                    : IsEditVisible = Visibility.Visible;
            }

            FilesAct = new AttachListFiles<ActFiles>(CurrentJob.JobDate.Year,  "Job");
            FilesAct.AssignFiles(CurrentJob.ActFiles);

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
            else if (CurrentJob.Action.All(it => it.ActionStatus == EnumStatus.Finish))
                CurrentJob.JobStatus = EnumStatusJob.Complete;

            else
                CurrentJob.JobStatus = EnumStatusJob.InWork;
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
            ListUsersRestrict = ListUsers.Where(it => it.UserRole == EnumRoles.Пользователь);
            win.DataContext = this;
            NewAction = new ActionUser();
            if (win.ShowDialog() == true)
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
            if (MessageBox.Show($"Удалить {SelectedAction.User.UserName}", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
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

            foreach (var item in CurrentJob.Action)
            {
                // прошло часов после последней отправки
                var times = CurrentTime - item.ActionTimeSend;

                // осталось часов до выполнения
                var left = item.ActionDateEnd - CurrentTime;

                bool IsSend = (left.Value.TotalHours < 24 && times.Value.TotalHours > 24);

                // если оповещения еще не было или было более суток назад 
                if (item.ActionTimeSend is null || IsSend)
                {
                    SenderToEmail senderEmail = new SenderToEmail(item.User);
                    senderEmail.SendMail("Сообщение");

                    // если отправка была успешна
                    // сохраняем время отправки
                    item.ActionTimeSend = CurrentTime;
                    _repo.Save();
                }
            }
        }

        //--------------------------------------------------------------------------------
        // Команда Проверено
        //--------------------------------------------------------------------------------
        public ICommand AcceptCommand => new LambdaCommand(OnAcceptCommandExecuted, CanAcceptCommand);
        private bool CanAcceptCommand(object p) => SelectedAction?.ActionStatus == EnumStatus.Checked;
        private void OnAcceptCommandExecuted(object p)
        {
            SelectedAction.ActionStatus = EnumStatus.Finish;
            SetStatusActionToJob();
            _repo.Save();
            SelectedAction.OnPropertyChanged(nameof(SelectedAction.ActionStatus));
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

            SelectedAction.OnPropertyChanged(nameof(SelectedAction.ActionStatus));

        }

        //--------------------------------------------------------------------------------
        // Команда В архив
        //--------------------------------------------------------------------------------
        public ICommand ArchiveCommand => new LambdaCommand(OnArchiveCommandExecuted, CanArchiveCommand);
        private bool CanArchiveCommand(object p) => CurrentJob?.JobStatus == EnumStatusJob.Complete && p is Window;
        private void OnArchiveCommandExecuted(object p)
        {
            if (MessageBox.Show($"Перевести форму в архив?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                CurrentJob.JobStatus = EnumStatusJob.Closed;
                _repo.Save();
                //Window win = App.Current.Windows.OfType<InControlDetailWindow>().First();

                var win = (Window)p;
                win.DialogResult = true;
            }
        }

        //--------------------------------------------------------------------------------
        // Команда Открыть файл пользователя двойным щелчком
        //--------------------------------------------------------------------------------
        public ICommand OpenFileCommand => new LambdaCommand(OnOpenFileCommandExecuted, CanOpenFileCommand);
        private bool CanOpenFileCommand(object p) => true;
        private void OnOpenFileCommandExecuted(object p)
        {

            if (p is ActionFiles af)
            {
                AttachListFiles<ActionFiles> FilesAction = new AttachListFiles<ActionFiles>(CurrentJob.JobDate.Year, /*af.idParent,*/ "Action");
                FilesAction.StartFile(af);
            }

        }


        //--------------------------------------------------------------------------------
        // Команда Обзор файлов актов
        //--------------------------------------------------------------------------------
        public ICommand BrowseActsCommand => new LambdaCommand(OnBrowseActsCommandExecuted, CanBrowseActsCommand);
        private bool CanBrowseActsCommand(object p) => true;
        private void OnBrowseActsCommandExecuted(object p)
        {
            OpenFileDialog dlgOpen = new OpenFileDialog();
            dlgOpen.Multiselect = true;

            if (dlgOpen.ShowDialog() == true)
            {
                FilesAct.AddFiles(dlgOpen.FileNames);
            }
        }

        //--------------------------------------------------------------------------------
        // Команда Drop файлы актов
        //--------------------------------------------------------------------------------

        public void Acts_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                FilesAct.AddFiles(files);
            }
        }

        //--------------------------------------------------------------------------------
        // Команда Открыть файл акта двойным щелчком
        //--------------------------------------------------------------------------------
        public ICommand OpenActFileCommand => new LambdaCommand(OnOpenActFileCommandExecuted, CanOpenActFileCommand);
        private bool CanOpenActFileCommand(object p) => true;
        private void OnOpenActFileCommandExecuted(object p)
        {
            if (p is ActFiles af)
            {
                FilesAct.StartFile(af);
            }

        }

        //--------------------------------------------------------------------------------
        // Команда Удалить файл акта
        //--------------------------------------------------------------------------------
        public ICommand DeleteActFileCommand => new LambdaCommand(OnDeleteActFileCommandExecuted, CanDeleteActFileCommand);
        private bool CanDeleteActFileCommand(object p) => true;
        private void OnDeleteActFileCommandExecuted(object p)
        {
            ActFiles FileName = p as ActFiles;
            FilesAct.DeleteFile(FileName);

        }

        //--------------------------------------------------------------------------------
        // Команда ОК для создаия формы
        //--------------------------------------------------------------------------------
        public ICommand OKCommand => new LambdaCommand(OnOKCommandExecuted, CanOKCommand);
        private bool CanOKCommand(object p) => p is Window;
        private void OnOKCommandExecuted(object p)
        {
            // удажяем отмеченные для удаления файлы
            foreach (ActFiles item in FilesAct.DeleteItem)
            {
                CurrentJob.ActFiles.Remove(item);
            }

            // Добавляем файлы
            foreach (ActFiles item in FilesAct.ListFiles)
            {
                if (item.FullName != null)
                {
                    //item.af_JobId = CurrentJob.id;
                    CurrentJob.ActFiles.Add(item);
                }
            }


            if (_repo.Add(CurrentJob, true))
            {
                FilesAct.CommitFilesAsync();

                // отправить оповещения для всех
                foreach (var item in CurrentJob.Action)
                {
                    SenderToEmail senderEmail = new SenderToEmail(item.User);
                    senderEmail.SendMail("Создана форма.");
                }
            }

            var win = (Window)p;
            //Window win = App.Current.Windows.OfType<InControlDetailWindow>().First();
            win.DialogResult = true;
        }


        #endregion

    }
}
