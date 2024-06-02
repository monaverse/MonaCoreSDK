using System.Collections.Generic;
using UnityEngine;
using System;
using Mona.SDK.Core.Body;
using Mona.SDK.Core.Assets.Interfaces;
using Mona.SDK.Core.Events;
using Unity.VisualScripting;
using Mona.SDK.Core.Network.Interfaces;
using Mona.SDK.Core.Utils;

namespace Mona.SDK.Core.Assets
{
    [Serializable]
    public class MonaAssetsOnSelf : IMonaAssetProvider
    {
        [SerializeField] private string _name = "";
        public string Name { get => _name; set => _name = value; }

        [SerializeReference]
        private List<IMonaAssetItem> _monaAssets = new List<IMonaAssetItem>();

        public List<IMonaAssetItem> AllAssets => _monaAssets;

        private IMonaNetworkSpawner _networkSpawner;

        public List<MonaBody> _monaBodyPrefabs;

        private Action<NetworkSpawnerStartedEvent> OnNetworkSpawnerStartedEvent;

        public List<string> AllNames => _monaAssets.ConvertAll<string>(x => x.PrefabId);

        public virtual List<string> DefaultNames => new List<string>();

        public void Initialize()
        {
            AddDelegates();
        }

        private void AddDelegates()
        {
            OnNetworkSpawnerStartedEvent = HandleNetworkSpawnerStartedEvent;
            MonaEventBus.Register<NetworkSpawnerStartedEvent>(new EventHook(MonaCoreConstants.NETWORK_SPAWNER_STARTED_EVENT), OnNetworkSpawnerStartedEvent);
        }

        private void RemoveDelegates()
        {
            MonaEventBus.Unregister(new EventHook(MonaCoreConstants.NETWORK_SPAWNER_STARTED_EVENT), OnNetworkSpawnerStartedEvent);
        }

        private void Dispose()
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

        public T GetMonaAsset<T>(Func<T, bool> predicate)
        {
            for (var i = 0; i < _monaAssets.Count; i++)
            {
                var monaAsset = (T)_monaAssets[i];
                if (predicate(monaAsset))
                    return monaAsset;
            }
            return default;
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

        private List<IMonaAssetItem> _deck = new List<IMonaAssetItem>();
        public IMonaAssetItem TakeTopCardOffDeck(bool shuffleIfEmpty)
        {
            if(_deck.Count == 0)
            {
                _deck.AddRange(_monaAssets);
                if(shuffleIfEmpty)
                {
                    _deck.Sort((a, b) => UnityEngine.Random.Range(-1, 2));
                    _deck.Sort((a, b) => UnityEngine.Random.Range(-1, 2));
                    _deck.Sort((a, b) => UnityEngine.Random.Range(-1, 2));
                }
            }

            var item = _deck[_deck.Count - 1];
            _deck.RemoveAt(_deck.Count - 1);
            return item;
        }

    }
}