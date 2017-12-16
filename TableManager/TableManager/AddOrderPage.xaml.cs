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
        Order _order = new Order();
        List<DishInOrder> _dishes = new List<DishInOrder>();
        ObservableCollection<Dish> availableDishes = new ObservableCollection<Dish>(UnitOfWork.Instance.Orders.GetDishes());
        ObservableCollection<Dish> displayDishes = new ObservableCollection<Dish>();


        public AddOrderPage()
        {
            InitializeComponent();
            comboBoxProducts.ItemsSource = availableDishes;
            comboBoxProducts.DisplayMemberPath = "Name";
            comboBoxProducts.SelectedValuePath = "Id";
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            //go back to main page
            NavigationService.Navigate(PageContainer.TablesPage);
        }

        private void buttonCompleteOrder_Click(object sender, RoutedEventArgs e)
        {
            //when all dishes are added "complete order" saves them and navigates to the main page
            NavigationService.Navigate(PageContainer.TablesPage);
        }

        private void buttonAddDish_Click(object sender, RoutedEventArgs e)
        {
            if (comboBoxProducts.SelectedIndex >= 0 || textBoxProductQuantity.Text != null 
                || int.TryParse(textBoxProductQuantity.Text, out int j) || j > 0)
            {
                try
                {
                    var dish = PageContainer.AddDish(int.Parse(comboBoxProducts.SelectedValue.ToString()),
                        int.Parse(textBoxProductQuantity.Text));
                    _order.OrderedDishes.Add(dish);
                    for (int i = 1; i < int.Parse(textBoxProductQuantity.Text); i++)
                    {
                        displayDishes.Add(comboBoxProducts.SelectedItem as Dish);
                    }
                }
                catch (InvalidOperationException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else MessageBox.Show("Specify the correct input of product and quantity.");
        }
    }
}
