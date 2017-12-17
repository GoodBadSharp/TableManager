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
        public event Action<Order> PassAddedOrderHandler;

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

        private void buttonCompleteOrder_Click(object sender, RoutedEventArgs e)
        {
            _currentTableId = GetCurrentTableIdCallback.Invoke();
            _order.Table_Id = _currentTableId;
            _order.Waiter_Id = GetCurrentWaiterIdCallback.Invoke();
            _order.OrderedDishes = _orderDishes;
            PassAddedOrderHandler?.Invoke(_order);
            UnitOfWork.Instance.Orders.AddOrder(_order);

            _order = new Order();
            _displayedDishes.Clear();
            _orderDishes.Clear();
            PassChangedStatusIdHandler?.Invoke(2);
            NavigationService.Navigate(PageContainer.TablesPage);
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
                    DishInOrder dish = PageContainer.AddDish(int.Parse(comboBoxProducts.SelectedValue.ToString()),
                        int.Parse(textBoxProductQuantity.Text));
                    _orderDishes.Add(dish);

                    for (int i = 0; i < int.Parse(textBoxProductQuantity.Text); i++)
                    {
                        _displayedDishes.Add(comboBoxProducts.SelectedItem as Dish);
                    }

                }
                catch (InvalidOperationException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else MessageBox.Show("Specify correct input for a dish and its quantity.");
        }


    }
}
