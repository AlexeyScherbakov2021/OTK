using Microsoft.Win32;
using OTK.Commands;
using OTK.Infrastructure;
using OTK.Models;
using OTK.Repository;
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
    internal class InControlUserWindowViewModel : Observable
    {
        private readonly RepositoryMSSQL<Jobs> _repoJob;
        public Users User => App.CurrentUser;

        public Jobs CurrentJob { get; set; }

        public ActionUser CurrentActor { get; set; }

        public bool IsEnabledButton { get; set; } = true;

        public ObservableCollection<ActionFiles> ListActionFiles { get; set; }

        //--------------------------------------------------------------------------------
        // Конструктор
        //--------------------------------------------------------------------------------
        public InControlUserWindowViewModel(RepositoryMSSQL<Jobs> repo, int jobId)
        {
            _repoJob = repo;
            CurrentJob = _repoJob.Get(jobId);
            CurrentActor = CurrentJob.Action.FirstOrDefault(it => it.User.id == User.id);
            ListActionFiles = new ObservableCollection<ActionFiles>(CurrentActor.ActionFiles);
            IsEnabledButton = CurrentActor.ActionStatus == EnumStatus.CheckedProcess;
        }

        public InControlUserWindowViewModel()
        {

        }


        #region Команды

        //--------------------------------------------------------------------------------
        // Команда Выполнено
        //--------------------------------------------------------------------------------
        public ICommand CommitCommand => new LambdaCommand(OnCommitCommandExecuted, CanCommitCommand);
        private bool CanCommitCommand(object p) => true;
        private void OnCommitCommandExecuted(object p)
        {
            CurrentActor.ActionStatus = EnumStatus.Checked;
            CurrentActor.Jobs.JobStatus = EnumStatusJob.ReqConfirm;
            CurrentActor.ActionFiles = ListActionFiles;
            _repoJob.Save();

            RepositoryFiles repoFiles = new RepositoryFiles();
            repoFiles.AddFilesAsync(CurrentActor);

            App.Current.Windows.OfType<InControlUserWindow>().FirstOrDefault().Close();

        }

        //--------------------------------------------------------------------------------
        // Команда Удалить файл
        //--------------------------------------------------------------------------------
        private readonly ICommand _DeleteFileCommand = null;
        public ICommand DeleteFileCommand => _DeleteFileCommand ?? new LambdaCommand(OnDeleteFileCommandExecuted, CanDeleteFileCommand);
        private bool CanDeleteFileCommand(object p) => true;
        private void OnDeleteFileCommandExecuted(object p)
        {
            ActionFiles FileName = p as ActionFiles;

            //MainWindowViewModel.repo.Delete<RouteAdding>(FileName);

            ListActionFiles.Remove(FileName);

        }




        //--------------------------------------------------------------------------------
        // Команда Обзор файлов
        //--------------------------------------------------------------------------------
        public ICommand BrowseCommand => new LambdaCommand(OnBrowseCommandExecuted, CanBrowseCommand);
        private bool CanBrowseCommand(object p) => true; 
        private void OnBrowseCommandExecuted(object p)
        {
            OpenFileDialog dlgOpen = new OpenFileDialog();
            dlgOpen.Multiselect = true;

            if (dlgOpen.ShowDialog() == true)
            {
                FilesFunction.AddFiles(dlgOpen.FileNames, ListActionFiles);
            }
        }

        //--------------------------------------------------------------------------------
        // Команда Drop
        //--------------------------------------------------------------------------------

        public void ItemsControl_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                FilesFunction.AddFiles(files, ListActionFiles);
            }
        }

        //--------------------------------------------------------------------------------
        // Команда Открыть файл двойным щелчком
        //--------------------------------------------------------------------------------
        public ICommand OpenFileCommand => new LambdaCommand(OnOpenFileCommandExecuted, CanOpenFileCommand);
        private bool CanOpenFileCommand(object p) => true;
        private void OnOpenFileCommandExecuted(object p)
        {
            RepositoryFiles repoFiles = new RepositoryFiles();
            repoFiles.OpenActionFile(p as ActionFiles);
        }


        #endregion

    }
}
