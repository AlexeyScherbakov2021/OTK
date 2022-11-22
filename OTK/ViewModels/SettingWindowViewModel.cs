using OTK.Commands;
using OTK.Infrastructure;
using OTK.Models;
using OTK.Repository;
using OTK.ViewModels.Setting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OTK.ViewModels
{
    internal class SettingWindowViewModel : Observable
    {
        public static RepositoryMSSQL<Users> repo = new RepositoryMSSQL<Users>();

        public UsersControlViewModel UserViewModel { get; set; }

        public SettingWindowViewModel()
        {
            UserViewModel = new UsersControlViewModel();

        }

        #region Команды
        //--------------------------------------------------------------------------------
        // Команда Добавить 
        //--------------------------------------------------------------------------------
        public ICommand ClosingCommand => new LambdaCommand(OnClosingCommandExecuted, CanClosingCommand);
        private bool CanClosingCommand(object p) => true;
        private void OnClosingCommandExecuted(object p)
        {
            UserViewModel.SaveUsers();
        }
        #endregion

    }
}
