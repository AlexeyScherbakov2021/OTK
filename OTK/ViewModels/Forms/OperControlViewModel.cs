using OTK.Infrastructure;
using OTK.Models;
using OTK.Views.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace OTK.ViewModels.Forms
{
    internal class OperControlViewModel : FormAbstract
    {



        //--------------------------------------------------------------------------------
        // Конструктор
        //--------------------------------------------------------------------------------
        public OperControlViewModel() : base()
        {
            NameForm = "Операционный контроль";
            FormType = EnumFormType.OK;
            LoadListJobs(EnumFilter.Require);
        }

        //--------------------------------------------------------------------------------
        // Создание формы
        //--------------------------------------------------------------------------------
        public override void CreateForm()
        {
            FormDetailWindow win = new FormDetailWindow();
            Jobs job = new Jobs();
            job.JobType = FormType;
            job.JobDate = DateTime.Now;
            win.DataContext = new FormDetailWindowViewModel(job, NameForm, new OperControlUC());
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
            Window win = new FormDetailWindow();

            if (User.UserRole == EnumRoles.Пользователь)
            {
                win = new FormUserWindow();
                win.DataContext = new FormUserWindowViewModel(SelectedJob.id, NameForm, new OperControlUC());
            }
            else
            {
                win = new FormDetailWindow();
                win.DataContext = new FormDetailWindowViewModel(SelectedJob, NameForm, new OperControlUC());
            }
            if (win.ShowDialog() == true)
            {

            }

            RefreshListJobs();
        }


    }
}
