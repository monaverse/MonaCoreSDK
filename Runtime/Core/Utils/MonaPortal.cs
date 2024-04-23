using Mona.SDK.Brains.Core.Events;
using Unity.VisualScripting;
using UnityEngine;

namespace Mona.SDK.Core.Utils
{
    public class MonaPortal
    {
        public static void GoTo(string portal)
        {
            Debug.Log($"Goto Portal Requested: {portal}");
            MonaEventBus.Trigger<MonaPortalChangeEvent>(new EventHook(MonaCoreConstants.ON_PORTAL_CHANGE_EVENT), new MonaPortalChangeEvent(portal));
        }
    }
}
