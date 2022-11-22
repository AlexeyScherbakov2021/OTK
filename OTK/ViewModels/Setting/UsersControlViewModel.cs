using OTK.Commands;
using OTK.Infrastructure;
using OTK.Models;
using OTK.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;

namespace OTK.ViewModels.Setting
{
    internal class UsersControlViewModel : Observable
    {
        RepositoryMSSQL<Users> repo;

        public Users SelectedUser { get; set; }
        public ObservableCollection<Users> ListUser { get; set; }

        public void SaveUsers()
        {
            //if (SelectedUser is null) return;
            repo.Save();
        }


        //--------------------------------------------------------------------------------
        // Конструктор 
        //--------------------------------------------------------------------------------
        public UsersControlViewModel()
        {
            repo = SettingWindowViewModel.repo;
            ListUser = new ObservableCollection<Users>(repo.Items.OrderBy(o => o.UserName));
        }



        #region Команды
        //--------------------------------------------------------------------------------
        // Команда Добавить 
        //--------------------------------------------------------------------------------
        public ICommand AddCommand => new LambdaCommand(OnAddCommandExecuted, CanAddCommand);
        private bool CanAddCommand(object p) => true;
        private void OnAddCommandExecuted(object p)
        {
            Users newUser = new Users();

            ListUser.Add(newUser);
            SelectedUser = newUser;
            repo.Add(newUser, true);
        }
        //--------------------------------------------------------------------------------
        // Команда Удалить
        //--------------------------------------------------------------------------------
        public ICommand DeleteCommand => new LambdaCommand(OnDeleteCommandExecuted, CanDeleteCommand);
        private bool CanDeleteCommand(object p) => SelectedUser != null;
        private void OnDeleteCommandExecuted(object p)
        {
            if (MessageBox.Show($"Удалить {SelectedUser.UserLogin}", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                if (repo.Delete(SelectedUser, true))
                    ListUser.Remove(SelectedUser);
            }
        }

        #endregion

    }
}
