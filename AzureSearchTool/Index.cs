using System.Collections.Generic;

namespace AzureSearchTool
{
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
}