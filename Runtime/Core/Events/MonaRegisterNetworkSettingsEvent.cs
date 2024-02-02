using Mona.SDK.Core.Input.Interfaces;
using Mona.SDK.Core.Network.Interfaces;
using System.Collections.Generic;

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