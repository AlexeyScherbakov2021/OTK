﻿using Microsoft.Win32;
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

        public bool IsEnabledButton { get; set; } = false;
        public bool IsEnabledButtonUser { get; set; } = false;
        public bool IsReadOnlyField { get; set; } = true;


        public AttachListFiles<ActionFiles> FilesAction { get; set; }

        public AttachListFiles<ActFiles> FilesAct { get; set; }

        //--------------------------------------------------------------------------------
        // Конструктор
        //--------------------------------------------------------------------------------
        public InControlUserWindowViewModel(/*RepositoryMSSQL<Jobs> repo,*/ int jobId)
        {
            _repoJob = new RepositoryMSSQL<Jobs>();
            CurrentJob = _repoJob.Get(jobId);
            CurrentActor = CurrentJob.Action.FirstOrDefault(it => it.User.id == User.id);

            FilesAction = new AttachListFiles<ActionFiles>(CurrentJob.JobDate.Year,/* CurrentActor.id,*/ "Action");
            FilesAction.AssignFiles(CurrentActor.ActionFiles);

            IsEnabledButtonUser = CurrentActor.ActionStatus == EnumStatus.CheckedProcess;

            FilesAct = new AttachListFiles<ActFiles>(CurrentJob.JobDate.Year, /*CurrentJob.id,*/ "Job");
            FilesAct.AssignFiles(CurrentJob.ActFiles);

        }

        public InControlUserWindowViewModel()
        {

        }


        #region Команды

        //--------------------------------------------------------------------------------
        // Команда Выполнено
        //--------------------------------------------------------------------------------
        public ICommand CommitCommand => new LambdaCommand(OnCommitCommandExecuted, CanCommitCommand);
        private bool CanCommitCommand(object p) => CurrentActor?.ActionStatus == EnumStatus.CheckedProcess;
        private void OnCommitCommandExecuted(object p)
        {
            CurrentActor.ActionStatus = EnumStatus.Checked;
            CurrentActor.Jobs.JobStatus = EnumStatusJob.ReqConfirm;

            // удажяем отмеченные для удаления файлы
            foreach(ActionFiles item in FilesAction.DeleteItem)
            {
                CurrentActor.ActionFiles.Remove(item );
            }

            // Добавляем файлы
            foreach(ActionFiles item in FilesAction.ListFiles)
            {
                if (item.FullName != null)
                {
                    item.idParent = CurrentActor.id;
                    CurrentActor.ActionFiles.Add(item);
                }
            }

            if(_repoJob.Save())
                FilesAction.CommitFilesAsync();

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
            FilesAction.DeleteFile(FileName);
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
                FilesAction.AddFiles(dlgOpen.FileNames);
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
                FilesAction.AddFiles(files);
            }
        }

        //--------------------------------------------------------------------------------
        // Команда Открыть файл двойным щелчком
        //--------------------------------------------------------------------------------
        public ICommand OpenFileCommand => new LambdaCommand(OnOpenFileCommandExecuted, CanOpenFileCommand);
        private bool CanOpenFileCommand(object p) => true;
        private void OnOpenFileCommandExecuted(object p)
        {
            if (p is ActionFiles af)
            {
                FilesAction.StartFile(af);
            }
        }

        #endregion

    }
}
