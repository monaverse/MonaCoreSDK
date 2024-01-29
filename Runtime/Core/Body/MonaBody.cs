using UnityEngine;
using Unity.VisualScripting;
using System.Collections.Generic;
using Mona.SDK.Core.Network;
using Mona.SDK.Core.Events;
using System;
using System.Collections;

namespace Mona.SDK.Core.Body
{
    public partial class MonaBody : MonaBodyBase, IMonaBody, IMonaTagged
    {
        public event Action OnStarted = delegate { };
        public event Action OnResumed = delegate { };
        public event Action OnPaused = delegate { };

        private bool _registerWhenEnabled;
        private IMonaNetworkSpawner _networkSpawner;
        private INetworkMonaBodyClient _networkBody;
        private Rigidbody _rigidbody;
        private CharacterController _characterController;
        private Camera _camera;
        private bool _visible = true;

        public bool IsNetworked => _networkBody != null;
        public Transform ActiveTransform => _networkBody != null ? _networkBody.NetworkTransform : transform;
        public Rigidbody ActiveRigidbody => _networkBody != null ? _networkBody.NetworkRigidbody : _rigidbody;
        public Transform Transform => transform;
        public float DeltaTime => _networkBody != null ? _networkBody.DeltaTime : Time.deltaTime;
        public Camera Camera => _camera;
        public INetworkMonaBodyClient NetworkBody => _networkBody;

        private bool _updateEnabled;

        public bool UpdateEnabled => _updateEnabled;

        public MonaBodyNetworkSyncType SyncType;

        public bool LocalOnly => SyncType == MonaBodyNetworkSyncType.NotNetworked;

        [SerializeField]
        protected List<string> _monaTags = new List<string>();

        public List<string> MonaTags => _monaTags;

        public static List<IMonaBody> MonaBodies = new List<IMonaBody>();
        private List<IMonaBody> _childMonaBodies = new List<IMonaBody>();

        private List<IMonaTagged> _monaTagged = new List<IMonaTagged>();

        public bool HasMonaTag(string tag) => MonaTags.Contains(tag) || _monaTagged.Find(x => x.HasMonaTag(tag)) != null;
        public static List<IMonaBody> FindByTag(string tag) => MonaBodies.FindAll((x) => x.HasMonaTag(tag));
        public static IMonaBody FindByLocalId(string localId) => MonaBodies.Find((x) => x.LocalId == localId);
        public IMonaBody FindChildByTag(string tag) => _childMonaBodies.Find((x) => x.HasMonaTag(tag));
        public Transform FindChildTransformByTag(string tag) => _childMonaBodies.Find((x) => x.HasMonaTag(tag))?.ActiveTransform;
        public List<IMonaBody> FindChildrenByTag(string tag) => _childMonaBodies.FindAll((x) => x.HasMonaTag(tag));
        public List<IMonaBody> Children() => _childMonaBodies;

        public Action<NetworkSpawnerStartedEvent> OnNetworkSpawnerStartedEvent;

        private bool _mockNetwork;

        private Renderer[] _renderers;

        private void Awake()
        {
            RegisterInParents();
            CacheComponents();
            InitializeTags();
            AddDelegates();
            RegisterInParents();
        }

        private void CacheComponents()
        {
            _renderers = GetComponentsInChildren<Renderer>(true);
            _rigidbody = GetComponent<Rigidbody>();
            _characterController = GetComponent<CharacterController>();
            _camera = GetComponentInChildren<Camera>();
        }
        
        private void InitializeTags()
        {
            _monaTagged = new List<IMonaTagged>(transform.GetComponents<IMonaTagged>());
            _monaTagged.Remove(this);
        }

        private void AddDelegates()
        {
            OnNetworkSpawnerStartedEvent = HandleNetworkSpawnerStarted;
            EventBus.Register(new EventHook(MonaCoreConstants.NETWORK_SPAWNER_STARTED_EVENT), OnNetworkSpawnerStartedEvent);
        }

        private void RemoveDelegates()
        {
            EventBus.Unregister(new EventHook(MonaCoreConstants.NETWORK_SPAWNER_STARTED_EVENT), OnNetworkSpawnerStartedEvent);
        }

        private void HandleNetworkSpawnerStarted(NetworkSpawnerStartedEvent evt)
        {
            if (SyncType != MonaBodyNetworkSyncType.NotNetworked)
            {
                _registerWhenEnabled = true;
                _networkSpawner = evt.NetworkSpawner;
                if (_networkSpawner == null)
                    _mockNetwork = true;
                if (gameObject.activeInHierarchy)
                    RegisterWithNetwork();
            }

            if (SyncType == MonaBodyNetworkSyncType.NotNetworked || _mockNetwork)
            {
                FireSpawnEvent();
                OnStarted();
            }

            RemoveDelegates();
        }

        private void OnEnable()
        {
            if (_registerWhenEnabled)
            {
                RegisterWithNetwork();
                _registerWhenEnabled = false;
            }
            Resume();
        }

        private void OnDisable()
        {
            Pause();
        }

        public void Pause()
        {
            OnPaused();
        }

        public void Resume()
        {
            OnResumed();
        }

        private void RegisterWithNetwork()
        {
            if (_networkSpawner != null && gameObject != null)
                _networkSpawner.RegisterMonaBody(this);
        }

        private void OnDestroy()
        {
            FireDespawnEvent();
            UnregisterInParents();
            RemoveDelegates();
        }

        private void RegisterInParents()
        {
            MonaBodies.Add(this);
            var parents = GetComponentsInParent<MonaBody>();
            for (var i = 0; i < parents.Length; i++)
                parents[i].RegisterAsChild(this);
        }

        private void UnregisterInParents()
        {
            MonaBodies.Remove(this);
            var parents = GetComponentsInParent<MonaBody>();
            for (var i = 0; i < parents.Length; i++)
                parents[i].UnregisterAsChild(this);
        }

        public void RegisterAsChild(MonaBody body)
        {
            if (!_childMonaBodies.Contains(body))
                _childMonaBodies.Add(body);
        }

        public void UnregisterAsChild(MonaBody body)
        {
            if (!_childMonaBodies.Contains(body))
                _childMonaBodies.Add(body);
        }

        public void SetNetworkMonaBody(INetworkMonaBodyClient obj)
        {
            _networkBody = obj;
            FireSpawnEvent();
            OnStarted();
        }

        public void SetUpdateEnabled(bool enabled) => _updateEnabled = enabled;

        public void FixedUpdateNetwork()
        {
            if (HasControl())
                FireUpdateEvent();
        }

        public void StateAuthorityChanged() => FireStateAuthorityChanged();

        public void FixedUpdate()
        {
            if (_networkBody == null)
                FireUpdateEvent();
        }

        private void FireSpawnEvent()
        {
            EventBus.Trigger<MonaBodySpawnedEvent>(new EventHook(MonaCoreConstants.MONA_BODY_SPAWNED), new MonaBodySpawnedEvent((IMonaBody)this));
        }

        private void FireDespawnEvent()
        {
            EventBus.Trigger<MonaBodyDespawnedEvent>(new EventHook(MonaCoreConstants.MONA_BODY_SPAWNED), new MonaBodyDespawnedEvent((IMonaBody)this));
        }

        private void FireUpdateEvent()
        {
            EventBus.Trigger<MonaBodyFixedTickEvent>(new EventHook(MonaCoreConstants.FIXED_TICK_EVENT, (IMonaBody)this), new MonaBodyFixedTickEvent());
        }

        private void FireStateAuthorityChanged()
        {
            EventBus.Trigger<MonaStateAuthorityChangedEvent>(new EventHook(MonaCoreConstants.STATE_AUTHORITY_CHANGED_EVENT, (IMonaBody)this), new MonaStateAuthorityChangedEvent(HasControl()));
        }

        public void SetParent(Transform parent)
        {
            UnregisterInParents();
            ActiveTransform.SetParent(parent, true);
            RegisterInParents();
        }

        public void SetActive(bool active, bool isNetworked = true)
        {
            ActiveTransform.gameObject.SetActive(active);
            if (isNetworked) _networkBody?.SetActive(active);
        }

        public bool HasControl()
        {
            if (SyncType == MonaBodyNetworkSyncType.NotNetworked || _mockNetwork) return true;
            if (_networkBody == null) return false;
            return _networkBody.HasControl();
        }

        public void TakeControl()
        {
            Debug.Log($"{nameof(MonaBody)}.{nameof(TakeControl)} {this}");
            _networkBody?.TakeControl();
        }

        public void ReleaseControl()
        {
            _networkBody?.ReleaseControl();
        }

        public void SetLayer(string layerName, bool includeChildren, bool isNetworked = true)
        {
            var layer = LayerMask.NameToLayer(layerName);
            if (gameObject.layer != layer)
            {
                gameObject.layer = layer;
                if (includeChildren)
                {
                    var children = gameObject.GetComponentsInChildren<Transform>();
                    for (var i = 0; i < children.Length; i++)
                        children[i].gameObject.layer = layer;
                }
                if (isNetworked) _networkBody?.SetLayer(layerName, includeChildren);
            }
        }

        public void SetKinematic(bool isKinematic, bool isNetworked = true)
        {
            if (SyncType == MonaBodyNetworkSyncType.NetworkRigidbody && ActiveRigidbody.isKinematic != isKinematic)
            {
                ActiveRigidbody.isKinematic = isKinematic;
                if (isNetworked) _networkBody?.SetKinematic(isKinematic);
            }
        }

        private Color _color = Color.white;
        public void SetColor(Color color, bool isNetworked = true)
        {
            if (_color != color)
            {
                _color = color;
                if (_renderers != null && _renderers.Length > 0)
                {
                    for (var i = 0; i < _renderers.Length; i++)
                        _renderers[i].material.color = _color;
                }
                if (isNetworked) _networkBody?.SetColor(_color);
            }
        }

        public Color GetColor()
        {
            if (_renderers != null && _renderers.Length > 0)
            {
                if(_color == null)
                    _color = _renderers[0].material.color;
            }
            return _color;
        }


        public void SetVisible(bool vis, bool isNetworked = true)
        {
            if (_visible != vis)
            {
                _visible = vis;
                if (_renderers != null && _renderers.Length > 0)
                {
                    for (var i = 0; i < _renderers.Length; i++)
                        _renderers[i].enabled = _visible;
                }
                if (isNetworked) _networkBody?.SetVisible(vis);
            }
        }

        public bool GetVisible()
        {
            return _visible;
        }


        public void MoveDirection(Vector3 direction, bool isKinematic = false, bool isNetworked = true)
        {
            if (SyncType == MonaBodyNetworkSyncType.NetworkRigidbody)
            {
                SetPosition(ActiveRigidbody.position + direction, isKinematic, isNetworked);
            }
            else
            {
                SetPosition(ActiveTransform.position + direction, isKinematic, isNetworked);
            }
        }

        public void SetPosition(Vector3 position, bool isKinematic = false, bool isNetworked = true)
        {
            if (SyncType == MonaBodyNetworkSyncType.NetworkRigidbody)
            {
                ActiveRigidbody.isKinematic = isKinematic;
                if (isKinematic)
                    ActiveRigidbody.position = position;
                else
                    ActiveRigidbody.MovePosition(position);
            }
            else
            {
                ActiveTransform.position = position;
            }
            if (isNetworked) _networkBody?.SetPosition(position, isKinematic);
        }

        public Vector3 GetPosition()
        {
            if (SyncType == MonaBodyNetworkSyncType.NetworkRigidbody)
                return ActiveRigidbody.position;
            else
                return ActiveTransform.position;
        }

        public Quaternion GetRotation()
        {
            if (SyncType == MonaBodyNetworkSyncType.NetworkRigidbody)
                return ActiveRigidbody.rotation;
            else
                return ActiveTransform.rotation;
        }

        public void RotateTowards(Vector3 direction, float angle, bool isKinematic = false, bool isNetworked = true)
        {
            if (SyncType == MonaBodyNetworkSyncType.NetworkRigidbody)
            {
                SetRotation(Quaternion.Slerp(ActiveRigidbody.rotation, Quaternion.LookRotation(direction, Vector3.up), angle / 360f), isKinematic, isNetworked);
            }
            else
            {
                SetRotation(Quaternion.Slerp(ActiveTransform.rotation, Quaternion.LookRotation(direction, Vector3.up), angle / 360f), isKinematic, isNetworked);
            }
        }

        public void RotateAround(Vector3 direction, float angle, bool isKinematic = false, bool isNetworked = true)
        {
            if (SyncType == MonaBodyNetworkSyncType.NetworkRigidbody)
            {
                SetRotation(ActiveRigidbody.rotation * Quaternion.AngleAxis(angle, direction), isKinematic, isNetworked);
            }
            else
            {
                SetRotation(ActiveTransform.rotation * Quaternion.AngleAxis(angle, direction), isKinematic, isNetworked);
            }
        }

        public void SetRotation(Quaternion rotation, bool isKinematic = false, bool isNetworked = true)
        {
            if (SyncType == MonaBodyNetworkSyncType.NetworkRigidbody)
            {
                ActiveRigidbody.isKinematic = isKinematic;
                if (isKinematic)
                    ActiveRigidbody.rotation = rotation;
                else
                    ActiveRigidbody.MoveRotation(rotation);

            }
            else
                ActiveTransform.rotation = rotation;
            if (isNetworked) _networkBody?.SetRotation(rotation, isKinematic);
        }

        public void SetScale(Vector3 scale, bool isNetworked = true)
        {
            if (!ActiveTransform.localScale.Equals(scale))
            {
                ActiveTransform.localScale = scale;
                if (isNetworked) _networkBody?.SetScale(scale);
            }
        }

    }
}
