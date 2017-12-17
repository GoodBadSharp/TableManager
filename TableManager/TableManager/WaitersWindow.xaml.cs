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
using System.Windows.Shapes;
using TableManageData;

namespace TableManager
{
    /// <summary>
    /// Логика взаимодействия для WaitersWindow.xaml
    /// </summary>
    public partial class WaitersWindow : Window
    {
        List<Waiter> _waiters;
        List<Button> _waiterButtons = new List<Button>();
        public event Action<Waiter> PassAuthorizedWaiterHandler;

        private const uint MF_BYCOMMAND = 0x00000000;
        private const uint MF_GRAYED = 0x00000001;
        private const uint SC_CLOSE = 0xF060;

        public WaitersWindow(IEnumerable<Waiter> waiters)
        {
            InitializeComponent();
            this._waiters = new List<Waiter>(waiters);
            CreateAuthGrid();
        }

        private void CreateAuthGrid()
        {
            DynamicGrid.ColumnDefinitions.Add(new ColumnDefinition());
            DynamicGrid.ColumnDefinitions.Add(new ColumnDefinition());

            for(int rowCount = 1; 2 * rowCount <= _waiters.Count; rowCount++)
            {
                if (rowCount > 1) { ResizeWindow(DynamicGrid); }
                DynamicGrid.RowDefinitions.Add(new RowDefinition());
            }

            for (int i = 0; i < _waiters.Count; i++) 
            {
                Button waiterAuthButton = new Button
                {
                    Name = $"button{_waiters[i].Id}Waiter",
                    Content = $"{_waiters[i].Id}",
                    Tag = _waiters[i].Id,
                    Height = 50,
                    Width = 50,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    VerticalContentAlignment = VerticalAlignment.Center
                };
                waiterAuthButton.Click += WaiterAuthorization;
                Grid.SetRow(waiterAuthButton, i / 2);
                Grid.SetColumn(waiterAuthButton, i % 2);
                DynamicGrid.Children.Add(waiterAuthButton);
                _waiterButtons.Add(waiterAuthButton);
            }
        }

        private void ResizeWindow(UIElement dimensionSource)
        {
            this.Height += dimensionSource.RenderSize.Height;
            DynamicGrid.Height += dimensionSource.RenderSize.Height;
        }

        private void WaiterAuthorization(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            int waiterId = (int)button.Tag;
            PassAuthorizedWaiterHandler?.Invoke(_waiters.Single(w => w.Id == waiterId));
        }
    }
}
