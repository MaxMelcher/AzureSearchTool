using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace AzureSearchTool
{
    public class Index
    {
        public Index()
        {
            Statistics = new Statistic();
            Suggesters = new List<Suggester>();
        }

        /// <summary>
        /// if the index was created with an admin key, all fields are available
        /// //todo verify if there is an option to enumerate all fields even with a query key
        /// </summary>
        public bool IsResolved = false;

        public string Name { get; set; }

        public List<Field> Fields { get; set; }

        public Statistic Statistics { get; set; }

        //todo implement scoring profile
        //public List<string> ScoringProfiles { get; set; }

        //todo implement DefaultScoringProfile
        //public string DefaultScoringProfile { get; set; }
        
        //todo implement CorsOptions
        //public string CorsOptions { get; set; }

        //todo implement suggesters
        public List<Suggester> Suggesters { get; set; }
    }

    public class Suggester
    {
        public String Name { get; set; }

        public String SourceFieldsAsText
        {
            get { return string.Join("; ", SourceFields.ToArray()); }
        }

        public List<String> SourceFields { get; set; }
        public String SearchMode { get; set; }
    }

    public class Statistic : INotifyPropertyChanged
    {
        private long _documentCount;
        private long _storageSize;

        public long DocumentCount
        {
            get { return _documentCount; }
            set
            {
                _documentCount = value;
                OnPropertyChanged("DocumentCount");
            }
        }

        public long StorageSize
        {
            get { return _storageSize; }
            set { _storageSize = value; OnPropertyChanged("StorageSize"); OnPropertyChanged("StorageSizeReadable"); }
        }

        public string StorageSizeReadable
        {
            get
            {
                return SizeSuffix(StorageSize);
            }
        }
          

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        static readonly string[] SizeSuffixes = { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };

        static string SizeSuffix(long value)
        {
            if (value < 0) { return "-" + SizeSuffix(-value); }

            int i = 0;
            decimal dValue = value;
            while (Math.Round(dValue / 1024) >= 1)
            {
                dValue /= 1024;
                i++;
            }

            return string.Format("{0:n1} {1}", dValue, SizeSuffixes[i]);
        }
    }
}