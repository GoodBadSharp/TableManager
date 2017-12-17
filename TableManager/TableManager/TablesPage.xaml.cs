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

        //public Action<int> CompleteOrder;
        //public Action<int> CancelOrder;

        public TablesPage()
        {
            InitializeComponent();
            //CompleteOrder += UnitOfWork.Instance.Orders.OrderComplete;
            //UnitOfWork.Instance.Orders.UpdateTableByIdHandler += UpdateTable;
            //UnitOfWork.Instance.Tables.UpdateTableByIdHandler += UpdateTable;
            List<TableManageData.Table> tables;
            using (var unitOfWork = new UnitOfWork())
            {
                unitOfWork.Tables.TableInfoHandler += CreateTablesGrid;
                tables = new List<TableManageData.Table>(unitOfWork.Tables.GetTableInfo());
            }
            tables.ForEach(t => CreateTablesGrid(t.Id, t.NumberOfSeats, t.Status_Id, t.X, t.Y));
        }


        private void Page_Loaded(object sender, RoutedEventArgs e)
        {            
            //CompleteOrder += TableSelectionChanged;
            PageContainer.AddOrderPage.GetCurrentTableIdCallback += GetCurrentTable;
            PageContainer.AddOrderPage.GetCurrentWaiterIdCallback += GetCurrentWaiter;
            PageContainer.AddOrderPage.PassChangedStatusIdHandler += ChangeCurrentTableColour;
            PageContainer.AddOrderPage.PassTableUpdateHandler += UpdateTable; 

            PageContainer.EditOrderPage.GetCurrentOrderCallback += GetCurrentOrder;
        }


        private void buttonStatistics_Click(object sender, RoutedEventArgs e)  { NavigationService.Navigate(PageContainer.StatsPage); }

        public void UpdateTable(int tableId)
        {
            using (var unitOfWork = new UnitOfWork())
            {
                _tablesOrders = new ObservableCollection<Order>(unitOfWork.Orders.GetActiveOrders(tableId));
                treeViewOrders.ItemsSource = _tablesOrders;
                ChangeTableColour(tableId, unitOfWork.Tables.GetTableStatusId(tableId));
            }
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
                using (var unitOfWork = new UnitOfWork())
                {
                    //unitOfWork.Orders.UpdateTableByIdHandler += UpdateTable;
                    unitOfWork.Orders.OrderComplete(selectedOrder.Id);
                    unitOfWork.SaveChanges();
                }
                UpdateTable(selectedOrder.Table_Id);
                //CompleteOrder?.Invoke(selectedOrder.Id);
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
                    Order selectedOrder = treeViewOrders.SelectedItem as Order;
                    using (var unitOfWork = new UnitOfWork())
                    {
                        //unitOfWork.Orders.UpdateTableByIdHandler += UpdateTable;
                        unitOfWork.Orders.CancelOrder(selectedOrder.Id);
                        unitOfWork.SaveChanges();
                    }
                    UpdateTable(selectedOrder.Table_Id);
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

        private void ActiveTableChanged()
        {
            int _selectedTablesStatusId;
            TableSelectionChanged(_selectedTablesId);
            using (var unitOfWork = new UnitOfWork())
            {
                _selectedTablesStatusId = unitOfWork.Tables.GetTableStatusId(_selectedTablesId);
            }
                if (_selectedTablesStatusId != 3)
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
        private Order GetCurrentOrder()
        {
            return _selectedOrder;
        }

        private void TableSelectionChanged(int id)
        {
            _selectedTablesId = id;
            UpdateTable(id);
        }

        private void treeViewOrders_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            try { _selectedOrder = treeViewOrders.SelectedItem as Order; }
            catch (Exception) { }
        }

        private void buttonReserve_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork())
                {
                    unitOfWork.Tables.ReserveOrCancelReservation(_selectedTablesId);
                    unitOfWork.SaveChanges();
                    ChangeCurrentTableColour(unitOfWork.Tables.GetTableStatusId(_selectedTablesId));
                }
                ActiveTableChanged();
            }
            catch (InvalidOperationException exc)
            { MessageBox.Show(exc.Message, "Warning", MessageBoxButton.OK, MessageBoxImage.Asterisk); }
        }
    }
}
