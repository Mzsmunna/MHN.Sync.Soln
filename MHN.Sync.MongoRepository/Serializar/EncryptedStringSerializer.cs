using MHN.Sync.MongoRepository.Helper;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHN.Sync.MongoRepository.Serializar
{
    public class EncryptedStringSerializer : SerializerBase<string>
    {
        public override string Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            if (context.Reader.CurrentBsonType == BsonType.Null)
            {
                context.Reader.ReadNull();
                return "";
            }
            var encryptedValue = context.Reader.ReadString();
            var decryptedValue = Cryptography.Decrypt(encryptedValue);
            return decryptedValue;
        }

        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                context.Writer.WriteString("");
            }
            else
            {
                value = Cryptography.Encrypt(value);
                context.Writer.WriteString(value);
            }
        }
    }
}
