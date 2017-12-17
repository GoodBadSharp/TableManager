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
using TableManagerData;
using TableManagerData.QueryLogic;

namespace TableManager
{
    /// <summary>
    /// Логика взаимодействия для StatisticsPage.xaml
    /// </summary>
    public partial class StatisticsPage : Page
    {
        ObservableCollection<QueryResult> listViewContent;
        public StatisticsPage()
        {
            InitializeComponent();
            using (var unitOfWork = new UnitOfWork())
            {
                comboBoxStatisticsType.ItemsSource = unitOfWork.Queries.GetQueryInfo();
            }
            comboBoxStatisticsType.SelectedValuePath = "QueryID";
            comboBoxStatisticsType.DisplayMemberPath = "Description";
            //UnitOfWork.Instance.Queries.QueryCollectionHandler += UpdateStatsComboBox;
            //UnitOfWork.Instance.Queries.GetSpecifiedFromDateCallback += GetFromDate;
            //UnitOfWork.Instance.Queries.GetSpecifiedTillDateCallback += GetTillDate;
            //UnitOfWork.Instance.Queries.UpdateTableHeadersHandler += UpdateTableHeaders;
            //UnitOfWork.Instance.Queries.QueryResultHandler += FillTable;

            //UnitOfWork.Instance.Queries.GetQueryInfo();
        }


        private void UpdateStatsComboBox(IEnumerable<QueryContainer> queries, string queryId, string queryDesc )
        {
            comboBoxStatisticsType.ItemsSource = queries;
            comboBoxStatisticsType.DisplayMemberPath = queryDesc;
            comboBoxStatisticsType.SelectedValuePath = queryId;
        }


        private void buttonBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(PageContainer.TablesPage);
        }


        private void buttonQuery_Click(object sender, RoutedEventArgs e)
        {
            if (comboBoxStatisticsType.SelectedValue != null)
            {
                try
                {
                    using (var unitOfWork = new UnitOfWork())
                    {
                        unitOfWork.Queries.GetSpecifiedFromDateCallback += GetFromDate;
                        unitOfWork.Queries.GetSpecifiedTillDateCallback += GetTillDate;
                        unitOfWork.Queries.UpdateTableHeadersHandler += UpdateTableHeaders;
                        unitOfWork.Queries.QueryResultHandler += FillTable;
                        unitOfWork.Queries.ConductQuery(int.Parse(comboBoxStatisticsType.SelectedValue.ToString()));
                    }
                }
                catch (NotImplementedException exc) { MessageBox.Show(exc.Message, "Warning", MessageBoxButton.OK, MessageBoxImage.Asterisk); }
                catch (InvalidOperationException exc) { MessageBox.Show(exc.Message, "Warning", MessageBoxButton.OK, MessageBoxImage.Asterisk); }
                catch (Exception) { MessageBox.Show("Unknown error occured while conducting query", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation); }
            }
        }


        private void UpdateTableHeaders(IEnumerable<string> headers, IEnumerable<string> bindings)
        {
            try
            {
                var gridView = new GridView();
                statsListView.View = gridView;
                var headerParameters = headers.Zip(bindings, (h, b) => new { Header = h, Binding = b });

                foreach (var p in headerParameters)
                    gridView.Columns.Add(new GridViewColumn { Header = p.Header, DisplayMemberBinding = new Binding(p.Binding) });
            }
            catch { throw new InvalidOperationException("Number of headers must match number of bindings"); }

            statsListView.ItemsSource = null;
        }


        private void FillTable(IEnumerable<QueryResult> content)
        {
            listViewContent = new ObservableCollection<QueryResult>(content);
            statsListView.ItemsSource = listViewContent;
        }


        private DateTime? GetFromDate()
        {
           return fromDatePicker.SelectedDate;
        }


        private DateTime? GetTillDate()
        {
            return tillDatePicker.SelectedDate;
        }
    }
}
