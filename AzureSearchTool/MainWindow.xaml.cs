using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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
            get { return _model ?? (_model = new SearchModel()); }
        }

        public MainWindow()
        {
            InitializeComponent();

            this.Title = "AzureSearchTool - (v." + Assembly.GetExecutingAssembly().GetName().Version + ")";

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

        private void Connect(object sender, RoutedEventArgs e)
        {
            Model.Connect();
        }

        private void SelectIndex(object sender, RoutedEventArgs e)
        {
            Index index;
            if (Model.IsAdminApiKey)
            {
                index = (Index) GridAvailableIndexes.SelectedItem;
            }
            else
            {
                index = new Index();
                index.IsResolved = false;
                index.Name = Model.IndexName;
            }
            Model.SelectIndex(index);
            Flyout.IsOpen = false;
        }

        private void Search(object sender, RoutedEventArgs e)
        {
            try
            {
                //set the progressbar
                ProgressBar.IsIndeterminate = true;

                GridSearchResults.AutoGenerateColumns = false;
                GridSearchResults.Columns.Clear();

                //todo make this async
                Model.Search();
                GridSearchResults.AutoGenerateColumns = true;
                ProgressBar.IsIndeterminate = false;

            }
            catch (Exception)
            {
                //todo handle this
                ProgressBar.IsIndeterminate = false;
                throw;
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
            About about = new About();
            about.ShowDialog();
        }

        private void SearchMode_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Model.SearchType = (SearchModel.SearchTypes)tabcontrolSearchMode.SelectedIndex;

            if (Model.SearchType == SearchModel.SearchTypes.Suggest)
            {
                tabResults.SelectedIndex = 2;
            }
        }

        private void Suggestion_KeyUp(object sender, KeyEventArgs e)
        {

            Model.SuggestionResults.Clear();
            GridSuggestionResults.AutoGenerateColumns = false;
            Model.Search();
            GridSuggestionResults.AutoGenerateColumns = true;
        }
    }
}
