using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace AzureSearchTool
{
    public class DynamicItemCollection<T> : ObservableCollection<T>, IList, ITypedList
        where T : JObject
    {
        public string GetListName(PropertyDescriptor[] listAccessors)
        {
            return null;
        }

        public PropertyDescriptorCollection GetItemProperties(PropertyDescriptor[] listAccessors)
        {
            var dynamicDescriptors = new PropertyDescriptor[0];
            if (this.Any())
            {
                JObject firstItem = this[0];

                dynamicDescriptors =
                    firstItem.Properties()
                        .Select(p => new DynamicPropertyDescriptor(p.Name))
                        .Cast<PropertyDescriptor>()
                        .ToArray();
            }

            return new PropertyDescriptorCollection(dynamicDescriptors);
        }
    }
}