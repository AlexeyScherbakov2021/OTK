using OTK.Commands;
using OTK.Infrastructure;
using OTK.Models;
using OTK.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OTK.ViewModels.Forms
{
    internal abstract class FormAbstract : Observable
    {
        public Users User { get; set; }
        protected readonly RepositoryMSSQL<Jobs> _repo;
        public ObservableCollection<Jobs> ListJobs { get; set; }
        public Jobs SelectedJob { get; set; }
        public EnumFormType FormType = EnumFormType.Fail;


        //--------------------------------------------------------------------------------
        // Конструктор
        //--------------------------------------------------------------------------------
        public FormAbstract()
        {
            User = App.CurrentUser;
            _repo = new RepositoryMSSQL<Jobs>();
            //LoadJobs(EnumStatusJob.ReqConfirm);
        }


        //--------------------------------------------------------------------------------
        // Загрузка работ
        //--------------------------------------------------------------------------------
        public void LoadJobs(EnumStatusJob status)
        {
#if DEBUG
            if (FormType == EnumFormType.Fail)
                throw new ArgumentOutOfRangeException(nameof(FormType));
#endif

            ListJobs = new ObservableCollection<Jobs>(_repo.Items.Where(it => it.JobStatus == status && it.JobType == FormType));
            OnPropertyChanged(nameof(ListJobs));
        }

        //--------------------------------------------------------------------------------
        // Создание формы
        //--------------------------------------------------------------------------------
        public abstract void CreateForm();

        //--------------------------------------------------------------------------------
        // Открытие формы
        //--------------------------------------------------------------------------------
        public abstract void OpenForm();


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

    }


}
