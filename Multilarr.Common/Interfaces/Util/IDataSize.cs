namespace Multilarr.Common.Interfaces.Util
{
    public interface IDataSize
    {
        string SizeSuffix(long value, int decimalPlaces = 1);
    }
}