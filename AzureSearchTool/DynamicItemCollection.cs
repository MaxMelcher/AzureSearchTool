using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;

namespace AzureSearchTool
{
    public class DynamicItemCollection<T> : ObservableCollection<T>, IList, ITypedList
        where T : DynamicObject
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
                var firstItem = this[0];

                dynamicDescriptors =
                    firstItem.GetDynamicMemberNames()
                        .Select(p => new DynamicPropertyDescriptor(p))
                        .Cast<PropertyDescriptor>()
                        .ToArray();
            }

            return new PropertyDescriptorCollection(dynamicDescriptors);
        }
    }
}