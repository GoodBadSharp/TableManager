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
        public event Func<int> GetCurrentTableIdCallback;
        public event Func<int> GetCurrentWaiterIdCallback;
        public event Func<Order> GetCurrentOrderCallback;

        List<Dish> availableDishes;
        ObservableCollection<Dish> _displayedDishes = new ObservableCollection<Dish>();
        List<DishInOrder> _orderDishes = new List<DishInOrder>();
        ObservableCollection<Dish> displayDishes = new ObservableCollection<Dish>();
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
            //GetCurrentTable += PageContainer.TablesPage.GetCurrentTable;
            comboBoxProducts.ItemsSource = availableDishes;
            comboBoxProducts.DisplayMemberPath = "Name";
            comboBoxProducts.SelectedValuePath = "Id";
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
        }

        private void buttonCompleteOrder_Click(object sender, RoutedEventArgs e)
        {
            //save all changes and go back to the main page
            NavigationService.Navigate(PageContainer.TablesPage);
        }

        private void buttonDeleteDish_Click(object sender, RoutedEventArgs e)
        {
            //deleting dish
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            //navigates back to the main page
            NavigationService.Navigate(PageContainer.TablesPage);
        }

        private void listBoxDishes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            buttonDeleteDish.IsEnabled = true;
        }
    }
}
