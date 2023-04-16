using MongoDB.Bson.Serialization;

namespace mongo_scratch.Infrastructure;

public class CustomSerializationProvider : IBsonSerializationProvider
{
    public IBsonSerializer? GetSerializer(Type type)
    {
        if (typeof(IEnumerationValue).IsAssignableFrom(type))
        {
            var genericEnumValueSerializerType = typeof(EnumerationValueSerializer<>);
            Type[] listOfTypeArgs = { type };
            return
                Activator.CreateInstance(genericEnumValueSerializerType.MakeGenericType(listOfTypeArgs))
                    as IBsonSerializer;
        }

        // if (typeof(IIdentity).IsAssignableFrom(type) /*&& type != typeof(EmailAddress)*/)
        // {
        //     var genericIdentitySerializerType = typeof(GenericIIdentitySerializer<>);
        //     Type[] listOfTypeArgs = { type };
        //     return 
        //         Activator.CreateInstance(genericIdentitySerializerType.MakeGenericType(listOfTypeArgs))
        //             as IBsonSerializer;
        // }

        return null;
    }
}