using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace AzureSearchTool
{
    public class DynamicItem : DynamicObject
    {
        private readonly Dictionary<string, object> dynamicProperties;

        public DynamicItem(IEnumerable<string> propertyNames)
        {
            dynamicProperties = propertyNames.ToDictionary(s => s, s => (object)null);
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            if (dynamicProperties.ContainsKey(binder.Name))
            {
                dynamicProperties[binder.Name] = value;
                return true;
            }

            return base.TrySetMember(binder, value);
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (dynamicProperties.ContainsKey(binder.Name))
            {
                result = dynamicProperties[binder.Name];
                return true;
            }

            return base.TryGetMember(binder, out result);
        }

        public override IEnumerable<string> GetDynamicMemberNames()
        {
            return dynamicProperties.Keys.ToArray();
        }
    }
}