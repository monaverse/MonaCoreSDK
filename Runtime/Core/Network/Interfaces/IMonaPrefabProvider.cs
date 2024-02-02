using Mona.SDK.Core.Body;

namespace Mona.SDK.Core.Network.Interfaces
{
    public interface IMonaPrefabProvider
    {
        MonaBody GetMonaBodyPrefab(string prefabId);
    }
}