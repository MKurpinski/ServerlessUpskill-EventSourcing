using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Upskill.Cache.Utils
{
    public class ByteSerializer : IByteSerializer
    {
        public byte[] ToByteArray<T>(T obj)
        {
            if (obj == null)
            {
                return null;
            }

            using (var ms = new MemoryStream())
            {
                var bf = new BinaryFormatter();
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        public T FromByteArray<T>(byte[] data)
        {
            if (data == null)
            {
                return default;
            }

            using (var ms = new MemoryStream(data))
            {
                var bf = new BinaryFormatter();
                var obj = bf.Deserialize(ms);
                return (T)obj;
            }
        }
    }
}