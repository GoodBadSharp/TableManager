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
    /// Логика взаимодействия для AddOrderPage.xaml
    /// </summary>
    public partial class AddOrderPage : Page
    {
        public event Func<int> GetCurrentTableIdCallback;
        public event Func<int> GetCurrentWaiterIdCallback;
        public event Action<int> PassChangedStatusIdHandler;
        public event Action<int> PassTableUpdateHandler;

        int _currentTableId;
        Order _order = new Order();
        List<Dish> availableDishes = new List<Dish>(UnitOfWork.Instance.Orders.GetDishes());
        ObservableCollection<Dish> _displayedDishes = new ObservableCollection<Dish>();
        List<DishInOrder> _orderDishes = new List<DishInOrder>();

        public AddOrderPage()
        {         
            InitializeComponent();      
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            _order = new Order();
            comboBoxProducts.ItemsSource = availableDishes;
            comboBoxProducts.DisplayMemberPath = "Name";
            comboBoxProducts.SelectedValuePath = "Id";
            listBoxDishes.ItemsSource = _displayedDishes;
            listBoxDishes.DisplayMemberPath = "Name";
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            _order = new Order();
            _displayedDishes.Clear();
            _orderDishes.Clear();
            NavigationService.Navigate(PageContainer.TablesPage);
        }

        private void buttonCompleteAddingOrder_Click(object sender, RoutedEventArgs e)
        {
            if (_orderDishes.Count > 0)
            {
                _currentTableId = GetCurrentTableIdCallback.Invoke();
                _order.Table_Id = _currentTableId;
                _order.Waiter_Id = GetCurrentWaiterIdCallback.Invoke();
                _order.OrderedDishes = _orderDishes;
                _order.OrderTime = DateTime.Today;
                PassTableUpdateHandler?.Invoke(_order.Table_Id);
                UnitOfWork.Instance.Orders.AddOrder(_order);

                _order = null;
                _displayedDishes.Clear();
                _orderDishes.Clear();
                PassChangedStatusIdHandler?.Invoke(2);
                NavigationService.Navigate(PageContainer.TablesPage);
            }
            else MessageBox.Show("Cannot add empty order. Add dishes or cancel order", "Warning", 
                MessageBoxButton.OK, MessageBoxImage.Asterisk);
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
                    Dish selectedDish = availableDishes.SingleOrDefault(d => d.Id == int.Parse(comboBoxProducts.SelectedValue.ToString()));
                    if (_orderDishes.SingleOrDefault(od => od.DishID == selectedDish.Id) == null)
                    {
                        DishInOrder dishes = PageContainer.AddDish(selectedDish.Id, int.Parse(textBoxProductQuantity.Text));
                        _orderDishes.Add(dishes);
                        for (int i = 0; i < int.Parse(textBoxProductQuantity.Text); i++)
                        {
                            _displayedDishes.Add(comboBoxProducts.SelectedItem as Dish);
                        }
                    }
                    else
                    {
                        _orderDishes.SingleOrDefault(od => od.DishID == selectedDish.Id).Quantity += int.Parse(textBoxProductQuantity.Text);
                        for (int i = 0; i < int.Parse(textBoxProductQuantity.Text); i++)
                        {
                            _displayedDishes.Add(comboBoxProducts.SelectedItem as Dish);
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

        private void buttonDeleteDish_Click(object sender, RoutedEventArgs e)
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


    }
}
