
using Mona.SDK.Core.Network;

namespace Mona.SDK.Core.Events
{
    public sealed class NetworkSpawnerStartedEvent
    {
        public IMonaNetworkSpawner NetworkSpawner;

        public NetworkSpawnerStartedEvent(IMonaNetworkSpawner spawner)
        {
            NetworkSpawner = spawner;
        }
    }
}
