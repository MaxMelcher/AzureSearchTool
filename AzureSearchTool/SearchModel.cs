using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
            set
            {
                _apiKey = value; OnPropertyChanged("ApiKey");
                OnPropertyChanged("Url");
            }
        }

        private string _filter = "";
        public string Filter
        {
            get
            {
                return _filter;
            }
            set
            {
                _filter = value;
                OnPropertyChanged("Filter");
                OnPropertyChanged("Url");
            }
        }

        public string Url
        {
            //https://maxmelcher.search.windows.net/indexes/twittersearch/docs?search=fifa&api-version=2015-02-28&$filter=Score gt 0.5&$top=25&$count=true
            get
            {
                if (Index == null)
                {
                    return "Index not valid";
                }

                var url = string.Format("https://{0}.{1}/indexes/{2}/docs?search={3}&api-version={4}", Service, BaseUrl, Index.Name, SearchQuery, ApiVersion);

                if (!string.IsNullOrEmpty(Top))
                {
                    url += string.Format("&$top={0}", Top);
                }

                if (!string.IsNullOrEmpty(Skip))
                {
                    url += string.Format("&$skip={0}", Skip);
                }

                if (!string.IsNullOrEmpty(Filter))
                {
                    url += string.Format("&$filter={0}", Filter);
                }

                if (!string.IsNullOrEmpty(SearchMode))
                {
                    url += string.Format("&searchMode={0}", SearchMode);
                }

                if (!string.IsNullOrEmpty(SearchFields))
                {
                    url += string.Format("&searchFields={0}", SearchFields);
                }

                if (!string.IsNullOrEmpty(Count))
                {
                    url += string.Format("&$count={0}", Count);
                }

                if (!string.IsNullOrEmpty(Orderby))
                {
                    url += string.Format("&$orderby={0}", Orderby);
                }

                if (!string.IsNullOrEmpty(Select))
                {
                    url += string.Format("&$select={0}", Select);
                }

                if (!string.IsNullOrEmpty(Facet))
                {
                    url += string.Format("&facet={0}", Facet);
                }

                if (!string.IsNullOrEmpty(Highlight))
                {
                    url += string.Format("&highlight={0}", Highlight);
                }

                if (!string.IsNullOrEmpty(HighlightPreTag))
                {
                    url += string.Format("&highlightPreTag={0}", HighlightPreTag);
                }

                if (!string.IsNullOrEmpty(HighlightPostTag))
                {
                    url += string.Format("&highlightPostTag={0}", HighlightPostTag);
                }

                if (!string.IsNullOrEmpty(ScoringProfile))
                {
                    url += string.Format("&scoringProfile={0}", ScoringProfile);
                }

                if (!string.IsNullOrEmpty(ScoringParameter))
                {
                    url += string.Format("&scoringParameter={0}", ScoringParameter);
                }

                return url;
            }
        }

        public string SearchQuery
        {
            get { return _searchQuery; }
            set
            {
                _searchQuery = value;
                OnPropertyChanged("Url");
            }
        }


        public Index Index
        {
            get { return _index; }
            set
            {
                _index = value;
                OnPropertyChanged("Index");
                OnPropertyChanged("Url");
            }
        }

        public string ApiVersion
        {
            get { return _apiVersion; }
            set
            {
                _apiVersion = value;
                OnPropertyChanged("ApiVersion");
                OnPropertyChanged("Url");
            }
        }

        private string _top = "";
        public string Top
        {
            get
            {
                return _top;
            }
            set
            {
                _top = value;
                OnPropertyChanged("Top");
                OnPropertyChanged("Url");
            }
        }

        private string _skip = "";
        public string Skip
        {
            get
            {
                return _skip;
            }
            set
            {
                _skip = value;
                OnPropertyChanged("Skip");
                OnPropertyChanged("Url");
            }
        }

        public List<string> AvailableApiVersions
        {
            get
            {
                return new List<string>()
                                        {
                                        "2014-07-31-Preview", "2014-10-20-Preview", "2015-02-28-Preview", "2015-02-28"
                                        };
            }
        }

        private ObservableCollection<Index> _indexes = new ObservableCollection<Index>();
        private string _error;
        private Index _index;
        private string _url;
        private string _searchQuery;
        private string _searchResultRaw;
        private string _status;



        public ObservableCollection<Index> Indexes
        {
            get { return _indexes; }
        }

        private DataTable _searchResults = new DataTable();
        private string _searchMode;
        private string _searchFields;
        private string _count;
        private string _orderby;
        private string _select;
        private string _facet;
        private string _highlight;
        private string _highlightPreTag;
        private string _highlightPostTag;
        private string _scoringProfile;
        private string _scoringParameter;

        public DataTable SearchResults
        {
            get { return _searchResults; }
            set { _searchResults = value; }
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
            set
            {
                _error = value;
                OnPropertyChanged("Error");
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



        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public string SearchResultRaw
        {
            get { return _searchResultRaw; }
            set
            {
                _searchResultRaw = value;
                OnPropertyChanged("SearchResultRaw");
            }
        }

        public string Status
        {
            get { return _status; }
            set
            {
                _status = value;
                OnPropertyChanged("Status");
            }
        }

        public string SearchMode
        {
            get { return _searchMode; }
            set
            {
                _searchMode = value;
                OnPropertyChanged("Skip");
                OnPropertyChanged("Url");
            }
        }

        public List<string> AvailableSearchModes
        {
            get
            {
                return
                    new List<string>
                    {
                        "any", "all"
                    };
            }
        }

        public string SearchFields
        {
            get { return _searchFields; }
            set
            {
                var tmp = value;
                //no spaces allowed here
                tmp = tmp.Replace(" ", "");
                _searchFields = tmp;
                OnPropertyChanged("Skip");
                OnPropertyChanged("SearchFields");
            }
        }

        public List<string> AvailableCountModes
        {
            get { return new List<string> { "true", "false" }; }
        }

        public string Count
        {
            get { return _count; }
            set
            {
                _count = value;
                OnPropertyChanged("Count");
                OnPropertyChanged("Url");
            }
        }

        public string Orderby
        {
            get { return _orderby; }
            set
            {
                _orderby = value;
                OnPropertyChanged("Orderby");
                OnPropertyChanged("Url");
            }
        }

        public string Select
        {
            get
            {
                return _select;

            }
            set
            {
                _select = value;
                OnPropertyChanged("Select");
                OnPropertyChanged("Url");
            }
        }

        public string Facet
        {
            get { return _facet; }
            set
            {
                _facet = value;
                OnPropertyChanged("Facet");
                OnPropertyChanged("Url");
            }
        }

        public string Highlight
        {
            get { return _highlight; }
            set
            {
                _highlight = value;
                OnPropertyChanged("Highlight");
                OnPropertyChanged("Url");
            }
        }

        public string HighlightPreTag
        {
            get { return _highlightPreTag; }
            set
            {
                _highlightPreTag = value;
                OnPropertyChanged("HighlightPreTag");
                OnPropertyChanged("Url");
            }
        }

        public string HighlightPostTag
        {
            get { return _highlightPostTag; }
            set
            {
                _highlightPostTag = value;
                OnPropertyChanged("HighlightPostTag");
                OnPropertyChanged("Url");
            }
        }

        public string ScoringProfile
        {
            get { return _scoringProfile; }
            set
            {
                _scoringProfile = value;
                OnPropertyChanged("ScoringProfile");
                OnPropertyChanged("Url");
            }
        }

        public string ScoringParameter
        {
            get { return _scoringParameter; }
            set
            {
                _scoringParameter = value;
                OnPropertyChanged("ScoringParameter");
                OnPropertyChanged("Url");
            }
        }

        public void Search()
        {
            try
            {
                if (string.IsNullOrEmpty(SearchQuery))
                {
                    Error = "No Search Query provided";
                    Status = "No Search Query provided";
                    return;
                }

                //clear the datatable and reset the columns
                SearchResults.Clear();
                SearchResults.Columns.Clear();

                var client = GetWebClient();
                var watch = new Stopwatch();
                watch.Start();
                SearchResultRaw = client.DownloadString(new Uri(Url));
                watch.Stop();
                /*
             {
                "@odata.context": "https://maxmelcher.search.windows.net/indexes('twittersearch')/$metadata#docs(Text,Mention,Created,Url,StatusId,Sentiment,Score)",
                "@odata.count": 31,
                "value": [
                    {
                        "@search.score": 0.094358146,
                        "Text": "RT @ynfa_thehub: #FIFA http://t.co/oi7ugyAhfz",
                        "Mention": "@ynfa_thehub",
                        "Created": null,
                        "Url": "https://twitter.com/Locket25/status/604595408925507585",
                        "StatusId": "604595408925507585",
                        "Sentiment": "good",
                        "Score": 0.7330736
                    }, ... 
             }
             * */

                dynamic results = JObject.Parse(SearchResultRaw);

                //pretty print it
                SearchResultRaw = JsonConvert.SerializeObject(results, Formatting.Indented);

                if (results.value.Count > 0)
                {
                    //create the columns
                    foreach (var col in results.value.First)
                    {
                        SearchResults.Columns.Add(col.Name);
                    }

                    //Todo: Mabye do more advanced column handling here, I am thinking of geolocation
                    //create the values for the table
                    foreach (var elem in results.value)
                    {
                        var row = SearchResults.Rows.Add();
                        foreach (var col in elem)
                        {

                            if (col.Name == "@search.score")
                            {
                                row[col.Name] = col.Value.Value;
                            }
                            else if (col.Name == "@search.highlights")
                            {
                                row[col.Name] = col.Value.Text;
                            }
                            else
                            {
                                    row[col.Name] = col.Value.Value;
                            }
                        }
                    }

                    Status = string.Format("Search Query executed in {0}ms", watch.ElapsedMilliseconds);
                    Error = "";
                }
                else
                {
                    Status = string.Format("0 results - Search Query executed in {0}ms", watch.ElapsedMilliseconds);
                    Error = "";
                }
            }
            catch (WebException ex) //handle the response errors, this gives nice insight what went wrong
            {
                if (ex.Response != null)
                {
                    var stream = ex.Response.GetResponseStream();
                    if (stream != null)
                    {
                        using (var reader = new StreamReader(stream))
                        {
                            var tmp = reader.ReadToEnd();
                            var json = JObject.Parse(tmp);

                            var error = json["error"];
                            if (error["message"] != null)
                            {
                                Error = string.Format("Error: {0}", error["message"].Value<string>());
                            }
                            if (error["code"] != null)
                            {
                                Error += string.Format("\r\nCode: {0}", error["code"].Value<string>());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Error = ex.Message;
            }
        }
    }


}