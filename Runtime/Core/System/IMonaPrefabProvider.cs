﻿using Mona.SDK.Core.Body;

namespace Mona.Core
{
    public interface IMonaPrefabProvider
    {
        MonaBody GetMonaBodyPrefab(string prefabId);
    }
}