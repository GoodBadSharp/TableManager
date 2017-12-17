using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TableManageData;
using TableManagerData;

namespace TableManager
{
    /// <summary>
    /// Логика взаимодействия для TablesPage.xaml
    /// </summary>
    public partial class TablesPage : Page
    {
        ObservableCollection<Order> _tablesOrders;
        Order _selectedOrder;
        int _selectedTablesId = -1;
        int _waiterID = 1;
        List<Button> tablesButtons = new List<Button>();

        public Action<int> CompleteOrder;
        public Action<int> CancelOrder;

        public TablesPage()
        {
            InitializeComponent();
            UnitOfWork.Instance.Tables.TableInfoHandler += CreateTablesGrid;
            CompleteOrder += UnitOfWork.Instance.Orders.OrderComplete;
            CancelOrder += UnitOfWork.Instance.Orders.CancelOrder;
            UnitOfWork.Instance.Tables.GetTableInfo();
        }


        private void Page_Loaded(object sender, RoutedEventArgs e)
        {            
            CompleteOrder += TableSelectionChanged;
            PageContainer.AddOrderPage.GetCurrentTableIdCallback += GetCurrentTable;
            PageContainer.AddOrderPage.GetCurrentWaiterIdCallback += GetCurrentWaiter;
            PageContainer.AddOrderPage.PassChangedStatusIdHandler += ChangeCurrentTableColour;
            PageContainer.AddOrderPage.PassAddedOrderHandler += AddOrderToCurrentTable;            
        }


        private void buttonStatistics_Click(object sender, RoutedEventArgs e)  { NavigationService.Navigate(PageContainer.StatsPage); }

        private void AddOrderToCurrentTable(Order order)
        {
            _tablesOrders.Add(order);
        }

        private void buttonAddOrder_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedTablesId > 0)
                NavigationService.Navigate(PageContainer.AddOrderPage);
        }


        private void buttonEditOrder_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(PageContainer.EditOrderPage);
        }


        /// <summary>
        /// Actions taking place when order is completed (guests got all dishes and payed).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonCompleteOrder_Click(object sender, RoutedEventArgs e)
        {
            if (treeViewOrders.SelectedItem is Order)
            {
                var item = treeViewOrders.SelectedItem as Order;
                CompleteOrder?.Invoke(item.Id);
            }               
        }

        private void ChangeCurrentTableColour(int tableStatusId)
        {
            ChangeTableColour(_selectedTablesId, tableStatusId);
        }

        private void ChangeTableColour(int tableId, int tableStatusId)
        {
            foreach (var table in tablesButtons)
            {
                if (int.Parse(table.Tag.ToString()) == tableId)
                {
                    switch (tableStatusId)
                    {
                        case 1: table.Background = Brushes.AliceBlue; break;
                        case 2: table.Background = Brushes.YellowGreen; break;
                        case 3: table.Background = Brushes.LightYellow; break;
                    }
                    break;
                }
            }
        }        

        private void buttonDeleteOrder_Click(object sender, RoutedEventArgs e)//actions taking place when order is cancelled
        {
            UnitOfWork.Instance.Orders.CancelOrder(_selectedOrder.Id);
        }

        public void CreateTablesGrid(int id, int numbetOfSeats, int statusId, int x, int y)
        {
            #region UIModification
            Button table = new Button { Name = $"table{id}Button", Tag = id,
                Content = $"Table {id} \n {numbetOfSeats} Seats",
                Height = 50, Width = 60,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
                BorderThickness = new Thickness(3),
                BorderBrush = Brushes.White
            };

            if (x+1 > DynamicGrid.ColumnDefinitions.Count)
            { 
                for (int i = DynamicGrid.ColumnDefinitions.Count; i <= x; i++)
                {
                    DynamicGrid.ColumnDefinitions.Add(new ColumnDefinition());
                }                
            }
            Grid.SetColumn(table, x);

            if (y+1 > DynamicGrid.RowDefinitions.Count)
            {
                for (int i = DynamicGrid.RowDefinitions.Count; i <= y; i++)
                {
                    DynamicGrid.RowDefinitions.Add(new RowDefinition());
                }
            }

            Grid.SetRow(table, y);           
            table.Click += buttonTable_Click;
            tablesButtons.Add(table);
            ChangeTableColour(int.Parse(table.Tag.ToString()), statusId);
            DynamicGrid.Children.Add(table);
            #endregion
        }

        private void ActiveTableChanged()
        {
            TableSelectionChanged(_selectedTablesId);
            if (UnitOfWork.Instance.Tables.GetTableStatusId(_selectedTablesId) != 3)
            {
                buttonAddOrder.IsEnabled = true;
                buttonEditOrder.IsEnabled = true;
                buttonDeleteOrder.IsEnabled = true;
                buttonCompleteOrder.IsEnabled = true;
            }
            else
            {
                buttonAddOrder.IsEnabled = false;
                buttonEditOrder.IsEnabled = false;
                buttonDeleteOrder.IsEnabled = false;
                buttonCompleteOrder.IsEnabled = false;
            }
            foreach (var table in tablesButtons)
            {
                if ((int)table.Tag == _selectedTablesId) table.BorderBrush = Brushes.DarkSlateBlue;
                else table.BorderBrush = Brushes.White;
            }
        }

        private void buttonTable_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            _selectedTablesId = (int)button.Tag;
            ActiveTableChanged();
        }

        private int GetCurrentTable()
        {
            return _selectedTablesId;
        }

        private int GetCurrentWaiter()
        {
            return _waiterID;
        }

        private void TableSelectionChanged(int id)
        {
            _selectedTablesId = id;
            _tablesOrders = new ObservableCollection<Order>(UnitOfWork.Instance.Orders.GetActiveOrders(id));
            treeViewOrders.ItemsSource = _tablesOrders;
        }

        private void treeViewOrders_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            try { _selectedOrder = treeViewOrders.SelectedItem as Order; }
            catch (Exception) { }
        }

        private void buttonReserve_Click(object sender, RoutedEventArgs e)
        {
            UnitOfWork.Instance.Tables.ReserveOrCancelReservation(_selectedTablesId);
            var res = UnitOfWork.Instance.Tables.GetTableStatusId(_selectedTablesId);
            ChangeCurrentTableColour(UnitOfWork.Instance.Tables.GetTableStatusId(_selectedTablesId));
            ActiveTableChanged();
        }
    }
}
