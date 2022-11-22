using Microsoft.Win32;
using OTK.Commands;
using OTK.Infrastructure;
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
        //private readonly RepositoryBase repo;

        //public User SelectUser { get; set; }
        //public IEnumerable<User> ListUser { get; set; }


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
                //// если праоль неверный, то ничего не делаем - возврат
                //if (pass.Password != SelectUser?.u_pass)
                //    return;

                //App.CurrentUser = SelectUser;
                //if (App.CurrentUser.u_role == 1)
                //{
                //    //если это администратор, то запускаем настройки
                //    SettingWindow win = new SettingWindow();
                //    win.Show();
                //    App.Current.MainWindow = win;
                //}
                //else
                //{
                //    // записываем в реестр
                //    RegistryKey SoftKey = Registry.CurrentUser.OpenSubKey("SOFTWARE", true);
                //    RegistryKey ProgKey = SoftKey.CreateSubKey("MoveOrders");
                //    ProgKey.SetValue("login", SelectUser.u_login);
                //    ProgKey.Close();
                //    SoftKey.Close();

                //    // если пользователь, то запускаем табель
                //    MainWindow win = new MainWindow();
                //    win.Show();
                //    App.Current.MainWindow = win;
                //}

            }
            App.Current.Windows.OfType<LoginWindow>().First().Close();
        }

        #endregion


        public LoginWindowViewModel()
        {

            //repo = new RepositoryBase();
            //ListUser = repo.Users.Include(it => it.RolesUser).OrderBy(o => o.u_login).ToArray();

            //if (ListUser is null || ListUser.Count() == 0 || !ListUser.Any(it => it.u_role == 1))
            //{
            //    ListUser = new List<User> { new User { u_name = "Admin", u_pass = "adm", u_role = 1, u_login = "Admin" } };
            //}

            //string login = "Admin";
            //RegistryKey SoftKey = Registry.CurrentUser.OpenSubKey("SOFTWARE");
            //RegistryKey ProgKey = SoftKey.OpenSubKey("MoveOrders");
            //if (ProgKey != null)
            //{
            //    login = ProgKey.GetValue("login", "Admin").ToString();
            //    App.Log.WriteLineLog("LoginWindowViewModel login");
            //    ProgKey.Close();
            //}
            //SoftKey.Close();



            //SelectUser = ListUser.FirstOrDefault(it => it.u_login == login);

        }

    }
}
