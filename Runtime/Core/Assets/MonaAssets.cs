using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Mona.SDK.Core.Body;
using Mona.SDK.Core.Assets.Interfaces;
using Mona.SDK.Core.Events;
using Unity.VisualScripting;
using Mona.SDK.Core.Network.Interfaces;

namespace Mona.SDK.Core.Assets
{   
    [Serializable]
    public class MonaAssets : ScriptableObject, IMonaAssetProvider
    {
        [SerializeReference] 
        private List<IMonaAssetItem> _monaAssets = new List<IMonaAssetItem>();

        public List<IMonaAssetItem> AllAssets => _monaAssets;

        private IMonaNetworkSpawner _networkSpawner;

        public List<MonaBody> _monaBodyPrefabs;

        private Action<NetworkSpawnerStartedEvent> OnNetworkSpawnerStartedEvent;

        public List<string> AllNames => _monaAssets.ConvertAll<string>(x => x.PrefabId);

        public virtual List<string> DefaultNames => new List<string>();

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

        public IMonaAssetItem CreateAsset(string name, Type type, int i)
        {
            var prop = (IMonaAssetItem)Activator.CreateInstance(type);
            prop.PrefabId = name;
            _monaAssets[i] = prop;
            return prop;
        }

        public IMonaAssetItem GetMonaAsset(string prefabId)
        {
            var asset = _monaAssets.Find((x) => x.PrefabId.Equals(prefabId));
            return asset != null ? asset : null;
        }

        public IMonaAssetItem GetMonaAssetByIndex(int index)
        {
            if (index < _monaAssets.Count && index >= 0)
                return _monaAssets[index];
            return null;
        }

        public List<IMonaBodyAssetItem> GetMonaBodies() => _monaAssets.FindAll(x => x is IMonaBodyAssetItem).ConvertAll<IMonaBodyAssetItem>(x => (IMonaBodyAssetItem)x);
        public List<IMonaAudioAssetItem> GetMonaAudioClips() => _monaAssets.FindAll(x => x is IMonaBodyAssetItem).ConvertAll<IMonaAudioAssetItem>(x => (IMonaAudioAssetItem)x);
        public List<IMonaAnimationAssetItem> GetMonaAnimations() => _monaAssets.FindAll(x => x is IMonaBodyAssetItem).ConvertAll<IMonaAnimationAssetItem>(x => (IMonaAnimationAssetItem)x);

    }

}