namespace Upskill.Cache.Utils
{
    public interface IByteSerializer
    {
        T FromByteArray<T>(byte[] data);
        byte[] ToByteArray<T>(T obj);
    }
}
