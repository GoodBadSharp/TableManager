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
        ObservableCollection<Order> _tablesOrdes;
        int _waiterID = 1;

        public Action<int> CompleteOrder;
        int activeTableId;

        public TablesPage()
        {
            InitializeComponent();
            UnitOfWork.Instance.Tables.TableInfoHandler += CreateTablesGrid;
            CompleteOrder += UnitOfWork.Instance.Orders.OrderComplete;
            CompleteOrder += TableSelectionChanged;

            UnitOfWork.Instance.Tables.GetTableInfo();
        }


        private void buttonStatistics_Click(object sender, RoutedEventArgs e)
        {
            //go to statistics page
            NavigationService.Navigate(PageContainer.StatsPage);
        }


        private void buttonAddOrder_Click(object sender, RoutedEventArgs e)
        {
            //adding order
            NavigationService.Navigate(PageContainer.AddOrderPage);
        }


        private void buttonEditOrder_Click(object sender, RoutedEventArgs e)
        {
            //editing order
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


        private void buttonDeleteOrder_Click(object sender, RoutedEventArgs e)
        {
            //actions taking place when order is cancelled
        }


        private void RefreshOrdersList(List<Order> orders)
        {
            //treeViewOrders.ItemsSource = orders;
        }


        public void CreateTablesGrid(int id, int numbetOfSeats, int x, int y)
        {
            #region UIModification
            Button table = new Button { Name=$"table{id}Button", Tag = id, Content = $"Table {id} \n {numbetOfSeats} Seats",
                Height = 50, Width = 60,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
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

            //table.Tag = id;
            //TextBlock info = new TextBlock();
            //info.Text = $"{id}\nNumber of seats: {numbetOfSeats}";
            //table.Content = info;
            DynamicGrid.Children.Add(table);
            #endregion
            table.Click += buttonTable_Click;           
        }


        private void buttonTable_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            TableSelectionChanged((int)button.Tag);
        }

        public int GetActiveTable()
        {
            return activeTableId;
        }

        private void TableSelectionChanged(int id)
        {
            // for testing purposes all table's orders are displayed!
            _tablesOrdes = new ObservableCollection<Order>(UnitOfWork.Instance.Orders.GetActiveOrders(id));
            treeViewOrders.ItemsSource = _tablesOrdes;
        }
    }
}
