using OTK.Commands;
using OTK.Infrastructure;
using OTK.Models;
using OTK.Repository;
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
    internal class RnOViewModel : Observable
    {
        public ObservableCollection<RnO> ListRnO { get; set; }
        public RnO SelectedRnO { get; set; }

        private EnumFilter _filter;

        //--------------------------------------------------------------------------------
        // Конструктор
        //--------------------------------------------------------------------------------
        public RnOViewModel()
        {

        }


        //--------------------------------------------------------------------------------
        // Загрузка списка РнО
        //--------------------------------------------------------------------------------
        public void LoadRnO(EnumFilter filter)
        {
            _filter = filter;
            using (RepositoryMSSQL<RnO> repo = new RepositoryMSSQL<RnO>())
            {
                ListRnO = new ObservableCollection<RnO>(repo.Items);
            }
            OnPropertyChanged(nameof(ListRnO));
        }

        private void RefreshRnO()
        {
            using (RepositoryMSSQL<RnO> repo = new RepositoryMSSQL<RnO>())
            {
                ListRnO = new ObservableCollection<RnO>(repo.Items);
            }
            OnPropertyChanged(nameof(ListRnO));
        }



        //--------------------------------------------------------------------------------
        // Создание формы РнО
        //--------------------------------------------------------------------------------
        public void CreateForm()
        {
            RnOWindow win = new RnOWindow();
            RnO rno = new RnO();
            rno.RnoDate = DateTime.Now;
            win.DataContext = new RnOWindowViewModel(rno);
            if (win.ShowDialog() == true)
            {
            }

        }


        #region Команды

        //--------------------------------------------------------------------------------
        // Команда Двойной щелчок
        //--------------------------------------------------------------------------------
        private readonly ICommand _DblClickCommand = null;
        public ICommand DblClickCommand => _DblClickCommand ?? new LambdaCommand(OnDblClickCommandExecuted, CanDblClickCommand);
        private bool CanDblClickCommand(object p) => SelectedRnO != null;
        private void OnDblClickCommandExecuted(object p)
        {
            Window win = new RnOWindow();
            win.DataContext = new RnOWindowViewModel(SelectedRnO);
            if (win.ShowDialog() == true)
            {
                RefreshRnO();
            }

        }

        #endregion
    }
}
