using Mona.SDK.Core.Body;

namespace Mona
{
    public interface IMonaPrefabProvider
    {
        MonaBody GetMonaBodyPrefab(string prefabId);
    }
}