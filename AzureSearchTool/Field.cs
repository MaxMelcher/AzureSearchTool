namespace AzureSearchTool
{
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