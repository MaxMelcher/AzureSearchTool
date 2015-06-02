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

            Model.Url = "https://maxmelcher.search.windows.net/indexes/twittersearch/docs?search=fifa&api-version=2015-02-28&$filter=Score gt 0.5&$top=25&$count=true";
        }

        private void MenuItem_Index_OnClick(object sender, RoutedEventArgs e)
        {
            Flyout.IsOpen = true;
        }
    }

    public class SearchModel
    {
        public string Filter { get; set; }
        public string Url { get; set; }
        public string Index { get; set; }
    }
}
