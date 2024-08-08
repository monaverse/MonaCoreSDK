using Mona.SDK.Core.Body;
using Mona.SDK.Core.Body.Enums;
using Mona.SDK.Core.Events;
using System;
using Unity.VisualScripting;
using UnityEngine;
using Mona.SDK.Core.Utils;

namespace Mona.SDK.Core.Mock
{
    public class MockEventBus : MonoBehaviour
    {
        public bool MockPlayer;
        public int MockPlayerId = 0;

        private Action<NetworkSpawnerStartedEvent> OnNetworkSpawnerStarted;

        public void Awake()
        {
            OnNetworkSpawnerStarted = HandleNetworkSpawnerStarted;
            MonaEventBus.Register<NetworkSpawnerStartedEvent>(new EventHook(MonaCoreConstants.NETWORK_SPAWNER_STARTED_EVENT), OnNetworkSpawnerStarted);
        }

        public void OnDestroy()
        {
            MonaEventBus.Unregister(new EventHook(MonaCoreConstants.NETWORK_SPAWNER_STARTED_EVENT), OnNetworkSpawnerStarted);
        }

        private void HandleNetworkSpawnerStarted(NetworkSpawnerStartedEvent evt)
        {
            if (MockPlayer)
            {
                if(MonaBodyFactory.FindByTag("Player").Count == 0)
                {
                    Debug.LogError($"Please add a 'Player' tag to a MonaBody representing your mock player");
                    return;
                }
                TriggerLocalPlayerJoined(MonaBodyFactory.FindByTag("Player")[0], MockPlayerId);
            }
        }

        public void TriggerOlympiaUIVisibilityChanged(bool isVisible)
        {
            MonaEventBus.Trigger<MonaCursorVisibilityEvent>(new EventHook(MonaCoreConstants.ON_CURSOR_VISIBILITY_CHANGED_EVENT), new MonaCursorVisibilityEvent(isVisible));
        }

        public void TriggerLocalPlayerJoined(IMonaBody playerBody, int playerId)
        {
            playerBody.AttachType = MonaBodyAttachType.LocalPlayer;
            MonaEventBus.Trigger<MonaPlayerJoinedEvent>(new EventHook(MonaCoreConstants.ON_PLAYER_JOINED_EVENT), new MonaPlayerJoinedEvent(playerBody, playerId, true));
        }

        public void TriggerPlayerJoined(IMonaBody playerBody, int playerId)
        {
            playerBody.AttachType = MonaBodyAttachType.RemotePlayer;
            MonaEventBus.Trigger<MonaPlayerJoinedEvent>(new EventHook(MonaCoreConstants.ON_PLAYER_JOINED_EVENT), new MonaPlayerJoinedEvent(playerBody, playerId, false));
        }

        public void TriggerPlayerLeft(int playerId)
        {
            MonaEventBus.Trigger<MonaPlayerLeftEvent>(new EventHook(MonaCoreConstants.ON_PLAYER_LEFT_EVENT), new MonaPlayerLeftEvent(playerId));
        }

        public void TriggerPlayerName(string playerName, int playerId)
        {
            MonaEventBus.Trigger<MonaPlayerNameEvent>(new EventHook(MonaCoreConstants.ON_PLAYER_NAME_EVENT), new MonaPlayerNameEvent(playerName, playerId));
        }
    }
}