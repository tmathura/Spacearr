﻿namespace Multilarr.WorkerService.Windows.Common.Interfaces
{
    public interface IDataSize
    {
        string SizeSuffix(long value, int decimalPlaces = 1);
    }
}