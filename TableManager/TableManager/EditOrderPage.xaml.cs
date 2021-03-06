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
    /// Логика взаимодействия для EditOrderPage.xaml
    /// </summary>
    public partial class EditOrderPage : Page
    {
        public event Func<Order> GetCurrentOrderCallback;
        public event Action<int> PassTableUpdateHandler;
        int tableId;
        int waiterId;
        Order currentOrder;
        Order orderOld;

        List<Dish> availableDishes;
        ObservableCollection<Dish> _displayedDishes;
        List<DishInOrder> _orderDishes = new List<DishInOrder>();

        public EditOrderPage()
        {
            InitializeComponent();

            using (var unitOfWork = new UnitOfWork())
            {
                availableDishes = new List<Dish>(unitOfWork.Orders.GetDishes());
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            
            try
            {
                currentOrder = GetCurrentOrderCallback.Invoke();
                orderOld = GetCurrentOrderCallback.Invoke();
                tableId = currentOrder.Table_Id;
                waiterId = currentOrder.Waiter_Id;
                _orderDishes = currentOrder.OrderedDishes.ToList();
                _displayedDishes = new ObservableCollection<Dish>();
                foreach (var dishes in _orderDishes)
                {
                    for (int i = 0; i < dishes.Quantity; i++)
                    {
                        _displayedDishes.Add(dishes.Dish);
                    }
                }
            }
            catch
            {
                throw new Exception("An error occured while passing information to editing page");
            }            
            comboBoxProducts.ItemsSource = availableDishes;
            comboBoxProducts.DisplayMemberPath = "Name";
            comboBoxProducts.SelectedValuePath = "Id";
            listBoxDishes.ItemsSource = _displayedDishes;
        }

        private void buttonAddDish_Click(object sender, RoutedEventArgs e)
        {
            if (comboBoxProducts.SelectedValue != null
                && textBoxProductQuantity.Text != null
                && int.TryParse(textBoxProductQuantity.Text, out int j)
                && j > 0)
            {
                try
                {
                    Dish selectedDish = availableDishes.SingleOrDefault
                        (d => d.Id == int.Parse(comboBoxProducts.SelectedValue.ToString()));
                    var dishForAdding = new DishInOrder();
                    var dishForDisplaying = new DishInOrder();

                    dishForAdding.Quantity = int.Parse(textBoxProductQuantity.Text);
                    dishForAdding.DishID = selectedDish.Id;
                    dishForAdding.OrderID = currentOrder.Id;

                    dishForDisplaying.Dish = selectedDish;
                    dishForDisplaying.Order = currentOrder;
                    dishForDisplaying.Quantity = int.Parse(textBoxProductQuantity.Text);

                    if (_orderDishes.SingleOrDefault(od => od.DishID == selectedDish.Id) == null)
                    {
                        _orderDishes.Add(dishForAdding);
                        for (int i = 0; i < dishForDisplaying.Quantity; i++)
                        {
                            _displayedDishes.Add(dishForDisplaying.Dish);
                        }
                    }
                    else
                    {
                        _orderDishes.SingleOrDefault(od => od.DishID == selectedDish.Id).Quantity 
                            += int.Parse(textBoxProductQuantity.Text);
                        for (int i = 0; i < int.Parse(textBoxProductQuantity.Text); i++)
                        {
                            _displayedDishes.Add(dishForDisplaying.Dish);
                        }
                    }
                }
                catch (InvalidOperationException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else MessageBox.Show("Specify correct input for a dish and its quantity.", "Warning", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }

        private void buttonCompleteOrder_Click(object sender, RoutedEventArgs e)//save all changes and go back to the main page
        {           
            if (_orderDishes.Count > 0)
            {
                try
                {
                    currentOrder.OrderedDishes = _orderDishes;
                    currentOrder.OrderTime = DateTime.Today;

                    foreach (var item in currentOrder.OrderedDishes)
                    {
                        item.Dish = null;
                    }

                    using (var unitOfWork = new UnitOfWork())
                    {
                        unitOfWork.Orders.UpdateOrder(currentOrder, orderOld);
                        unitOfWork.SaveChanges();
                    }

                    PassTableUpdateHandler?.Invoke(currentOrder.Table_Id);
                    currentOrder = null;
                    _displayedDishes.Clear();
                    _orderDishes.Clear();
                    NavigationService.Navigate(PageContainer.TablesPage);
                }
                catch (Exception) { MessageBox.Show("Unknown exception on completing order", "Warning", 
                    MessageBoxButton.OK, MessageBoxImage.Asterisk); }
            }
            else MessageBox.Show("Cannot add empty order. Add dishes or cancel order", "Warning", 
                MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }

        private void buttonDeleteDish_Click(object sender, RoutedEventArgs e) //deleting dish
        {
            try
            {
                if (listBoxDishes.SelectedItem is Dish)
                {
                    Dish selectedDish = listBoxDishes.SelectedItem as Dish;
                    if (_orderDishes.SingleOrDefault(dio => dio.DishID == selectedDish.Id).Quantity > 1)
                    {
                        _orderDishes.SingleOrDefault(dio => dio.DishID == selectedDish.Id).Quantity--;
                        _displayedDishes.Remove(selectedDish);
                    }
                    else
                    {
                        _orderDishes.Remove(_orderDishes.SingleOrDefault(dio => dio.DishID == selectedDish.Id));
                        _displayedDishes.Remove(selectedDish);
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Unknown exception on completing order", "Warning",
                    MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e) //navigates back to the main page
        {          
            NavigationService.Navigate(PageContainer.TablesPage);
        }

        private void listBoxDishes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            buttonDeleteDish.IsEnabled = true;
        }
    }
}
