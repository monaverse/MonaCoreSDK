using UnityEngine;
using UnityEngine.SceneManagement;

namespace Mona.SDK.Core.Events
{
    public struct MonaChangeSpaceEvent
    {
        public Scene Scene;
        public MonaChangeSpaceEvent(Scene scene)
        {
            Scene = scene;
        }
    }
}