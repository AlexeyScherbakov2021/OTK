using OTK.Commands;
using OTK.Infrastructure;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using OTK.ViewModels.Forms;

namespace OTK.ViewModels
{
    internal class MainwindowViewModel : Observable
    {
#if DEBUG
        public string Title { get; set; } = "Список задач (отладочная версия)";
#else
        public string Title { get; set; } = "Список задач";
#endif

        public InControlViewModel vmInControl { get; set; }

        public TabItem SelectedTab { get; set; }


        #region Команды

        //--------------------------------------------------------------------------------
        // Команда Фильтровать заказы
        //--------------------------------------------------------------------------------
        private readonly ICommand _FilterCommand = null;
        public ICommand FilterCommand => _FilterCommand ?? new LambdaCommand(OnFilterCommandExecuted, CanFilterCommand);
        private bool CanFilterCommand(object p) => true;
        private void OnFilterCommandExecuted(object p)
        {
            //int select = int.Parse(p.ToString());
            //timer.Stop();

            //// подготовка к обновлению всех записей
            //repo.Refresh();


            //if (CheckCreated)
            //{
            //    // созданные мной заказы
            //    ListOrders = new ObservableCollection<Order>(repo.Orders
            //    .Where(it => it.o_statusId < EnumStatus.Closed && it.RouteOrders.Where(i => i.ro_step == 0
            //    && i.ro_userId == App.CurrentUser.id).Any())
            //    //.Include(it => it.RouteOrders)
            //    );

            //}
            //else if (CheckCoordinated)
            //{
            //    // Требующие рассмотрения
            //    ListOrders = new ObservableCollection<Order>(repo.Orders
            //        .Where(it => it.RouteOrders
            //                .Where(r => r.ro_userId == App.CurrentUser.id
            //                    && r.ro_check == EnumCheckedStatus.CheckedProcess)
            //                .Any())
            //    //.Include(it => it.RouteOrders)
            //       );
            //}

            //else if (CheckClosed)
            //{
            //    // Закрытые заказы
            //    ListOrders = new ObservableCollection<Order>(repo.Orders
            //        .Where(it => it.o_statusId == EnumStatus.Closed && it.RouteOrders.Where(r =>
            //        r.ro_userId == App.CurrentUser.id).Any())
            //    //.Include(it => it.RouteOrders)
            //    );
            //}
            //else if (CheckWork)
            //{
            //    // В работе
            //    ListOrders = /*_IsViewAllOrders
            //        ? ListOrders = new ObservableCollection<Order>(repo.Orders.Where(it => it.o_statusId < EnumStatus.Closed ))
            //        :*/ ListOrders = new ObservableCollection<Order>(repo.Orders
            //            .Where(it => it.o_statusId < EnumStatus.Closed && it.RouteOrders.Where(r => r.ro_userId == App.CurrentUser.id).Any()));



            //}
            //else if (CheckAll)
            //{
            //    // все заказы
            //    ListOrders = _IsViewAllOrders
            //        ? new ObservableCollection<Order>(repo.Orders)
            //        : new ObservableCollection<Order>(repo.Orders
            //            .Where(it => it.RouteOrders.Where(r => r.ro_userId == App.CurrentUser.id).Any())
            //            //.Include(it => it.RouteOrders)
            //            );
            //}

            //// сортировка этапов для всех заказов
            //foreach (var item in ListOrders)
            //{
            //    item.RouteOrders = SortStepRoute(item.RouteOrders);
            //}

            //// установка пользователей в работе для каждого заказа
            //foreach (var item in ListOrders)
            //{
            //    //item.WorkUser = item.RouteOrders.FirstOrDefault(it => it.ro_check == EnumCheckedStatus.CheckedProcess)?.User;
            //    item.WorkUser = MainWindowViewModel.repo.RouteOrders.FirstOrDefault(it => it.ro_orderId == item.id
            //        && it.ro_check == EnumCheckedStatus.CheckedProcess)?.User;
            //}

            //OnPropertyChanged(nameof(ListOrders));

            //timer.Start();
        }


        //--------------------------------------------------------------------------------
        // Команда Удалить заказ
        //--------------------------------------------------------------------------------
        private readonly ICommand _DeleteCommand = null;
        public ICommand DeleteCommand => _DeleteCommand ?? new LambdaCommand(OnDeleteCommandExecuted, CanDeleteCommand);
        private bool CanDeleteCommand(object p) => true;
        private void OnDeleteCommandExecuted(object p)
        {

            //if (MessageBox.Show($"Удалить заказ {SelectedOrder.o_name}", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            //{
            //    RepositoryFiles repoFiles = new RepositoryFiles();
            //    repoFiles.DeleteFiles(SelectedOrder);
            //    repo.Delete(SelectedOrder, true);
            //    ListOrders.Remove(SelectedOrder);
            //}
        }

        //--------------------------------------------------------------------------------
        // Команда Отчет
        //--------------------------------------------------------------------------------
        private readonly ICommand _ReportCommand = null;
        public ICommand ReportCommand => _ReportCommand ?? new LambdaCommand(OnReportCommandExecuted, CaReportCommand);
        private bool CaReportCommand(object p) => true;
        private void OnReportCommandExecuted(object p)
        {
            //timer.Stop();
            //ReportRoutesWindow reportWindow = new ReportRoutesWindow();
            //reportWindow.ShowDialog();
            //timer.Start();
        }


        //--------------------------------------------------------------------------------
        // Команда Двойной щелчок
        //--------------------------------------------------------------------------------
        private readonly ICommand _DblClickCommand = null;
        public ICommand DblClickCommand => _DblClickCommand ?? new LambdaCommand(OnDblClickCommandExecuted, CanDblClickCommand);
        private bool CanDblClickCommand(object p) => true;
        private void OnDblClickCommandExecuted(object p)
        {
            //timer.Stop();

            ////repo.LoadRouteOrders(SelectedOrder);

            //OrderWindowViewModel vm = new OrderWindowViewModel(SelectedOrder);
            //OrderWindow orderWindow = new OrderWindow();
            //orderWindow.DataContext = vm;
            ////(orderWindow.DataContext as OrderWindowViewModel).order = SelectedOrder;
            //if (orderWindow.ShowDialog() == true)
            //{
            //    repo.Save();
            //    OnFilterCommandExecuted(null);
            //}
            //timer.Start();
        }

        //--------------------------------------------------------------------------------
        // Команда Создать
        //--------------------------------------------------------------------------------
        private readonly ICommand _CreateCommand = null;
        public ICommand CreateCommand => _CreateCommand ?? new LambdaCommand(OnCreateCommandExecuted, CanCreateCommand);
        private bool CanCreateCommand(object p) => true;
        private void OnCreateCommandExecuted(object p)
        {
            SelectedTab.DataContext = new InControlViewModel();
            (SelectedTab.DataContext as InControlViewModel).CreateForm();
        }


        #endregion

    }
}
