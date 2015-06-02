using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AzureSearchTool
{
    public class SearchModel
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
        public string Url { get; set; }
        public Index Index { get; set; }

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
        public ObservableCollection<Index> Indexes
        {
            get { return _indexes; }
        }

        public async void Connect()
        {
            //https://maxmelcher.search.windows.net/indexes?api-version=2015-02-28

            string indexUrl = string.Format("https://{0}.{1}/indexes?api-version={2}", Service, BaseUrl, ApiVersion);

            WebClient client = GetWebClient();

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
    }

    public class Index
    {
        public string Name { get; set; }
        public List<Field> Fields { get; set; }
        
        //todo implement scoring profile
        //public List<string> ScoringProfiles { get; set; }

        //todo implement DefaultScoringProfile
        //public string DefaultScoringProfile { get; set; }
        
        //todo implement CorsOptions
        //public string CorsOptions { get; set; }

        //todo implement suggesters
        //public string Suggesters { get; set; }
    }

    public class Field
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public bool Searchable { get; set; }
        public bool Filterable { get; set; }
        public bool Retrievable { get; set; }
        public bool Sortable { get; set; }
        public bool Facetable { get; set; }
        public bool Key { get; set; }
        public string Analyzer { get; set; }
    }
}