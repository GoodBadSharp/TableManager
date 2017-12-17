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
    /// Логика взаимодействия для EditOrderPage.xaml
    /// </summary>
    public partial class EditOrderPage : Page
    {
        public event Func<Order> GetCurrentOrderCallback;
        public event Action<int> PassTableUpdateHandler;
        int tableId;
        int waiterId;
        Order currentOrder;

        List<Dish> availableDishes;
        ObservableCollection<DishInOrder> _displayedDishes;
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
                tableId = currentOrder.Table_Id;
                waiterId = currentOrder.Waiter_Id;
                _orderDishes = currentOrder.OrderedDishes.ToList();
                _displayedDishes = new ObservableCollection<DishInOrder>();
                foreach (var dish in _orderDishes)
                {
                    for (int i = 0; i < dish.Quantity; i++)
                    {
                        _displayedDishes.Add(dish);
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
                    dishForAdding.Dish = selectedDish;
                    dishForAdding.Order = currentOrder;
                    dishForAdding.Quantity = int.Parse(textBoxProductQuantity.Text);
                    dishForAdding.DishID = selectedDish.Id;
                    dishForAdding.OrderID = currentOrder.Id;

                    if (_orderDishes.SingleOrDefault(od => od.DishID == selectedDish.Id) == null)
                    {
                        _orderDishes.Add(dishForAdding);
                        for (int i = 0; i < dishForAdding.Quantity; i++)
                        {
                            _displayedDishes.Add(dishForAdding);
                        }
                    }
                    else
                    {
                        _orderDishes.SingleOrDefault(od => od.DishID == selectedDish.Id).Quantity += int.Parse(textBoxProductQuantity.Text);
                        for (int i = 0; i < int.Parse(textBoxProductQuantity.Text); i++)
                        {
                            _displayedDishes.Add(dishForAdding);
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
                currentOrder.OrderedDishes = _orderDishes;
                currentOrder.OrderTime = DateTime.Today;
                PassTableUpdateHandler?.Invoke(currentOrder.Table_Id);
                //UnitOfWork.Instance.Orders.UpdateOrder(currentOrder);

                currentOrder = null;
                _displayedDishes.Clear();
                _orderDishes.Clear();
                NavigationService.Navigate(PageContainer.TablesPage);
            }
            else MessageBox.Show("Cannot add empty order. Add dishes or cancel order", "Warning", 
                MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }

        private void buttonDeleteDish_Click(object sender, RoutedEventArgs e) //deleting dish
        {
            var dish = listBoxDishes.SelectedItem as DishInOrder;
            _orderDishes.Remove(dish);
            _displayedDishes.Remove(dish);
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
