using System;
using System.Collections.Generic;
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
        public TablesPage()
        {
            InitializeComponent();
            //List<Order> orders = new List<Order>
            //{
            //    new Order { Id = 1, OrderTime = new DateTime(2012, 12, 12), OrderedDishes = new List<DishInOrder> {
            //            new DishInOrder { DishID = 100, Dish = new Dish{ Name = "lol"} },
            //            new DishInOrder { DishID = 101, Dish = new Dish{ Name = "kek"}  } } },
            //    new Order { Id = 2, OrderTime = new DateTime(2013, 12, 12), OrderedDishes = new List<DishInOrder> {
            //            new DishInOrder { DishID = 200, Dish = new Dish{ Name = "lol"} },
            //            new DishInOrder { DishID = 201, Dish = new Dish{ Name = "kek"} } } }
            //};

            //UpdateOrdersList(orders);
            ////if(treeViewOrders.SelectedItem?.GetType() == typeof(int))
            ////{
            ////    MessageBox.Show("ok");
            ////}
            UnitOfWork.Instance.Tables.TableInfoHandler += CreateTablesGrid;
            //UnitOfWork.Instance.Tables.GetTableInfo();
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

        private void buttonCompleteOrder_Click(object sender, RoutedEventArgs e)
        {
            //actions taking place when order is completed (guests got all dishes and payed)
        }

        private void buttonDeleteOrder_Click(object sender, RoutedEventArgs e)
        {
            //actions taking place when order is cancelled
        }

        private void UpdateOrdersList(List<Order> orders)
        {
            //treeViewOrders.ItemsSource = orders;
        }
        public void CreateTablesGrid(int id, int numbetOfSeats, int x, int y)
        {
            #region UIModification
            Button table = new Button();
            if (x > DynamicGrid.ColumnDefinitions.Count)
            { 
                for (int i = DynamicGrid.ColumnDefinitions.Count; i <= x; i++)
                {
                    DynamicGrid.ColumnDefinitions.Add(new ColumnDefinition());
                }                
            }
            Grid.SetColumn(table, x);

            if (y > DynamicGrid.RowDefinitions.Count)
            {
                for (int i = DynamicGrid.RowDefinitions.Count; i <= y; i++)
                {
                    DynamicGrid.RowDefinitions.Add(new RowDefinition());
                }
            }
            Grid.SetRow(table, y);

            table.Tag = id;
            TextBlock info = new TextBlock();
            info.Text = $"{id}\nNumber of seats: {numbetOfSeats}";
            table.Content = info;
            #endregion
        }
    }
}
