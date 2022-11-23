using OTK.Commands;
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
        //private RespUserWindow win;

        public string Title { get; set; } = "форма Входной контроль";

        public List<Users> ListUsers { get; set; }  

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
            _repo= repo;
            _repoUsers = new RepositoryMSSQL<Users>();
            ListUsers = _repoUsers.Items.OrderBy(o => o.UserName).ToList();

            if (job.id == 0)
            {
                Title = "Создание формы Входной контроль";
                IsEditVisible = Visibility.Collapsed;
                IsCreateVisible= Visibility.Visible;
            }

            CurrentJob= job;
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
        // Команда ОК в диалоговом окне ответственного
        //--------------------------------------------------------------------------------
        //public ICommand DialogOKCommand => new LambdaCommand(OnDialogOKCommandExecuted, CanDialogOKCommand);
        //private bool CanDialogOKCommand(object p) => true;
        //private void OnDialogOKCommandExecuted(object p)
        //{
        //    win.Close();
        //    CurrentJob.Action.Add(NewAction);
        //}

        //--------------------------------------------------------------------------------
        // Команда Отмена в диалоговом окне ответственного
        //--------------------------------------------------------------------------------
        //public ICommand DialogCancelCommand => new LambdaCommand(OnDialogCancelCommandExecuted, CanDialogCancelCommand);
        //private bool CanDialogCancelCommand(object p) => true;
        //private void OnDialogCancelCommandExecuted(object p)
        //{
        //    win.Close();
        //}


        #endregion

    }
}
