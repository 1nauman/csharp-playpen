using System.Reflection;
using Marten;
using Marten.Linq.Fields;

namespace marten_playpen;

public class CustomStringFieldSource : IFieldSource
{
    public bool TryResolve(string dataLocator, StoreOptions options, ISerializer serializer, Type documentType,
        MemberInfo[] members, out IField field)
    {
        field = new SimpleDataField(documentType);

        if (!members.Any())
        {
            return false;
        }


        var directMember = members.First();

        if (!(directMember is PropertyInfo propertyInfo &&
              propertyInfo.PropertyType.IsAssignableTo(typeof(IEnumValue))))
        {
            //If property is not IEnumeration Value.
            return false;
        }

        field = new CustomStringField(dataLocator, Casing.Default, members);
        return true;
    }
}