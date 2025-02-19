﻿using OTK.Commands;
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
            NameForm = "Входной контроль";
            FormType = EnumFormType.VK;
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
            win.DataContext = new FormDetailWindowViewModel(job, NameForm, new InControlUC());
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
                win.DataContext = new FormUserWindowViewModel(SelectedJob.id, NameForm, new InControlUC());
            }
            else
            {
                win = new FormDetailWindow();
                win.DataContext = new FormDetailWindowViewModel(SelectedJob, NameForm, new InControlUC());
            }
            if (win.ShowDialog() == true)
            {
            }

            RefreshListJobs();
        }


        #region Команды
        #endregion


    }
}
