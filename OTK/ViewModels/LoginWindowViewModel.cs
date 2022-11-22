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
using System.Windows.Controls;
using System.Windows.Input;

namespace OTK.ViewModels
{
    internal class LoginWindowViewModel : Observable
    {
        private readonly RepositoryMSSQL<Users> repo;

        public Users SelectUser { get; set; }
        public IEnumerable<Users> ListUser { get; set; }

        //-----------------------------------------------------------------------------------
        // Конструктор
        //-----------------------------------------------------------------------------------
        public LoginWindowViewModel()
        {
            repo = new RepositoryMSSQL<Users>();
            ListUser = repo.Items.OrderBy(o => o.UserLogin).ToArray();

            if (ListUser is null || ListUser.Count() == 0 || !ListUser.Any(it => it.UserRole == EnumRoles.Admin))
            {
                ListUser = new List<Users> { new Users { UserName = "Admin", UserPass = "adm", UserRole = EnumRoles.Admin, UserLogin = "Admin" } };
            }

            string login = "Admin";
            RegistryKey SoftKey = Registry.CurrentUser.OpenSubKey("SOFTWARE");
            RegistryKey ProgKey = SoftKey.OpenSubKey("OTK");
            if (ProgKey != null)
            {
                login = ProgKey.GetValue("login", "Admin").ToString();
                ProgKey.Close();
            }
            SoftKey.Close();

            SelectUser = ListUser.FirstOrDefault(it => it.UserLogin == login);

        }


        #region Команды
        //--------------------------------------------------------------------------------
        // Команда Сохранить дефлятор
        //--------------------------------------------------------------------------------
        //private readonly ICommand _OkCommand = null;
        public ICommand OkCommand => new LambdaCommand(OnOkCommandExecuted, CanOkCommand);
        private bool CanOkCommand(object p) => true;
        private void OnOkCommandExecuted(object p)
        {
            if (p is PasswordBox pass)
            {
                // если праоль неверный, то ничего не делаем - возврат
                if (pass.Password != SelectUser?.UserPass)
                    return;

                App.CurrentUser = SelectUser;
                if (App.CurrentUser.UserRole == EnumRoles.Admin)
                {
                    //если это администратор, то запускаем настройки
                    SettingWindow win = new SettingWindow();
                    win.Show();
                    App.Current.MainWindow = win;
                }
                else
                {
                    // записываем в реестр
                    RegistryKey SoftKey = Registry.CurrentUser.OpenSubKey("SOFTWARE", true);
                    RegistryKey ProgKey = SoftKey.CreateSubKey("OTK");
                    ProgKey.SetValue("login", SelectUser.UserLogin);
                    ProgKey.Close();
                    SoftKey.Close();

                    // если пользователь, то запускаем табель
                    MainWindow win = new MainWindow();
                    win.Show();
                    App.Current.MainWindow = win;
                }

            }
            App.Current.Windows.OfType<LoginWindow>().First().Close();
        }

        #endregion


    }
}
