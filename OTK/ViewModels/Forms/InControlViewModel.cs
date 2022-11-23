using OTK.Commands;
using OTK.Infrastructure;
using OTK.Models;
using OTK.Repository;
using OTK.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OTK.ViewModels.Forms
{
    internal class InControlViewModel : Observable
    {
        private readonly RepositoryMSSQL<Jobs> _repo;

        public ObservableCollection<Jobs> ListJobs { get; set; }
        public Jobs SelectedJob { get; set; }

        //--------------------------------------------------------------------------------
        // Конструктор
        //--------------------------------------------------------------------------------
        public InControlViewModel()
        {
            _repo = new RepositoryMSSQL<Jobs>();
            LoadJobs( EnumStatus.CheckedProcess);
        }


        //--------------------------------------------------------------------------------
        // Загрузка работ
        //--------------------------------------------------------------------------------
        private void LoadJobs(EnumStatus status)
        {
            ListJobs = new ObservableCollection<Jobs>(_repo.Items.Where(it => it.JobStatus == status));
            //OnPropertyChanged(nameof(ListJobs));

        }


        //--------------------------------------------------------------------------------
        // Создание формы
        //--------------------------------------------------------------------------------
        public void CreateForm()
        {
            InControlDetailWindow win = new InControlDetailWindow();
            Jobs job = new Jobs();
            win.DataContext = new InControlDetailWindowViewModel(_repo, job);
            if(win.ShowDialog() == true)
            {
                _repo.Add(job);
                _repo.Save();

                ListJobs.Add(job);
            }
        }


        //--------------------------------------------------------------------------------
        // Открытие формы
        //--------------------------------------------------------------------------------
        public void OpenForm()
        {
            InControlDetailWindow win = new InControlDetailWindow();
            win.DataContext = new InControlDetailWindowViewModel(_repo, SelectedJob);
            if (win.ShowDialog() == true)
            {
                //_repo.Save();
            }

        }


        #region Команды

        //--------------------------------------------------------------------------------
        // Команда Двойной щелчок
        //--------------------------------------------------------------------------------
        private readonly ICommand _DblClickCommand = null;
        public ICommand DblClickCommand => _DblClickCommand ?? new LambdaCommand(OnDblClickCommandExecuted, CanDblClickCommand);
        private bool CanDblClickCommand(object p) => true;
        private void OnDblClickCommandExecuted(object p)
        {
            OpenForm();
        }


        //--------------------------------------------------------------------------------
        // Команда Создать
        //--------------------------------------------------------------------------------
        //private readonly ICommand _CreateCommand = null;
        //public ICommand CreateCommand => _CreateCommand ?? new LambdaCommand(OnCreateCommandExecuted, CanCreateCommand);
        //private bool CanCreateCommand(object p) => true;
        //private void OnCreateCommandExecuted(object p)
        //{


        //}

        #endregion


    }
}
