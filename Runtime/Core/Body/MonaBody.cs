﻿using UnityEngine;
using Unity.VisualScripting;
using System.Collections.Generic;
using Mona.SDK.Core.Network;
using Mona.SDK.Core.Events;
using System;
using System.Collections;
using Mona.SDK.Core.Body.Enums;
using Mona.SDK.Core.Network.Interfaces;
using Mona.SDK.Core.Input;
using Mona.SDK.Core.Utils;

namespace Mona.SDK.Core.Body
{
    public class MonaBody : MonaBodyBase, IMonaBody, IMonaTagged
    {
        public event Action OnStarted = delegate { };
        public event Action OnDisableOnLoad = delegate { };
        public event Action<IMonaBody> OnDisabled = delegate { };
        public event Action OnResumed = delegate { };
        public event Action OnPaused = delegate { };
        public event Action OnControlRequested = delegate { };

        private string _skinId;
        public string SkinId { get => _skinId; set => _skinId = value; }

        private GameObject _skin;
        public GameObject Skin { get => _skin; set => _skin = value; }

        private bool _registerWhenEnabled;
        private bool _startWhenEnabled;
        private IMonaNetworkSpawner _networkSpawner;
        private INetworkMonaBodyClient _networkBody;
        private Rigidbody _rigidbody;
        private CharacterController _characterController;
        private Animator _animator;
        private Camera _camera;
        private bool _visible = true;
        private DragType _dragType = DragType.Linear;
        private List<Collider> _colliders;
        private List<Collider> _triggers;
        private float _drag;
        private float _angularDrag;
        private float _dragDivisor = 1;
        private float _angularDragDivisor = 1;
        private float _bounce;
        private float _friction;
        private bool _onlyApplyDragWhenGrounded = true;
        private bool _applyPinOnGrounded = false;
        private IMonaBody _parent;
        private IMonaBody _spawner;
        private IMonaBody _poolBodyPrevious { get; set; }
        private IMonaBody _poolBodyNext { get; set; }
        private MonaBodyAttachType _attachType = MonaBodyAttachType.None;
        private bool _hasRigidbodyInParent;

        private Vector3 _initialPosition = Vector3.zero;
        private Vector3 _initialLocalPosition = Vector3.zero;
        private Quaternion _initialRotation = Quaternion.identity;
        private Quaternion _initialLocalRotation = Quaternion.identity;
        private Vector3 _initialScale = Vector3.one;

        private MonaBodyTransformBounds _positionBounds = new MonaBodyTransformBounds();
        private MonaBodyTransformBounds _rotationBounds = new MonaBodyTransformBounds();

        public bool IsAttachedToRemotePlayer() => _attachType == MonaBodyAttachType.RemotePlayer;
        public bool IsAttachedToLocalPlayer() => _attachType == MonaBodyAttachType.LocalPlayer;

        public IMonaBody Parent => _parent;
        public IMonaBody Spawner { get => _spawner; set => _spawner = value; }
        public IMonaBody PoolBodyPrevious { get => _poolBodyPrevious; set => _poolBodyPrevious = value; }
        public IMonaBody PoolBodyNext { get => _poolBodyNext; set => _poolBodyNext = value; }

        public bool IsNetworked => _networkBody != null;

        private Transform _activeTransform;
        public Transform ActiveTransform => _activeTransform;

        private bool _hasRigidbody;
        private Rigidbody _activeRigidbody;
        public Rigidbody ActiveRigidbody => _activeRigidbody;

        public Transform Transform => (_destroyed) ? null : transform;
        public float DeltaTime => _networkBody != null ? _networkBody.DeltaTime : Time.deltaTime;
        public Camera Camera => _camera;
        public INetworkMonaBodyClient NetworkBody => _networkBody;
        public Animator Animator => _animator;
        public MonaBodyAttachType AttachType { get => _attachType; set => _attachType = value; }
        public List<Collider> Colliders => _colliders;

        public Vector3 InitialPosition => _initialPosition;
        public Vector3 InitialLocalPosition => _initialLocalPosition;
        public Quaternion InitialRotation => _initialRotation;
        public Quaternion InitialLocalRotation => _initialLocalRotation;
        public Vector3 InitialScale => _initialScale;
        public Vector3 CurrentVelocity => _hasRigidbody && !ActiveRigidbody.isKinematic ? ActiveRigidbody.velocity : _transformVelocity;

        public MonaBodyTransformBounds PositionBounds { get => _positionBounds; set => _positionBounds = value; }
        public MonaBodyTransformBounds RotationBounds { get => _rotationBounds; set => _rotationBounds = value; }

        private bool _grounded;
        public bool Grounded => _grounded;

        private int _childIndex = 0;
        public int ChildIndex { get => _childIndex; set => _childIndex = value; }

        private bool _setActive = false;
        private bool _setActiveIsNetworked = true;

        private Vector3 _lastPosition;
        private Vector3 _transformVelocity;

        public struct MonaBodyForce
        {
            public Vector3 Force;
            public ForceMode Mode;
        }

        public struct MonaBodyDirection
        {
            public Vector3 Direction;
        }

        public struct MonaBodyRotation
        {
            public Quaternion Rotation;
        }


        private List<MonaBodyForce> _force = new List<MonaBodyForce>();
        private List<MonaBodyDirection> _positionDeltas = new List<MonaBodyDirection>();
        private List<MonaBodyRotation> _rotationDeltas = new List<MonaBodyRotation>();

        private bool _updateEnabled;

        public bool UpdateEnabled => _updateEnabled;

        public MonaBodyNetworkSyncType SyncType = MonaBodyNetworkSyncType.NetworkTransform;
        public bool SyncPositionAndRotation = true;
        public bool DisableOnLoad = false;

        public bool LocalOnly => SyncType == MonaBodyNetworkSyncType.NotNetworked;

        [SerializeField]
        protected List<string> _monaTags = new List<string>();

        public List<string> MonaTags => _monaTags;

        public static List<IMonaBody> MonaBodies = new List<IMonaBody>();
        public static Dictionary<string, List<IMonaBody>> MonaBodiesByTag = new Dictionary<string, List<IMonaBody>>();

        private List<IMonaBody> _childMonaBodies = new List<IMonaBody>();
        private Dictionary<string, List<IMonaBody>> _childMonaBodiesByTag = new Dictionary<string, List<IMonaBody>>();

        private bool _hasInput;
        private List<MonaInput> _monaInputs = new List<MonaInput>();

        private List<IMonaTagged> _monaTagged = new List<IMonaTagged>();

        public bool HasMonaTag(string tag)
        {
            if (MonaTags.Contains(tag)) return true;
            for(var i = 0;i < _monaTagged.Count; i++)
            {
                if (_monaTagged[i].HasMonaTag(tag))
                    return true;
            }
            return false;
        }

        public void AddTag(string tag)
        {
            if (!HasMonaTag(tag))
            {
                MonaTags.Add(tag);
                RegisterInTagRegistry(tag);
            }
        }

        private void RegisterInTagRegistry(string tag)
        {
            if (!MonaBodiesByTag.ContainsKey(tag))
                MonaBodiesByTag.Add(tag, new List<IMonaBody>());
            if (!MonaBodiesByTag[tag].Contains(this))
                MonaBodiesByTag[tag].Add(this);
        }

        private void RegisterInChildTagRegistry(string tag, IMonaBody body)
        {
            if (!_childMonaBodiesByTag.ContainsKey(tag))
                _childMonaBodiesByTag.Add(tag, new List<IMonaBody>());
            if (!_childMonaBodiesByTag[tag].Contains(body))
                _childMonaBodiesByTag[tag].Add(body);
        }

        public void RemoveTag(string tag)
        {
            if (HasMonaTag(tag))
            {
                MonaTags.Remove(tag);
                UnregisterInTagRegistry(tag);
            }
        }

        private void UnregisterInTagRegistry(string tag)
        {
            if (!MonaBodiesByTag.ContainsKey(tag))
                MonaBodiesByTag.Add(tag, new List<IMonaBody>());
            if (MonaBodiesByTag[tag].Contains(this))
                MonaBodiesByTag[tag].Remove(this);
        }

        private void UnregisterInChildTagRegistry(string tag, IMonaBody body)
        {
            if (!_childMonaBodiesByTag.ContainsKey(tag))
                _childMonaBodiesByTag.Add(tag, new List<IMonaBody>());
            if (_childMonaBodiesByTag[tag].Contains(body))
                _childMonaBodiesByTag[tag].Remove(body);
        }

        public static List<IMonaBody> FindByTag(string tag)
        {
            if(MonaBodiesByTag.ContainsKey(tag))
                return MonaBodiesByTag[tag];
            return _empty;
        }

        public static IMonaBody FindByLocalId(string localId)
        {
            for (var i = 0; i < MonaBodies.Count; i++)
            {
                if (MonaBodies[i].LocalId == localId)
                    return MonaBodies[i];
            }
            return null;
        }

        public IMonaBody FindChildByTag(string tag)
        {
            if (_childMonaBodiesByTag.ContainsKey(tag) && _childMonaBodiesByTag.Count > 0)
                return _childMonaBodiesByTag[tag][0];
            return null;
        }

        public Transform FindChildTransformByTag(string tag)
        {
            if (_childMonaBodiesByTag.ContainsKey(tag) && _childMonaBodiesByTag.Count > 0)
                return _childMonaBodiesByTag[tag][0]?.ActiveTransform;
            return null;
        }

        private static List<IMonaBody> _empty = new List<IMonaBody>();
        public List<IMonaBody> FindChildrenByTag(string tag)
        {
            if (_childMonaBodiesByTag.ContainsKey(tag))
                return _childMonaBodiesByTag[tag];
            return _empty;
        }

        public List<IMonaBody> Children() => _childMonaBodies;

        public Transform GetTransformParent() => ActiveTransform.parent;

        public void SetDragType(DragType dragType) => _dragType = dragType;
        public void SetOnlyApplyDragWhenGrounded(bool apply) => _onlyApplyDragWhenGrounded = apply;
        public void SetApplyPinOnGrounded(bool apply) => _applyPinOnGrounded = apply;

        public void SetDrag(float drag)
        {
            _drag = drag;
            if (_hasRigidbody)
                ActiveRigidbody.drag = drag;
        }

        public void SetAngularDrag(float drag)
        {
            _angularDrag = drag;
            if (_hasRigidbody)
                ActiveRigidbody.angularDrag = drag;
        }

        public void SetUseGravity(bool useGravity)
        {
            if (_hasRigidbody)
                ActiveRigidbody.useGravity = useGravity;
        }

        public void SetVelocity(Vector3 velocity)
        {
            if (_hasRigidbody && !ActiveRigidbody.isKinematic)
                ActiveRigidbody.velocity = velocity;
        }

        public Vector3 GetVelocity()
        {
            //if (_hasRigidbody)
            //    return ActiveRigidbody.velocity;
            return _transformVelocity;
        }

        public void SetAngularVelocity(Vector3 velocity)
        {
            if (_hasRigidbody && !ActiveRigidbody.isKinematic)
                ActiveRigidbody.angularVelocity = velocity;
        }

        public void SetFriction(float friction)
        {
            if (_friction != friction)
            {
                _friction = friction;
                for (var i = 0; i < _colliders.Count; i++)
                {
                    if (_colliders[i] == null)
                        continue;

                    var material = _colliders[i].material;
                    if (material != null)
                        material.dynamicFriction = _friction;
                }
            }
        }

        public void SetBounce(float bounce)
        {
            if (_bounce != bounce)
            {
                _bounce = bounce;
                for (var i = 0; i < _colliders.Count; i++)
                {
                    if (_colliders[i] == null)
                        continue;

                    var material = _colliders[i].material;
                    if (material != null)
                        material.bounciness = _bounce;
                }
            }
        }

        public void SetTriggerVolumeState(bool useAsTrigger)
        {
            for (var i = 0; i < _colliders.Count; i++)
            {
                if (_colliders[i] == null)
                    continue;

                _colliders[i].isTrigger = useAsTrigger;
            }
        }

        public Action<NetworkSpawnerStartedEvent> OnNetworkSpawnerStartedEvent;
        public Action<MonaFixedTickEvent> OnFixedUpdate;

        private bool _mockNetwork;

        private Renderer[] _renderers;
        private Renderer[] _bodyRenderers;
        public Renderer[] Renderers => _renderers;
        public Renderer[] BodyRenderers => _bodyRenderers;

        private void Awake()
        {
            _setActive = gameObject.activeInHierarchy;
            RegisterInParents();
            CacheComponents();
            InitializeTags();
            AddDelegates();
            SetInitialTransforms();
        }

        private void Start()
        {
            FireInstantiated();
            HasRigidbodyInParent();
        }

        private void CacheComponents()
        {
            CacheRenderers();
            _rigidbody = GetComponent<Rigidbody>();
            _activeTransform = transform;
            _activeRigidbody = _rigidbody;
            _hasRigidbody = _rigidbody != null;

            if (SyncType == MonaBodyNetworkSyncType.NetworkRigidbody)
            {
                AddRigidbody();
            }
            _characterController = GetComponent<CharacterController>();
            _camera = GetComponentInChildren<Camera>();
            CacheColliders();
        }

        public void CacheRenderers()
        {
            _renderers = GetComponentsInChildren<Renderer>(true);

            _materialsIndex.Clear();
            for (var i = 0; i < _renderers.Length; i++)
                _materialsIndex[_renderers[i]] = _renderers[i].materials;

            _bodyRenderers = GetComponents<Renderer>();
        }

        public void AddRigidbody()
        {
            if(SyncType != MonaBodyNetworkSyncType.NotNetworked)
                SyncType = MonaBodyNetworkSyncType.NetworkRigidbody;
            if (_rigidbody == null)
            {
                _rigidbody = GetComponent<Rigidbody>();
                if (_rigidbody == null)
                {
                    _rigidbody = gameObject.AddComponent<Rigidbody>();
                    _rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
                }
                _rigidbody.isKinematic = true;
                _activeRigidbody = _rigidbody;
                _hasRigidbody = true;
            }
            if (gameObject.GetComponent<DontGoThroughThings>() == null)
                gameObject.AddComponent<DontGoThroughThings>();
        }

        public void RemoveRigidbody()
        {
            if (SyncType != MonaBodyNetworkSyncType.NotNetworked)
                SyncType = MonaBodyNetworkSyncType.NetworkTransform;
            if (_rigidbody != null)
            {
                Destroy(_rigidbody);
                Destroy(gameObject.GetComponent<DontGoThroughThings>());
                _rigidbody = null;
                _activeRigidbody = null;
                _hasRigidbody = false;
            }
        }

        private Vector3 _baseOffset;
        private bool _hasCharacterController;
        public void CacheColliders()
        {
            _colliders = new List<Collider>();
            _triggers = new List<Collider>();
            var colliders = GetComponentsInChildren<Collider>();
            var referencePosition = GetPosition() - Vector3.up * 100f;
            _baseOffset = referencePosition;
            _hasCharacterController = false;
            var closestDistance = Mathf.Infinity;
            for (var i = 0; i < colliders.Length; i++)
            {
                var collider = colliders[i];
                if (collider != null)
                {
                    if (collider.isTrigger)
                        _triggers.Add(collider);
                    else
                    {
                        _colliders.Add(collider);

                        if (collider is CharacterController)
                            _hasCharacterController = true;

                        var radius = (collider is CharacterController) ? ((CharacterController)collider).radius : 0f;
                        var closest = collider.ClosestPoint(referencePosition) - Vector3.up*radius;
                        var distanceToReference = Vector3.Distance(closest, referencePosition);
                        if (distanceToReference < closestDistance)
                        {
                            closestDistance = distanceToReference;
                            _baseOffset = closest;
                        }
                    }

                }
            }
            _baseOffset = _baseOffset - GetPosition();
        }

        public bool HasCollider()
        {
            return _colliders.Count > 0;
        }

        public void AddCollider()
        {
            for (var i = 0; i < _renderers.Length; i++)
            {
                _renderers[i].AddComponent<BoxCollider>();
                break;
            }
            CacheColliders();
        }

        public Vector3 GetCenter()
        {
            Vector3 pos = Vector3.zero;
            if (_renderers.Length > 0)
            {
                for (var i = 0; i < _renderers.Length; i++)
                {
                    if (_renderers[i] == null) continue;
                    pos += _renderers[i].bounds.center;
                }
                return pos / _renderers.Length;
            }
            else if(_colliders.Count > 0)
            {
                for (var i = 0; i < _colliders.Count; i++)
                {
                    if (_colliders[i] == null) continue;
                    pos += _colliders[i].bounds.center;
                }
                return pos / _colliders.Count;
            }
            return GetPosition();
        }

        public void InitializeTags()
        {
            _monaTagged = new List<IMonaTagged>(transform.GetComponents<IMonaTagged>());
            _monaTagged.Remove(this);

            for (var i = 0; i < MonaTags.Count; i++)
                RegisterInTagRegistry(MonaTags[i]);

            for (var i = 0; i < _monaTagged.Count; i++)
            {
                for (var t = 0; t < _monaTagged[i].MonaTags.Count; t++)
                    RegisterInTagRegistry(_monaTagged[i].MonaTags[t]);
            }
        }

        private void AddDelegates()
        {
            OnNetworkSpawnerStartedEvent = HandleNetworkSpawnerStarted;
            MonaEventBus.Register(new EventHook(MonaCoreConstants.NETWORK_SPAWNER_STARTED_EVENT), OnNetworkSpawnerStartedEvent);
            MonaEventBus.Register(new EventHook(MonaCoreConstants.NETWORK_SPAWNER_STARTED_EVENT, this), OnNetworkSpawnerStartedEvent);

            if (_networkBody == null)
            {
                OnFixedUpdate = HandleFixedUpdate;
                MonaEventBus.Register(new EventHook(MonaCoreConstants.FIXED_TICK_EVENT), OnFixedUpdate);
            }
        }

        public void SetInitialTransforms()
        {
            _initialPosition = ActiveTransform.position;
            _initialLocalPosition = ActiveTransform.localPosition;
            _initialRotation = ActiveTransform.rotation;
            _initialLocalRotation = ActiveTransform.localRotation;
            _initialScale = ActiveTransform.localScale;
        }

        private void RemoveDelegates()
        {
            MonaEventBus.Unregister(new EventHook(MonaCoreConstants.NETWORK_SPAWNER_STARTED_EVENT), OnNetworkSpawnerStartedEvent);
            MonaEventBus.Unregister(new EventHook(MonaCoreConstants.NETWORK_SPAWNER_STARTED_EVENT, this), OnNetworkSpawnerStartedEvent);
        }

        private bool _networkSpawned;
        private void HandleNetworkSpawnerStarted(NetworkSpawnerStartedEvent evt)
        {
            if (_networkSpawned) return;
            _networkSpawned = true;

            if (SyncType != MonaBodyNetworkSyncType.NotNetworked)
            {
                _registerWhenEnabled = true;
                _networkSpawner = evt.NetworkSpawner;
                if (_networkSpawner == null)
                    _mockNetwork = true;
                if (gameObject.activeInHierarchy)
                    RegisterWithNetwork();
            }

            if (SyncType == MonaBodyNetworkSyncType.NetworkRigidbody || _hasRigidbody)
                SetLayer(MonaCoreConstants.LAYER_PHYSICS_GROUP_A, true, true);

            if (SyncType == MonaBodyNetworkSyncType.NotNetworked || _mockNetwork)
            {
                FireSpawnEvent();
                if (DisableOnLoad)
                {
                    _startWhenEnabled = true;
                    OnDisableOnLoad?.Invoke();
                    SetActive(false);
                }
                else
                    OnStarted();
            }

            RemoveDelegates();
        }

        private void OnEnable()
        {
            if(_startWhenEnabled)
            {
                _startWhenEnabled = false;
                OnStarted();
            }

            if (_registerWhenEnabled)
            {
                RegisterWithNetwork();
                _registerWhenEnabled = false;
            }
            //Resume();
        }

        private void OnDisable()
        {
            //Pause();
            OnDisabled?.Invoke(this);
        }

        public void Pause(bool isNetworked = true)
        {
            OnPaused();
            if (isNetworked) _networkBody?.SetPaused(true);
        }

        public void Resume(bool isNetworked = true)
        {
            OnResumed();
            if (isNetworked) _networkBody?.SetPaused(false);
        }

        private void RegisterWithNetwork()
        {
            if (_networkSpawner != null && gameObject != null)
            {
                _networkSpawner.RegisterMonaBody(this);
            }
        }

        public void Destroy()
        {
            GameObject.Destroy(gameObject);
        }

        private bool _destroyed;
        private void OnDestroy()
        {
            _destroyed = true;
            _hasRigidbody = false;
            CleanupTags();
            FireDespawnEvent();
            UnregisterInParents();
            RemoveDelegates();
        }

        private void CleanupTags()
        {
            for (var i = 0; i < MonaTags.Count; i++)
                UnregisterInTagRegistry(MonaTags[i]);

            for (var i = 0; i < _monaTagged.Count; i++)
            {
                for (var t = 0; t < _monaTagged[i].MonaTags.Count; t++)
                    UnregisterInTagRegistry(_monaTagged[i].MonaTags[t]);
            }
        }

        public void SetMaterial(Material material)
        {
            for (var i = 0; i < _renderers.Length; i++)
                SetMaterial(material, i);
        }

        public void SetBodyMaterial(Material material, bool useSharedMaterial = false)
        {
            for (var i = 0; i < _bodyRenderers.Length; i++)
                SetBodyMaterial(material, i, useSharedMaterial);
        }

        public void SetSharedMaterial(Material material)
        {
            for (var i = 0; i < _renderers.Length; i++)
                SetSharedMaterial(material, i);
        }

        private Dictionary<Renderer, Material[]> _materialsIndex = new Dictionary<Renderer, Material[]>();

        public void SetMaterial(Material material, int rendererIndex, int materialSlot = -1)
        {
            var renderer = _renderers[rendererIndex];
            if (materialSlot == -1)
            {
                var materials = _materialsIndex[renderer];
                for (var i = 0; i < materials.Length; i++)
                    materials[i] = material;
                renderer.materials = materials;                
            }
            else
                renderer.materials[materialSlot] = material;
        }

        public void SetBodyMaterial(Material material, int rendererIndex, bool useSharedMaterial, int materialSlot = -1)
        {
            var renderer = _bodyRenderers[rendererIndex];

            if (useSharedMaterial)
            {
                if (materialSlot == -1)
                {
                    var materials = renderer.sharedMaterials;
                    for (var i = 0; i < materials.Length; i++)
                        materials[i] = material;
                    renderer.sharedMaterials = materials;
                }
                else
                    renderer.sharedMaterials[materialSlot] = material;
            }
            else
            {
                if (materialSlot == -1)
                {
                    var materials = _materialsIndex[renderer];
                    for (var i = 0; i < materials.Length; i++)
                        materials[i] = material;
                    renderer.materials = materials;
                }
                else
                    renderer.materials[materialSlot] = material;
            }
        }

        public void SetSharedMaterial(Material material, int rendererIndex, int materialSlot = -1)
        {
            var renderer = _renderers[rendererIndex];
            if (materialSlot == -1)
            {
                var materials = renderer.sharedMaterials;
                for (var i = 0; i < materials.Length; i++)
                    materials[i] = material;
                renderer.sharedMaterials = materials;
            }
            else
                renderer.sharedMaterials[materialSlot] = material;
        }

        public void SetTexture(Texture texture, string textureSlot, bool sharedMaterial)
        {
            for (var i = 0; i < _renderers.Length; i++)
            {
                var material = _renderers[i].sharedMaterial;
                if (!sharedMaterial) material = _renderers[i].material;
                material.SetTexture(textureSlot, texture);
            }
        }

        private void HasRigidbodyInParent()
        {
            var rb = GetComponentsInParent<Rigidbody>(true);
            for (var i = 0; i < rb.Length; i++)
            {
                if (rb[i] != _rigidbody)
                {
                    _hasRigidbodyInParent = true;
                    return;
                }
            }
            _hasRigidbodyInParent = false;
        }

        private void RegisterInParents()
        {
            MonaBodies.Add(this);
            var parents = new List<IMonaBody>(GetComponentsInParent<IMonaBody>(true));
            parents.Remove(this);

            var foundParentBody = false;
            for (var i = 0; i < parents.Count; i++)
            {
                if(!foundParentBody && !(parents[i] is IMonaBodyPart))
                {
                    foundParentBody = true;
                    _parent = parents[i];
                }
                parents[i].RegisterAsChild(this);
            }
        }

        private void UnregisterInParents()
        {
            MonaBodies.Remove(this);
            _parent = null;
            var parents = new List<IMonaBody>(GetComponentsInParent<IMonaBody>(true));
            parents.Remove(this);
            for (var i = 0; i < parents.Count; i++)
                parents[i].UnregisterAsChild(this);
        }

        public void RegisterAsChild(IMonaBody body)
        {
            if (!_childMonaBodies.Contains(body))
                _childMonaBodies.Add(body);

            for (var i = 0; i < body.MonaTags.Count; i++)
                RegisterInChildTagRegistry(body.MonaTags[i], body);

            for (var i = 0; i < _monaTagged.Count; i++)
            {
                for (var t = 0; t < _monaTagged[i].MonaTags.Count; t++)
                    RegisterInChildTagRegistry(_monaTagged[i].MonaTags[t], body);
            }
        }

        public void UnregisterAsChild(IMonaBody body)
        {
            if (_childMonaBodies.Contains(body))
                _childMonaBodies.Remove(body);

            for (var i = 0; i < body.MonaTags.Count; i++)
                UnregisterInChildTagRegistry(body.MonaTags[i], body);

            for (var i = 0; i < _monaTagged.Count; i++)
            {
                for (var t = 0; t < _monaTagged[i].MonaTags.Count; t++)
                    UnregisterInChildTagRegistry(_monaTagged[i].MonaTags[t], body);
            }
        }

        public void SetNetworkMonaBody(INetworkMonaBodyClient obj)
        {
            _networkBody = obj;
            _activeTransform = obj.NetworkTransform;
            _activeRigidbody = obj.NetworkRigidbody;

            FireSpawnEvent();
            if (DisableOnLoad)
            {
                _startWhenEnabled = true;
                SetActive(false);
            }
            else
                OnStarted();


            if (_networkBody != null)
            {
                if(_animator != null)
                    _networkBody?.SetAnimator(_animator);
                MonaEventBus.Unregister(new EventHook(MonaCoreConstants.FIXED_TICK_EVENT), OnFixedUpdate);
            }
        }

        public void SetAnimator(Animator animator)
        {
            _animator = animator;
            _networkBody?.SetAnimator(_animator);
        }

        public void SetUpdateEnabled(bool enabled) => _updateEnabled = enabled;

        public void FixedUpdateNetwork(float deltaTime, bool hasInput, List<MonaInput> inputs)
        {
            if(hasInput)
                ApplyInputs(inputs);

            FireFixedUpdateEvent(deltaTime, false);

            SetGroundedState();
            CalculateVelocity(deltaTime, false);
            ApplyGroundingObjectVelocity();

            ApplyPositionAndRotation();
            ApplyAllForces(deltaTime);
            ApplyDrag();
            //ApplySetActive();
            CalculateVelocity(deltaTime, true);
        }

        public void FixedUpdateNetwork(float deltaTime, bool hasInput, MonaInput input)
        {
            if (hasInput)
                ApplyInput(input);

            FireFixedUpdateEvent(deltaTime, false);

            SetGroundedState();
            CalculateVelocity(deltaTime, false);
            ApplyGroundingObjectVelocity();

            ApplyPositionAndRotation();
            ApplyAllForces(deltaTime);
            ApplyDrag();
            ApplyGroundingObjectVelocity();
            //ApplySetActive();
            CalculateVelocity(deltaTime, true);
        }

        public void StateAuthorityChanged() => FireStateAuthorityChanged();

        public void HandleFixedUpdate(MonaFixedTickEvent evt)
        {
            if (_destroyed) return;
            if (_networkBody == null)
            {
                _hasInput = _monaInputs.Count > 0;
                if (_hasInput)
                {
                    //if(_monaInputs.Count > 1) 
                    //    Debug.Log($"mona inputs count {_monaInputs.Count}");
                    ApplyInputs(_monaInputs);
                    _monaInputs.Clear();
                    _lastInput = default;
                }

                BindPosition();
                BindRotation();

                FireFixedUpdateEvent(evt.DeltaTime, false);

                SetGroundedState();
                CalculateVelocity(evt.DeltaTime, false);
                ApplyGroundingObjectVelocity();

                ApplyPositionAndRotation();
                ApplyAllForces(evt.DeltaTime);
                ApplyDrag();

                if(!_setActive)
                    ApplySetActive();
                CalculateVelocity(evt.DeltaTime, true);

                //TODOif (isNetworked) _networkBody?.SetPosition(position, isKinematic);
                //_hasInput = false;
            }
        }

        private void CalculateVelocity(float deltaTime, bool setLastPosition)
        {
            _transformVelocity = (GetPosition() - _lastPosition) / deltaTime;

            if (setLastPosition)
                _lastPosition = GetPosition();
        }

        private void SetGroundedState()
        {
            if (ActiveRigidbody == null)
                return;

            _grounded = false;

            var hitCount = Physics.RaycastNonAlloc(GetPosition() + _baseOffset + Vector3.up * 0.01f, -Vector3.up, _results, 0.02f, ~0, QueryTriggerInteraction.Ignore);

            if (hitCount > 0)
            {
                for (var i = 0; i < hitCount; i++)
                {
                    if (!_colliders.Contains(_results[i].collider))
                    {
                        _grounded = true;

                        if (_cachedGroundingObject.transform != _results[i].collider.transform)
                            _cachedGroundingObject.Initialize(_results[i].collider.transform, this);

                        break;
                    }
                }
            }

            if (!_grounded && _cachedGroundingObject.TrackingObject)
                _cachedGroundingObject = new GroundingObject();
        }

        private void ApplyGroundingObjectVelocity()
        {
            if (!_grounded || !_cachedGroundingObject.TrackingObject)
                return;

            Vector3 adjustedVelocity = _cachedGroundingObject.AdjustedRiderVelocity;

            if (Mathf.Approximately(adjustedVelocity.magnitude, 0f))
                return;

            if(!ActiveRigidbody.isKinematic)
                ActiveRigidbody.velocity = adjustedVelocity;
        }

        private Vector3 _applyPosition;
        
        private void ApplyAddPosition(Vector3 delta)
        {
            _applyPosition += delta;
        }

        private Quaternion _applyRotation;
        private void ApplyPositionAndRotation()
        {
            if (_rotationDeltas.Count == 0 && _positionDeltas.Count == 0) return;

            _applyRotation = GetRotation();

            for (var i = 0; i < _rotationDeltas.Count; i++)
            {
                var rotation = _rotationDeltas[i];
                ApplyAddRotation(rotation.Rotation);
            }
            _rotationDeltas.Clear();

            _applyPosition = GetPosition();

            for (var i = 0; i < _positionDeltas.Count; i++)
            {
                var position = _positionDeltas[i];
                ApplyAddPosition(position.Direction);
            }

            //Debug.Log($"{Transform.gameObject} apply position {_applyPosition} {Time.frameCount}");

            _positionDeltas.Clear();
            
            
            _applyPosition = _positionBounds.BindValue(_applyPosition);
            _applyRotation = _rotationBounds.BindValue(_applyRotation, ActiveTransform);

            if(_hasRigidbody)
            {
                if(Parent != null)
                {
                    ActiveTransform.position = _applyPosition;
                    ActiveTransform.rotation = _applyRotation;
                }
                else if (ActiveRigidbody.isKinematic)
                {
                    ActiveRigidbody.Move(_applyPosition, _applyRotation);
                }
                else
                {
                    ActiveRigidbody.velocity = Vector3.zero;
                    ActiveRigidbody.angularVelocity = Vector3.zero;
                    ActiveRigidbody.position = _applyPosition;
                    ActiveRigidbody.rotation = _applyRotation.normalized;
                }
            }
            else
            {
                ActiveTransform.position = _applyPosition;
                ActiveTransform.rotation = _applyRotation;
            }
        }

        public void CancelForces()
        {
            ActiveRigidbody.velocity = Vector3.zero;
            ActiveRigidbody.angularVelocity = Vector3.zero;
            _force.Clear();
        }

        private void ApplyAddRotation(Quaternion delta)
        {
            _applyRotation *= delta;
        }

        private void ApplyAllForces(float deltaTime)
        {
            if (!_hasRigidbody)
                return;

            for (var i = 0; i < _force.Count; i++)
            {
                var force = _force[i];
                ActiveRigidbody.AddForce(force.Force, force.Mode);
                //Debug.Log($"{nameof(ApplyAllForces)} {force.Force} {force.Mode}");
            }
            _force.Clear();
        }

        private void ApplyInputs(List<MonaInput> inputs)
        {
            for (var i = 0; i < _monaInputs.Count; i++)
            {
                ApplyInput(_monaInputs[i]);
            }
        }

        private void ApplyInput(MonaInput input)
        {
            MonaEventBus.Trigger(new EventHook(MonaCoreConstants.INPUT_EVENT, (IMonaBody)this), new MonaInputEvent(input));
            FireBodyHasInputEvent();
        }    

        public bool Intersects(SphereCollider collider, bool includeTriggers = false)
        {
            return Intersects((Collider)collider, includeTriggers);
        }

        public bool Intersects(Collider collider, bool includeTriggers = false)
        {
            for (var i = 0; i < _colliders.Count; i++)
            {
                var bodyCollider = _colliders[i];
                if (bodyCollider != null && bodyCollider.bounds.Intersects(collider.bounds))
                    return true;
            }

            if(includeTriggers)
            {
                for (var i = 0; i < _triggers.Count; i++)
                {
                    var bodyCollider = _triggers[i];
                    if (bodyCollider != null && bodyCollider.bounds.Intersects(collider.bounds))
                        return true;
                }
            }
            return false;
        }

        public bool WithinRadius(IMonaBody body, float radius = 1f, bool includeTriggers = false)
        {
            var hasColliders = _colliders.Count > 0 || (includeTriggers && _triggers.Count < 0);
            if (hasColliders)
            {
                for (var i = 0; i < _colliders.Count; i++)
                {
                    var myCollider = _colliders[i];
                    var myPosition = GetPosition();
                    var otherPosition = body.GetPosition();
                    var closestPointToOtherPosition = myCollider.ClosestPointOnBounds(otherPosition);
                    //Debug.Log($"{nameof(WithinRadius)} {Transform.name} other {body.Transform.name} radius {radius} pos {myPosition} other {otherPosition} closestPoint {Vector3.Distance(otherPosition, closestPointToOtherPosition)} dist {Vector3.Distance(myPosition, otherPosition)} ");
                    if (myCollider != null && (Vector3.Distance(otherPosition, closestPointToOtherPosition) < radius || Vector3.Distance(myPosition, otherPosition) < radius))
                        return true;
                }

                if(includeTriggers)
                {
                    for (var i = 0; i < _triggers.Count; i++)
                    {
                        var myCollider = _triggers[i];
                        var myPosition = GetPosition();
                        var otherPosition = body.GetPosition();
                        var closestPointToOtherPosition = myCollider.ClosestPointOnBounds(otherPosition);
                        //Debug.Log($"{nameof(WithinRadius)} {Transform.name} other {body.Transform.name} radius {radius} pos {myPosition} other {otherPosition} closestPoint {Vector3.Distance(otherPosition, closestPointToOtherPosition)} dist {Vector3.Distance(myPosition, otherPosition)} ");
                        if (myCollider != null && (Vector3.Distance(otherPosition, closestPointToOtherPosition) < radius || Vector3.Distance(myPosition, otherPosition) < radius))
                            return true;
                    }

                }
            }
            else
            {
                var myPosition = GetPosition();
                var otherPosition = body.GetPosition();
                if (Vector3.Distance(myPosition, otherPosition) < radius)
                    return true;
            }
            return false;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position + _baseOffset + Vector3.up * 0.01f, -Vector3.up * .2f);
        }

        private RaycastHit[] _results = new RaycastHit[10];
        private GroundingObject _cachedGroundingObject = new GroundingObject();

        private void ApplyDrag()
        {
            if (_hasRigidbody || _hasCharacterController)
            {
                if(_onlyApplyDragWhenGrounded && !_grounded)
                {
                    _grounded = false;
                    //var layerMask = 1 << LayerMask.NameToLayer(MonaCoreConstants.LAYER_PHYSICS_GROUP_A) | 1 << LayerMask.NameToLayer(MonaCoreConstants.LAYER_LOCAL_PLAYER);
                    //Debug.Log($"Raycast {_baseOffset} {GetPosition()}");
                    var hitCount = Physics.RaycastNonAlloc(GetPosition() + _baseOffset + Vector3.up * 0.01f, -Vector3.up, _results, 0.2f, ~0, QueryTriggerInteraction.Ignore);
                    if (hitCount > 0)
                    {
                        for(var i = 0;i < hitCount; i++)
                        {
                            if (!_colliders.Contains(_results[i].collider) && (Mathf.Approximately(Mathf.Abs(_transformVelocity.y), 0) || _results[i].distance < 0.04f))
                            {
                                _grounded = true;
                                break;
                            }
                        }
                        //Debug.Log($"Raycast hit {hit.collider} {_colliders.Contains(hit.collider)}");
                        //_grounded = true;
                    }

                    if(!_grounded)
                    {
                        if (_hasRigidbody)
                        {
                            ActiveRigidbody.drag = 0;
                            ActiveRigidbody.angularDrag = 0;
                        }
                        //Debug.Log($"no drag");
                        return;
                    }
                }

                if (_dragType == DragType.Linear)
                {
                    //Debug.Log($"apply drag {_drag} {_angularDrag} {_dragDivisor} {_angularDragDivisor}");
                    if (_hasRigidbody)
                    {
                        ActiveRigidbody.drag = _drag;
                        ActiveRigidbody.angularDrag = _angularDrag;
                    }
                }
                else
                {
                    //Debug.Log($"apply drag {_drag} {_angularDrag} {_dragDivisor} {_angularDragDivisor}");
                    if (_hasRigidbody)
                    {
                        ActiveRigidbody.drag = _drag * ActiveRigidbody.velocity.magnitude * (ActiveRigidbody.velocity.magnitude / _dragDivisor);
                        ActiveRigidbody.angularDrag = _angularDrag * ActiveRigidbody.velocity.magnitude * (ActiveRigidbody.velocity.magnitude / _angularDragDivisor);
                    }
                }
            }
        }

        private MonaInput _lastInput;
        public void SetLocalInput(MonaInput input)
        {
            if(!_lastInput.Equals(input))
            {
                _lastInput = input;
                _monaInputs.Add(input);
            }
            
            if (_networkBody != null)
                _networkBody.SetLocalInput(input);
        }

        private void FireSpawnEvent()
        {
            MonaEventBus.Trigger<MonaBodySpawnedEvent>(new EventHook(MonaCoreConstants.MONA_BODY_SPAWNED), new MonaBodySpawnedEvent((IMonaBody)this));
        }

        private void FireDespawnEvent()
        {
            MonaEventBus.Trigger<MonaBodyDespawnedEvent>(new EventHook(MonaCoreConstants.MONA_BODY_SPAWNED), new MonaBodyDespawnedEvent((IMonaBody)this));
        }

        private void FireBodyHasInputEvent()
        {
            MonaEventBus.Trigger<MonaBodyHasInputEvent>(new EventHook(MonaCoreConstants.MONA_BODY_HAS_INPUT_EVENT, (IMonaBody)this), new MonaBodyHasInputEvent());
        }

        private void FireFixedUpdateEvent(float deltaTime, bool hasInput)
        {
            MonaEventBus.Trigger<MonaBodyFixedTickEvent>(new EventHook(MonaCoreConstants.MONA_BODY_FIXED_TICK_EVENT, (IMonaBody)this), new MonaBodyFixedTickEvent(deltaTime, hasInput));
        }

        private void FireStateAuthorityChanged()
        {
            MonaEventBus.Trigger<MonaStateAuthorityChangedEvent>(new EventHook(MonaCoreConstants.STATE_AUTHORITY_CHANGED_EVENT, (IMonaBody)this), new MonaStateAuthorityChangedEvent((IMonaBody)this));
        }

        private void FireInstantiated()
        {
            MonaEventBus.Trigger<MonaBodyInstantiatedEvent>(new EventHook(MonaCoreConstants.MONA_BODY_INSTANTIATED), new MonaBodyInstantiatedEvent(this));
        }

        public void SetTransformParent(Transform parent)
        {
            UnregisterInParents();
            ActiveTransform.SetParent(parent, true);
            RegisterInParents();
            HasRigidbodyInParent();
            MonaEventBus.Trigger<MonaBodyParentChangedEvent>(new EventHook(MonaCoreConstants.MONA_BODY_PARENT_CHANGED_EVENT, (IMonaBody)this), new MonaBodyParentChangedEvent());
        }

        public void SetActive(bool active)
        {
            SetActive(active, true);
        }

        public void SetActive(bool active, bool isNetworked)
        {
            if (gameObject != null && active != _setActive)
            {
                _setActive = active;
                _setActiveIsNetworked = isNetworked;
                if(active)
                {
                    ApplySetActive();
                }
            }
        }

        private void ApplySetActive()
        {
            //if (Transform != null && Transform.gameObject != null &&
            if(gameObject.activeInHierarchy != _setActive)
            {
                
                gameObject.SetActive(_setActive);
                if (_hasRigidbody)
                {
                    ActiveRigidbody.isKinematic = true;
                    //ActiveRigidbody.Sleep();
                }
                if (_setActiveIsNetworked) _networkBody?.SetActive(_setActive);
            }
        }

        public bool GetActive()
        {
            return gameObject.activeSelf && gameObject.activeInHierarchy;
        }


        public bool HasControl()
        {
            if (SyncType == MonaBodyNetworkSyncType.NotNetworked || _mockNetwork) return true;
            if (System.Object.ReferenceEquals(_networkBody, null)) return true;
            return _networkBody.HasControl();
        }

        public void TakeControl()
        {
            Debug.Log($"{nameof(MonaBody)}.{nameof(TakeControl)} {this}");
            if (!HasControl())
            {
                _networkBody?.TakeControl();
                OnControlRequested?.Invoke();
            }
        }

        public void ReleaseControl()
        {
            _networkBody?.ReleaseControl();
        }

        public void TriggerRemoteAnimation(string clipName)
        {
            _networkBody?.TriggerAnimation(clipName);
        }

        public void ResetLayer()
        {
            if (_hasRigidbody)
                SetLayer(MonaCoreConstants.LAYER_PHYSICS_GROUP_A, true);
            else
                SetLayer(MonaCoreConstants.LAYER_DEFAULT, true);
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

        public int GetLayer()
        {
            return gameObject.layer;
        }

        public void SetKinematic(bool isKinematic, bool isNetworked = true)
        {
            if (_hasRigidbody && ActiveRigidbody.isKinematic != isKinematic)
            {
                if (!ActiveRigidbody.isKinematic && isKinematic)
                {
                    ActiveRigidbody.velocity = Vector3.zero;
                    ActiveRigidbody.angularVelocity = Vector3.zero;
                }
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

        public void SetShaderColor(string propertyName, Color color)
        {
            for (var i = 0; i < _renderers.Length; i++)
                _renderers[i].material.SetColor(propertyName, color);
        }

        public void SetShaderVector(string propertyName, Vector4 value)
        {
            for (var i = 0; i < _renderers.Length; i++)
                _renderers[i].material.SetVector(propertyName, value);
        }

        public void SetShaderVectorArray(string propertyName, Vector4[] value)
        {
            for (var i = 0; i < _renderers.Length; i++)
                _renderers[i].material.SetVectorArray(propertyName, value);
        }

        public void SetShaderFloat(string propertyName, float value)
        {
            for (var i = 0; i < _renderers.Length; i++)
                _renderers[i].material.SetFloat(propertyName, value);
        }

        public void SetShaderInteger(string propertyName, int value)
        {
            for (var i = 0; i < _renderers.Length; i++)
                _renderers[i].material.SetInt(propertyName, value);
        }

        public Color GetShaderColor(string propertyName)
        {
            if (_renderers.Length > 0)
                return _renderers[0].material.GetColor(propertyName);
            return Color.white;
        }

        public Vector4 GetShaderVector(string propertyName)
        {
            if (_renderers.Length > 0)
                return _renderers[0].material.GetVector(propertyName);
            return Vector4.zero;
        }

        public Vector4[] GetShaderVectorArray(string propertyName)
        {
            if (_renderers.Length > 0)
                return _renderers[0].material.GetVectorArray(propertyName);
            return new Vector4[0];
        }

        public float GetShaderFloat(string propertyName)
        {
            if (_renderers.Length > 0)
                return _renderers[0].material.GetFloat(propertyName);
            return 0f;
        }

        public int GetShaderInteger(string propertyName)
        {
            if (_renderers.Length > 0)
                return _renderers[0].material.GetInteger(propertyName);
            return 0;
        }

        public void SetVisible(bool vis, bool isNetworked = true)
        {
            if (_visible != vis)
            {
                _visible = vis;
                if (_renderers != null && _renderers.Length > 0)
                {
                    for (var i = 0; i < _renderers.Length; i++)
                    {
                        if(_renderers[i] != null)
                            _renderers[i].enabled = _visible;
                    }
                }
                if (isNetworked) _networkBody?.SetVisible(vis);
            }
        }

        public bool GetVisible()
        {
            return _visible;
        }

        public void ApplyForce(Vector3 direction, ForceMode mode, bool isNetworked = true)
        {
            if(_hasRigidbody)
            {
                //Debug.Log($"{nameof(ApplyForce)} {direction} {mode}");
                AddForce(direction, mode);
            }
        }

        private void AddForce(Vector3 force, ForceMode mode)
        {
            _force.Add(new MonaBodyForce() { Force = force, Mode = mode });
        }

        public void MoveDirection(Vector3 direction, bool isNetworked = true)
        {
            AddPosition(direction, isNetworked);
        }

        public void BindPosition()
        {
            if (_hasRigidbody)
            {
                if(!_hasRigidbodyInParent || _positionBounds.RestrictTransform)
                    ActiveRigidbody.position = _positionBounds.BindValue(ActiveTransform.position);
            }
            else
                ActiveTransform.position = _positionBounds.BindValue(ActiveTransform.position);
        }

        public void BindRotation()
        {
            if (_hasRigidbody)
            {
                if(!_hasRigidbodyInParent || _positionBounds.RestrictTransform)
                    ActiveRigidbody.rotation = _rotationBounds.BindValue(ActiveTransform.rotation, ActiveTransform);
            }
            else
                ActiveTransform.rotation = _rotationBounds.BindValue(ActiveTransform.rotation, ActiveTransform);
        }

        public void TeleportPosition(Vector3 position, bool isNetworked = true, bool setToLocal = false)
        {
            _positionDeltas.Clear();

            position = _positionBounds.BindValue(position);
            //Debug.Log($"{nameof(TeleportPosition)} {position} {Time.frameCount}");
            if (_hasRigidbody)
            {
                if (setToLocal)
                    position = ActiveTransform.TransformPoint(position);

                var was = ActiveRigidbody.isKinematic;
                ActiveRigidbody.isKinematic = true;
                ActiveRigidbody.position = position;
                ActiveRigidbody.isKinematic = was;
                ActiveTransform.position = position;
            }
            else
            {
                if (setToLocal)
                    ActiveTransform.localPosition = position;
                else
                    ActiveTransform.position = position;
            }
                
            if (isNetworked) _networkBody?.TeleportPosition(position);
        }

        public void TeleportRotation(Quaternion rotation, bool isNetworked = true)
        {
            rotation = _rotationBounds.BindValue(rotation, ActiveTransform);
            if (_hasRigidbody)
            {
                var was = ActiveRigidbody.isKinematic;
                ActiveRigidbody.isKinematic = true;
                ActiveRigidbody.rotation = rotation;
                ActiveRigidbody.isKinematic = was;
                ActiveTransform.rotation = rotation;
            }
            else
                ActiveTransform.rotation = rotation;
            if (isNetworked) _networkBody?.TeleportRotation(rotation);
        }

        public void SetPosition(Vector3 position, bool isNetworked = true)
        {
            Vector3 currentPosition = GetPosition();
            //Debug.Log($"{nameof(MonaBody)}.{nameof(SetPosition)} {position} delta: {position - currentPosition}");
            AddPosition(position - currentPosition, isNetworked);
        }

        public void TeleportScale(Vector3 scale, bool isNetworked = true)
        {
            ActiveTransform.localScale = scale;
            if (_hasRigidbody)
            {
                ActiveRigidbody.transform.localScale = scale;
            }
            if (isNetworked) _networkBody?.TeleportScale(scale);
        }

        public void SetSpawnTransforms(Vector3 position, Quaternion rotation, Vector3 scale, bool spawnedAsChild, bool isNetworked = true)
        {
            position = _positionBounds.BindValue(position);
            rotation = _rotationBounds.BindValue(rotation, ActiveTransform);

            if (spawnedAsChild)
            {
                ActiveTransform.localPosition = position;
                ActiveTransform.localRotation = rotation;
            }
            else
            {
                ActiveTransform.position = position;
                ActiveTransform.rotation = rotation;
            }

            _initialPosition = ActiveTransform.position;
            _initialRotation = ActiveTransform.rotation;
            _initialLocalPosition = ActiveTransform.localPosition;
            _initialLocalRotation = ActiveTransform.localRotation;

            ActiveTransform.localScale = _initialScale = scale;

            //TODO: I don't like this. sometimes child monabodies are getting destroyed and not cleaning themselves up.
            for (var i = _childMonaBodies.Count - 1; i >= 0; i--)
            {
                try
                {
                    _childMonaBodies[i].SetInitialTransforms();
                }
                catch(Exception e)
                {
                    _childMonaBodies.RemoveAt(i);
                }
            }


            if (isNetworked)
            {
                _networkBody?.TeleportPosition(position);
                _networkBody?.TeleportRotation(rotation);
                _networkBody?.TeleportScale(scale);
            }
        }

        public void AddPosition(Vector3 dir, bool isNetworked = true)
        {
            _positionDeltas.Add(new MonaBodyDirection() { Direction = dir });
        }

        public Vector3 GetPosition()
        {
            if (_destroyed) return default;
            if (_hasRigidbody)
                return ActiveRigidbody.position;
            else
                return ActiveTransform.position;
        }

        public Quaternion GetRotation()
        {
            if (_hasRigidbody)
                return ActiveRigidbody.rotation;
            else
                return ActiveTransform.rotation;
        }
        /* TODO
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
        */

        public void RotateAround(Vector3 direction, float angle, bool isNetworked = true)
        {
            if (_hasRigidbody)
            {
                SetRotation(Quaternion.AngleAxis(angle, direction), isNetworked);
            }
            else
            {
                SetRotation(Quaternion.AngleAxis(angle, direction), isNetworked);
            }
        }

        public void SetRotation(Quaternion rotation, bool isNetworked = true)
        {
            _rotationDeltas.Add(new MonaBodyRotation() { Rotation = rotation });
        }

        public void SetScale(Vector3 scale, bool isNetworked = true)
        {
            if (!ActiveTransform.localScale.Equals(scale))
            {
                ActiveTransform.localScale = scale;
                if (isNetworked) _networkBody?.SetScale(scale);
            }
        }

        public Vector3 GetScale()
        {
            return ActiveTransform.localScale;
        }

        public List<IMonaBody> GetTagsByDistance(string tag)
        {
            var tags = FindByTag(tag);

            if (tags.Count < 1)
                return tags;

            tags.Sort((a, b) =>
            {
                var dista = Vector3.Distance(GetPosition(), a.GetPosition());
                var distb = Vector3.Distance(GetPosition(), b.GetPosition());
                return dista.CompareTo(distb);
            });

            return tags;
        }

        public IMonaBody GetClosestTag(string tag)
        {
            var tags = GetTagsByDistance(tag);

            return tags.Count > 0 ? tags[0] : null;
        }

        public IMonaBody GetFurthestTag(string tag)
        {
            var tags = GetTagsByDistance(tag);
            return tags.Count > 0 ? tags[tags.Count -1] : null;
        }

        public IMonaBody GetNextTag(string tag, float closestExludedDistance)
        {
            var tags = GetTagsByDistance(tag);

            if (tags.Count < 1)
                return null;

            if (tags.Count == 1)
                return tags[0];

            float distanceToClosest = Vector3.Distance(GetPosition(), tags[0].GetPosition());

            return distanceToClosest > closestExludedDistance ? tags[0] : tags[1];
        }
    }
}
