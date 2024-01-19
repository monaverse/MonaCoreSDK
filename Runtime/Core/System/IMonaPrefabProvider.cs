using Mona.Core.Body;

namespace Mona.Core
{
    public interface IMonaPrefabProvider
    {
        MonaBody GetMonaBodyPrefab(string prefabId);
        MonaReactor GetMonaReactorPrefab(string prefabId);
    }
}