using OTK.Commands;
using OTK.Infrastructure;
using OTK.Models;
using OTK.Repository;
using OTK.Views;
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
    internal class InControlViewModel : FormAbstract
    {

        //--------------------------------------------------------------------------------
        // Конструктор
        //--------------------------------------------------------------------------------
        public InControlViewModel() : base()
        {
            FormType = EnumFormType.VK;
            LoadListJobs(EnumFilter.Require);
        }


        //--------------------------------------------------------------------------------
        // Создание формы
        //--------------------------------------------------------------------------------
        public override void CreateForm()
        {
            InControlDetailWindow win = new InControlDetailWindow();
            Jobs job = new Jobs();
            job.JobType = FormType;
            job.JobDate = DateTime.Now;
            win.DataContext = new InControlDetailWindowViewModel(job);
            if (win.ShowDialog() == true)
            {
                RefreshListJobs();
            }
        }


        //--------------------------------------------------------------------------------
        // Открытие формы
        //--------------------------------------------------------------------------------
        public override void OpenForm()
        {
            Window win = new InControlDetailWindow();

            //using (RepositoryMSSQL<Jobs> repo = new RepositoryMSSQL<Jobs>())
            //{

                if (User.UserRole == EnumRoles.Пользователь)
                {
                    win = new InControlUserWindow();
                    win.DataContext = new InControlUserWindowViewModel(SelectedJob.id);
                }
                else
                {
                    win = new InControlDetailWindow();
                    win.DataContext = new InControlDetailWindowViewModel( SelectedJob);
                }
                if (win.ShowDialog() == true)
                {

                    //_repo.Save();
                }

                RefreshListJobs();
            //}
        }


        #region Команды
        #endregion


    }
}
