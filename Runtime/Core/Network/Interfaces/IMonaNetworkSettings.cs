using Mona.SDK.Core.Network.Enums;

namespace Mona.SDK.Core.Network.Interfaces
{
    public interface IMonaNetworkSettings
    {
        MonaNetworkType NetworkType { get; set; }
        MonaNetworkType GetNetworkType();
    }
}