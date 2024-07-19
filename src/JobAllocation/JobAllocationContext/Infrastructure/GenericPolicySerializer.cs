using JobAllocation.JobAllocationContext.Domain.Aggregates.Policies;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace JobAllocation.JobAllocationContext.Infrastructure;

public class GenericPoliticaSerializer : SerializerBase<IPolicy>, IBsonDocumentSerializer
{
    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, IPolicy value)
    {
        var type = value.GetType();
        context.Writer.WriteStartDocument();
        context.Writer.WriteName("_t");
        context.Writer.WriteString(type.AssemblyQualifiedName); // Stores the complete qualified name for desserialization
        context.Writer.WriteName("Data");
        BsonSerializer.Serialize(context.Writer, type, value); // Serializes the object itself
        context.Writer.WriteEndDocument();
    }

    public override IPolicy Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        context.Reader.ReadStartDocument();
        string typeName = null!;
        IPolicy instance = null!;
        bool typeFound = false, dataFound = false;

        while (context.Reader.ReadBsonType() != BsonType.EndOfDocument)
        {
            var fieldName = context.Reader.ReadName();
            if (fieldName == "_t" && context.Reader.GetCurrentBsonType() == BsonType.String)
            {
                typeName = context.Reader.ReadString();
                typeFound = true;
            }
            else if (fieldName == "Data" && typeFound)
            {
                var type = Type.GetType(typeName);
                if (type == null)
                    throw new InvalidOperationException("Could not find type: " + typeName);

                var serializer = BsonSerializer.LookupSerializer(type);
                instance = (IPolicy)serializer.Deserialize(context);
                dataFound = true;
            }
            else
                context.Reader.SkipValue();
        }

        context.Reader.ReadEndDocument();

        if (typeFound && dataFound && instance != null)
            return instance;
        throw new InvalidOperationException("Necessary data ('_t' or 'Data') not found in document or could not be deserialized.");

    }

    public bool TryGetMemberSerializationInfo(string memberName, out BsonSerializationInfo serializationInfo)
    {
        var serializer = BsonSerializer.LookupSerializer(typeof(IPolicy));
        serializationInfo = new BsonSerializationInfo(memberName, serializer, typeof(IPolicy));
        return true;
    }
}

