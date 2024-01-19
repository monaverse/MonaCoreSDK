using Mona.SDK.Core;
using Mona.SDK.Core.Body;
using Mona.SDK.Core.Events;
using Mona.SDK.Core.Network;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Mona
{
    public partial class MonaReactor : MonaBodyBase
    {
        private IMonaNetworkSpawner _networkSpawner;
        private bool _registerWhenEnabled;

        public static List<MonaReactor> MonaReactors = new List<MonaReactor>();
        public static MonaReactor FindByLocalId(string localId) => MonaReactors.Find((x) => x.LocalId == localId);

        public Action<NetworkSpawnerStartedEvent> OnNetworkSpawnerStartedEvent;

        private void Awake()
        {
            if (!MonaReactors.Contains(this))
                MonaReactors.Add(this);

            AddDelegates();
            ReactorSetup();
        }

        private void AddDelegates()
        {
            OnNetworkSpawnerStartedEvent = HandleNetworkSpawnerStarted;
            EventBus.Register<NetworkSpawnerStartedEvent>(new EventHook(MonaCoreConstants.MONA_BODIES_START_EVENT), OnNetworkSpawnerStartedEvent);
        }    

        private void RemoveDelegates()
        {
            EventBus.Unregister(new EventHook(MonaCoreConstants.MONA_BODIES_START_EVENT), OnNetworkSpawnerStartedEvent);
        }

        private void OnDestroy()
        {
            StopCoroutine("WaitForNetworkSpawner");

            if (MonaReactors.Contains(this))
                MonaReactors.Remove(this);

            EventBus.Unregister(new EventHook(MonaCoreConstants.MONA_BODIES_START_EVENT), OnNetworkSpawnerStartedEvent);
        }

        private void HandleNetworkSpawnerStarted(NetworkSpawnerStartedEvent evt)
        {
            _registerWhenEnabled = true;
            _networkSpawner = evt.NetworkSpawner;

            if (gameObject.activeInHierarchy)
            {
                RegisterWithNetwork();
            }

            RemoveDelegates();
        }

        private void OnEnable()
        {
            if (_registerWhenEnabled)
            {
                RegisterWithNetwork();
            }
        }

        private void RegisterWithNetwork()
        {
            if (_networkSpawner != null && gameObject != null)
                _networkSpawner.RegisterMonaReactor(this);
        }

        public void ReactorSetup()
        {
            //Debug.Log($"{nameof(ReactorSetup)} {name}");
            // Ensure we have any events, if not we self destruct
            if (OnEnterTrigger == null && OnExitTrigger == null && OnPlayerInteract == null && OnPlayerLookStart == null && OnPlayerLookEnd == null && OnObjectEnable == null && OnObjectDisable == null)
            {
                Destroy(this);
            }

            // Ensure collider based reactors are triggers
            if (OnEnterTrigger.Length > 0 || OnExitTrigger.Length > 0)
            {
                BoxCollider boxCollider = GetComponent<BoxCollider>();

                if (boxCollider)
                {
                    boxCollider.isTrigger = true;
                }
            }
        }

        public void SetNetworkMonaReactor(INetworkMonaReactorClient obj) => _networkReactorClient = obj;

        public void SyncLocalValues()
        {
            //Debug.Log($"{nameof(MonaReactor)}.{nameof(SyncAnimator)}");
            RegisterAllMonaEvents();
            for (var i = 0; i < parameterRegistry.Count; i++)
            {
                var record = parameterRegistry[i];
                switch (record.valueType)
                {
                    case ValueType.Int:
                        _networkReactorClient?.SetAnimationInt(i, record.animator.GetInteger(record.name));
                        break;
                    case ValueType.Float:
                        _networkReactorClient?.SetAnimationFloat(i, record.animator.GetFloat(record.name));
                        break;
                    case ValueType.Boolean:
                        _networkReactorClient?.SetAnimationBool(i, record.animator.GetBool(record.name));
                        break;
                }
                //Debug.Log($"Network Reactor Multiplayer Spawned, initialize property {record.name}");
            }
        }

        private void ExecuteEvent(MonaEvent monaEvent)
        {
            if (monaEvent.Object == null)
            {
                Debug.LogError("Event " + monaEvent.Name + " : Object is null");
                return;
            }

            Animator animator = monaEvent.Object.GetComponent<Animator>();

            if (animator == null)
            {
                Debug.LogError("Event " + monaEvent.Name + " : Object does not have an Animator component");
                return;
            }

            switch (monaEvent.ValueType)
            {
                case ValueType.Int:

                    HandleIntEventType(monaEvent, animator);
                    break;
                case ValueType.Float:

                    HandleFloatEventType(monaEvent, animator);
                    break;
                case ValueType.Boolean:

                    HandleBooleanEventType(monaEvent, animator);
                    break;
                case ValueType.Trigger:

                    HandleTriggerEventType(monaEvent, animator);
                    break;
            }
        }

        private void OnTriggerEnter(Collider eventObject)
        {
            if (OnEnterTrigger == null) return;
            Debug.Log($"{nameof(MonaReactor)}.{nameof(OnTriggerEnter)}");
            HandleMonaEvents(OnEnterTrigger, eventObject);
        }

        private void OnTriggerExit(Collider eventObject)
        {
            if (OnExitTrigger == null) return;
            Debug.Log($"{nameof(MonaReactor)}.{nameof(OnTriggerExit)}");
            HandleMonaEvents(OnExitTrigger, eventObject);
        }

        public void OnInteractEvent()
        {
            if (OnPlayerInteract == null) return;
            Debug.Log($"{nameof(MonaReactor)}.{nameof(OnInteractEvent)}");
            HandleMonaEvents(OnPlayerInteract);
        }

        public void OnLookStartEvent()
        {
            if (OnPlayerLookStart == null) return;
            Debug.Log($"{nameof(MonaReactor)}.{nameof(OnLookStartEvent)}");
            HandleMonaEvents(OnPlayerLookStart);
        }

        public void OnLookEndEvent()
        {
            if (OnPlayerLookEnd == null) return;
            Debug.Log($"{nameof(MonaReactor)}.{nameof(OnLookEndEvent)}");
            HandleMonaEvents(OnPlayerLookEnd);
        }

        private void OnEnableEvent()
        {
            if (OnObjectEnable == null) return;
            Debug.Log($"{nameof(MonaReactor)}.{nameof(OnEnableEvent)}");
            HandleMonaEvents(OnObjectEnable);
        }

        private void OnDisableEvent()
        {
            if (OnObjectDisable == null) return;
            Debug.Log($"{nameof(MonaReactor)}.{nameof(OnDisableEvent)}");
            HandleMonaEvents(OnObjectDisable);
        }
    }
}