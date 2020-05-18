﻿using System.Threading.Tasks;

namespace Spacearr.Common.Interfaces
{
    public interface ISetting
    {
        string AppId { get; set; }
        string Key { get; set; }
        string Secret { get; set; }
        string Cluster { get; set; }
        Task PopulateSetting();
    }
}