using Mona.SDK.Core.Body;
using Mona.SDK.Core.Events;
using Unity.VisualScripting;
using UnityEngine;

namespace Mona.SDK.Core.Mock
{
    public class MockEventBus : MonoBehaviour
    {
        public void TriggerOlympiaUIVisibilityChanged(bool isVisible)
        {
            EventBus.Trigger<MonaCursorVisibilityEvent>(new EventHook(MonaCoreConstants.ON_CURSOR_VISIBILITY_CHANGED_EVENT), new MonaCursorVisibilityEvent(isVisible));
        }

        public void TriggerLocalPlayerJoined(IMonaBody playerBody, int playerId)
        {
            EventBus.Trigger<MonaPlayerJoinedEvent>(new EventHook(MonaCoreConstants.ON_PLAYER_JOINED_EVENT), new MonaPlayerJoinedEvent(playerBody, playerId, true));
        }

        public void TriggerPlayerJoined(IMonaBody playerBody, int playerId)
        {
            EventBus.Trigger<MonaPlayerJoinedEvent>(new EventHook(MonaCoreConstants.ON_PLAYER_JOINED_EVENT), new MonaPlayerJoinedEvent(playerBody, playerId, false));
        }

        public void TriggerPlayerLeft(int playerId)
        {
            EventBus.Trigger<MonaPlayerLeftEvent>(new EventHook(MonaCoreConstants.ON_PLAYER_LEFT_EVENT), new MonaPlayerLeftEvent(playerId));
        }

        public void TriggerPlayerName(string playerName, int playerId)
        {
            EventBus.Trigger<MonaPlayerNameEvent>(new EventHook(MonaCoreConstants.ON_PLAYER_NAME_EVENT), new MonaPlayerNameEvent(playerName, playerId));
        }
    }
}