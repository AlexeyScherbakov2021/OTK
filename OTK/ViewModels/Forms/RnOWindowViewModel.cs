using Microsoft.Win32;
using OTK.Commands;
using OTK.Infrastructure;
using OTK.Models;
using OTK.Repository;
using OTK.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Xml.Linq;

namespace OTK.ViewModels.Forms
{
    internal class RnOWindowViewModel : Observable
    {
        private readonly RepositoryMSSQL<RnO> _repo;
        private readonly RepositoryMSSQL<Users> _repoUsers;

        public Visibility IsCloseVisible { get; set; }
        public Visibility IsEditVisible { get; set; }
        public Visibility IsCreateVisible { get; set; }
        public bool IsReadOnlyField { get; set; }
        public bool IsEnabledButton { get; set; } = true;

        public List<Users> ListUsers { get; set; }

        public readonly string NameForm = "РнО";
        public string Title { get; set; }

        public RnO CurrentRnO { get; set; }

        public AttachListFiles<ActFiles> FilesAct { get; set; }


        public RnOWindowViewModel()
        {

        }


        public RnOWindowViewModel(RnO rno)
        {
            _repo = new RepositoryMSSQL<RnO>();
            _repoUsers = new RepositoryMSSQL<Users>(_repo.GetDB());
            ListUsers = _repoUsers.Items.OrderBy(o => o.UserName).ToList();

            if (rno.id == 0)    // если это было создание
            {
                Title = "Создание формы " + NameForm;
                IsEditVisible = Visibility.Collapsed;
                IsCreateVisible = Visibility.Visible;
                IsCloseVisible = Visibility.Collapsed;
                IsReadOnlyField = false;
                IsEnabledButton = true;
                CurrentRnO = rno;
            }
            else
            {
                Title = "Форма " + NameForm;
                CurrentRnO = _repo.Get(rno.id);
                IsReadOnlyField = true;
                IsEnabledButton = false;
                IsCloseVisible = Visibility.Visible;
                IsCreateVisible = Visibility.Collapsed;

                //IsEditVisible = CurrentRnO == EnumStatusJob.Closed
                //    ? IsEditVisible = Visibility.Collapsed
                //    : IsEditVisible = Visibility.Visible;
            }

            FilesAct = new AttachListFiles<ActFiles>(CurrentRnO.RnoDate.Year, "RnO");
            //FilesAct.AssignFiles(CurrentRnO.ActFiles);

        }


        #region Команды

        //--------------------------------------------------------------------------------
        // Команда В архив
        //--------------------------------------------------------------------------------
        public ICommand ArchiveCommand => new LambdaCommand(OnArchiveCommandExecuted, CanArchiveCommand);
        private bool CanArchiveCommand(object p) => p is Window;
        private void OnArchiveCommandExecuted(object p)
        {
            if (MessageBox.Show($"Перевести форму в архив?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                //CurrentJob.JobStatus = EnumStatusJob.Closed;
                //_repo.Save();
                ////Window win = App.Current.Windows.OfType<InControlDetailWindow>().First();

                //var win = (Window)p;
                //win.DialogResult = true;
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
                //AttachListFiles<ActionFiles> FilesAction = new AttachListFiles<ActionFiles>(CurrentJob.JobDate.Year, /*af.idParent,*/ "Action");
                //FilesAction.StartFile(af);
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
                //FilesAct.AddFiles(dlgOpen.FileNames);
            }
        }

        //--------------------------------------------------------------------------------
        // Команда Drop файлы актов
        //--------------------------------------------------------------------------------

        public void Acts_Drop(object sender, DragEventArgs e)
        {
            //if (e.Data.GetDataPresent(DataFormats.FileDrop))
            //{
            //    string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            //    FilesAct.AddFiles(files);
            //}
        }

        //--------------------------------------------------------------------------------
        // Команда Открыть файл акта двойным щелчком
        //--------------------------------------------------------------------------------
        public ICommand OpenActFileCommand => new LambdaCommand(OnOpenActFileCommandExecuted, CanOpenActFileCommand);
        private bool CanOpenActFileCommand(object p) => true;
        private void OnOpenActFileCommandExecuted(object p)
        {
            //if (p is ActFiles af)
            //{
            //    FilesAct.StartFile(af);
            //}

        }

        //--------------------------------------------------------------------------------
        // Команда Удалить файл акта
        //--------------------------------------------------------------------------------
        public ICommand DeleteActFileCommand => new LambdaCommand(OnDeleteActFileCommandExecuted, CanDeleteActFileCommand);
        private bool CanDeleteActFileCommand(object p) => true;
        private void OnDeleteActFileCommandExecuted(object p)
        {
            //ActFiles FileName = p as ActFiles;
            //FilesAct.DeleteFile(FileName);

        }

        //--------------------------------------------------------------------------------
        // Команда ОК для создаия формы
        //--------------------------------------------------------------------------------
        public ICommand OKCommand => new LambdaCommand(OnOKCommandExecuted, CanOKCommand);
        private bool CanOKCommand(object p) => p is Window;
        private void OnOKCommandExecuted(object p)
        {
            // удажяем отмеченные для удаления файлы
            //foreach (ActFiles item in FilesAct.DeleteItem)
            //{
            //    CurrentJob.ActFiles.Remove(item);
            //}

            //// Добавляем файлы
            //foreach (ActFiles item in FilesAct.ListFiles)
            //{
            //    if (item.FullName != null)
            //    {
            //        //item.af_JobId = CurrentJob.id;
            //        CurrentJob.ActFiles.Add(item);
            //    }
            //}

            if (_repo.Add(CurrentRnO, true))
            {
                //FilesAct.CommitFilesAsync();
            }

            var win = (Window)p;
            //Window win = App.Current.Windows.OfType<InControlDetailWindow>().First();
            win.DialogResult = true;
        }


        #endregion

    }
}
