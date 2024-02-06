
using Mona.SDK.Core.Network.Interfaces;

namespace Mona.SDK.Core.Events
{
    public sealed class NetworkSpawnerInitializedEvent
    {
        public IMonaNetworkSpawner NetworkSpawner;

        public NetworkSpawnerInitializedEvent(IMonaNetworkSpawner spawner)
        {
            NetworkSpawner = spawner;
        }
    }
}
