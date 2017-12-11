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
        }

        private void buttonStatistics_Click(object sender, RoutedEventArgs e)
        {
            //go to statistics page
            NavigationService.Navigate(new StatisticsPage());
        }

        private void buttonAddOrder_Click(object sender, RoutedEventArgs e)
        {
            //adding order
            NavigationService.Navigate(new AddOrderPage());
        }

        private void buttonEditOrder_Click(object sender, RoutedEventArgs e)
        {
            //editing order
            NavigationService.Navigate(new EditOrderPage());
        }

        private void buttonCompleteOrder_Click(object sender, RoutedEventArgs e)
        {
            //actions taking place when order is completed (guests got all dishes and payed)
        }

        private void buttonDeleteOrder_Click(object sender, RoutedEventArgs e)
        {
            //actions taking place when order is cancelled
        }
    }
}
