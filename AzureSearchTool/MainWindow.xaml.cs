using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AzureSearchTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private SearchModel _model;

        public SearchModel Model
        {
            get
            {
                if (_model == null)
                    _model = new SearchModel();
                return _model;
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            Grid.DataContext = Model;
            Flyout.DataContext = Model;

            OpenConnectionFlyout();
        }

        private void OpenConnectionFlyout()
        {
            Task.Delay(1000).ContinueWith(action =>
            {
                Dispatcher.Invoke(() =>
                {
                    Flyout.IsOpen = true;
                });
            });
        }

        private void MenuItem_Index_OnClick(object sender, RoutedEventArgs e)
        {
            Flyout.IsOpen = true;
        }

        private async void Connect(object sender, RoutedEventArgs e)
        {
             Model.Connect();
        }

        private void SelectIndex(object sender, RoutedEventArgs e)
        {
            Index index = (Index) GridAvailableIndexes.SelectedItem;
            Model.SelectIndex(index);
            Flyout.IsOpen = false;
        }

        private void Search(object sender, RoutedEventArgs e)
        {
            try
            {
                ProgressBar.IsIndeterminate = true;
                GridSearchResults.AutoGenerateColumns = false;
                GridSearchResults.Columns.Clear();
                Model.Search();
                GridSearchResults.AutoGenerateColumns = true;
                
                
            }
            catch(Exception ex)
            {
                //todo handle this
                throw ex;
            }
            finally
            {
                ProgressBar.IsIndeterminate = false;
            }
        }

        private void Searchbox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                Model.Search();
            }
        }

        private void FilterExample(object sender, RoutedEventArgs e)
        {
            Model.Filter = "Score gt 0.5";
        }

        private void MenuItem_About_OnClick(object sender, RoutedEventArgs e)
        {

        }
    }
}
