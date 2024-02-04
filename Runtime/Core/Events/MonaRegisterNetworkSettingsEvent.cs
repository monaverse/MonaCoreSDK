using Mona.SDK.Core.Network.Interfaces;

namespace Mona.SDK.Core.Events
{
    public struct MonaRegisterNetworkSettingsEvent
    {
        public IMonaNetworkSettings Settings;
        public MonaRegisterNetworkSettingsEvent(IMonaNetworkSettings settings)
        {
            Settings = settings;
        }
    }
}