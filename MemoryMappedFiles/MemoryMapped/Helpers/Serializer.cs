using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace MemoryMapped.Helpers
{
    static public class Serializer
    {
        //static private readonly JsonSerializerSettings _settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, TypeNameHandling = TypeNameHandling.Auto };
        static public byte[] Serialize<T>(T content)
        {
            using (var memoryStream = new MemoryStream())
            {
                var binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(memoryStream, content);
                return memoryStream.ToArray();
            }
        }

        static public bool Deserialize(byte[] bytes)
        {
            using (var memoryStream = new MemoryStream(bytes))
            {
                var binaryFormatter = new BinaryFormatter();
                try
                {
                    return Convert.ToBoolean(binaryFormatter.Deserialize(memoryStream));
                }
                catch(Exception ex)
                {
                    return false;
                }
            }
        }
    }
}
