using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Mona.SDK.Core.Body;
using Mona.SDK.Core;
using Unity.VisualScripting;
using Mona.SDK.Core.Events;

namespace Mona.SDK.Core.Network
{
    public partial class MonaPrefabs : MonoBehaviour, IMonaPrefabProvider
    {
        private IMonaNetworkSpawner _networkSpawner;

        public List<MonaBody> _monaBodyPrefabs;

        private Action<NetworkSpawnerStartedEvent> OnNetworkSpawnerStartedEvent;

        public void Awake()
        {
            AddDelegates();
        }

        private void AddDelegates()
        {
            OnNetworkSpawnerStartedEvent = HandleNetworkSpawnerStartedEvent;
            EventBus.Register<NetworkSpawnerStartedEvent>(new EventHook(MonaCoreConstants.NETWORK_SPAWNER_STARTED_EVENT), OnNetworkSpawnerStartedEvent);
        }

        private void RemoveDelegates()
        {
            EventBus.Unregister(new EventHook(MonaCoreConstants.NETWORK_SPAWNER_STARTED_EVENT), OnNetworkSpawnerStartedEvent);
        }

        private void OnDestroy()
        {
            RemoveDelegates();
        }

        private void HandleNetworkSpawnerStartedEvent(NetworkSpawnerStartedEvent evt)
        {
            _networkSpawner = evt.NetworkSpawner;
            _networkSpawner.RegisterMonaPrefabProvider(this);
        }

        public MonaBody GetMonaBodyPrefab(string prefabId)
        {
            var prefab = _monaBodyPrefabs.Find((x) => x.PrefabId.Equals(prefabId));
            return prefab != null ? prefab : null;
        }
    }
}