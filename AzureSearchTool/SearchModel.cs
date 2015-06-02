using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace AzureSearchTool
{
    public class SearchModel : INotifyPropertyChanged
    {
        private string _service = "MaxMelcher";
        private string _apiKey = "B98A05BCCACF2A0BA020FE6299CD4AA1";

        private string _apiVersion = "2015-02-28";
        private const string BaseUrl = "search.windows.net";

        public string Service
        {
            get { return _service; }
            set { _service = value; }
        }

        public string ApiKey
        {
            get { return _apiKey; }
            set { _apiKey = value; }
        }

        public string Filter { get; set; }

        public string Url
        {
            //https://maxmelcher.search.windows.net/indexes/twittersearch/docs?search=fifa&api-version=2015-02-28&$filter=Score gt 0.5&$top=25&$count=true
            get
            {
                if (Index == null)
                {
                    return "Index not valid";
                }
                return string.Format("https://{0}.{1}/indexes/{2}/docs?search={3}&api-version={4}", Service, BaseUrl, Index.Name, Search, ApiVersion);
            }
            set { _url = value; OnPropertyChanged("Url"); }
        }

        public string Search
        {
            get { return _search; }
            set { _search = value; OnPropertyChanged("Url"); }
        }


        public Index Index
        {
            get { return _index; }
            set { _index = value; OnPropertyChanged("Index"); OnPropertyChanged("Url"); }
        }

        public string ApiVersion
        {
            get { return _apiVersion; }
            set { _apiVersion = value; }
        }

        //todo create a dropdown for this
        public List<string> AvailableApiVersions = new List<string>()
        {
            "2014-07-31-Preview", "2014-10-20-Preview", "2015-02-28-Preview", "2015-02-28"
        };

        private ObservableCollection<Index> _indexes = new ObservableCollection<Index>();
        private string _error;
        private Index _index;
        private string _url;
        private string _search;

        public ObservableCollection<Index> Indexes
        {
            get { return _indexes; }
        }

        public async void Connect()
        {
            //https://maxmelcher.search.windows.net/indexes?api-version=2015-02-28

            string indexUrl = string.Format("https://{0}.{1}/indexes?api-version={2}", Service, BaseUrl, ApiVersion);

            WebClient client = GetWebClient();


            try
            {
                var jsonIndexes = await client.DownloadStringTaskAsync(new Uri(indexUrl));

                //{'@odata.context': 'https://maxmelcher.search.windows.net/$metadata#indexes','value':''}
                var dummy = new
                {
                    value = new Index[] { }
                };

                var result = JsonConvert.DeserializeAnonymousType(jsonIndexes, dummy);
                Indexes.Clear();

                foreach (Index index in result.value)
                {
                    Indexes.Add(index);
                }
                Error = "";
            }
            catch (Exception ex)
            {
                Error = ex.Message;
                Indexes.Clear();
                Index = null;
            }
        }

        public string Error
        {
            get { return _error; }
            set { _error = value; OnPropertyChanged("Error"); }
        }

        private WebClient GetWebClient()
        {
            var client = new WebClient();
            client.Headers.Add("Content-Type", "application/json; charset=utf-8");
            client.Headers.Add("api-key", ApiKey);
            return client;
        }

        public void SelectIndex(Index index)
        {
            Index = index;
        }



        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}