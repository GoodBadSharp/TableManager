﻿using System;
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
        List<TableManageData.Table> tables = new List<TableManageData.Table>();

        public Action<int> CompleteOrder;

        public TablesPage()
        {
            InitializeComponent();
            UnitOfWork.Instance.Tables.TableInfoHandler += CreateTablesGrid;
            CompleteOrder += UnitOfWork.Instance.Orders.OrderComplete;
            UnitOfWork.Instance.Orders.UpdateTableByIdHandler += UpdateTable;
            UnitOfWork.Instance.Tables.GetTableInfo();
        }


        private void Page_Loaded(object sender, RoutedEventArgs e)
        {            
            CompleteOrder += TableSelectionChanged;
            PageContainer.AddOrderPage.GetCurrentTableIdCallback += GetCurrentTable;
            PageContainer.AddOrderPage.GetCurrentWaiterIdCallback += GetCurrentWaiter;
            PageContainer.AddOrderPage.PassChangedStatusIdHandler += ChangeCurrentTableColour;
            PageContainer.AddOrderPage.PassTableUpdateHandler += UpdateTable;            
        }


        private void buttonStatistics_Click(object sender, RoutedEventArgs e)  { NavigationService.Navigate(PageContainer.StatsPage); }

        private void UpdateTable(int tableId)
        {
            _tablesOrders = new ObservableCollection<Order>(UnitOfWork.Instance.Orders.GetActiveOrders(tableId));
            treeViewOrders.ItemsSource = _tablesOrders;
            if (_tablesOrders.Count != 0)
                ChangeTableColour(tableId, 2);
            else
                ChangeTableColour(tableId, 1);
        }

        private void buttonAddOrder_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedTablesId > 0)
                NavigationService.Navigate(PageContainer.AddOrderPage);
        }


        private void buttonEditOrder_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedTablesId > 0 && _selectedOrder.Id > 0)
                NavigationService.Navigate(PageContainer.EditOrderPage);
        }

         
        private void buttonCompleteOrder_Click(object sender, RoutedEventArgs e)
        {
            if (treeViewOrders.SelectedItem is Order)
            {
                Order selectedOrder = treeViewOrders.SelectedItem as Order;
                CompleteOrder?.Invoke(selectedOrder.Id);
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
                        case 1: table.Background = Brushes.AliceBlue; break; // 1: Vacant
                        case 2: table.Background = Brushes.YellowGreen; break; // 2: Occupied
                        case 3: table.Background = Brushes.LightYellow; break; // 3: Reserved
                    }
                    break;
                }
            }
        }        

        private void buttonDeleteOrder_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedOrder != null)
            {
                var result = MessageBox.Show("Do you wish to cancel this order?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                if (result == MessageBoxResult.Yes)
                {
                    UnitOfWork.Instance.Orders.CancelOrder(_selectedOrder.Id);
                    //UpdateTable(_selectedTablesId);
                }
            }
        }

        public void CreateTablesGrid(int id, int numbetOfSeats, int statusId, int x, int y)
        {
            #region UI Modification
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
            ChangeTableColour(id, statusId);
            DynamicGrid.Children.Add(table);
            #endregion
        }


        private void buttonTable_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            _selectedTablesId = (int)button.Tag;
            buttonEditOrder.IsEnabled = true;
            buttonDeleteOrder.IsEnabled = true;
            buttonCompleteOrder.IsEnabled = true;
            foreach (var table in tablesButtons)
            {
                table.BorderBrush = Brushes.White;
            }
            button.BorderBrush = Brushes.DarkSlateBlue;
            TableSelectionChanged((int)button.Tag);
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
            UpdateTable(id);
        }

        private void treeViewOrders_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            _selectedOrder = treeViewOrders.SelectedItem as Order;
        }
    }
}
