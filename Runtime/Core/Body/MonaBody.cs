using UnityEngine;
using Unity.VisualScripting;
using System.Collections.Generic;
using Mona.SDK.Core.Network;
using Mona.SDK.Core.Events;
using System;
using System.Collections;
using Mona.SDK.Core.Body.Enums;
using Mona.SDK.Core.Network.Interfaces;
using Mona.SDK.Core.Input;

namespace Mona.SDK.Core.Body
{
    public class MonaBody : MonaBodyBase, IMonaBody, IMonaTagged
    {
        public event Action OnStarted = delegate { };
        public event Action<IMonaBody> OnDisabled = delegate { };
        public event Action OnResumed = delegate { };
        public event Action OnPaused = delegate { };
        public event Action OnControlRequested = delegate { };

        private bool _registerWhenEnabled;
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
        private MonaBodyAttachType _attachType = MonaBodyAttachType.None;

        private Vector3 _initialPosition = Vector3.zero;
        private Vector3 _initialLocalPosition = Vector3.zero;
        private Quaternion _initialRotation = Quaternion.identity;
        private Quaternion _initialLocalRotation = Quaternion.identity;
        private Vector3 _initialScale = Vector3.one;

        private MonaBodyTransformBounds _positionBounds = new MonaBodyTransformBounds();

        public bool IsAttachedToRemotePlayer() => _attachType == MonaBodyAttachType.RemotePlayer;
        public bool IsAttachedToLocalPlayer() => _attachType == MonaBodyAttachType.LocalPlayer;

        public IMonaBody Parent => _parent;

        public bool IsNetworked => _networkBody != null;
        public Transform ActiveTransform => _networkBody != null ? _networkBody.NetworkTransform : ((transform != null) ? transform : null);
        public Rigidbody ActiveRigidbody => _networkBody != null ? _networkBody.NetworkRigidbody : ((_rigidbody != null) ? _rigidbody : null);
        public Transform Transform => (_destroyed) ? null : transform;
        public float DeltaTime => _networkBody != null ? _networkBody.DeltaTime : Time.deltaTime;
        public Camera Camera => _camera;
        public INetworkMonaBodyClient NetworkBody => _networkBody;
        public Animator Animator => _animator;
        public MonaBodyAttachType AttachType { get => _attachType; set => _attachType = value; }

        public Vector3 InitialPosition => _initialPosition;
        public Vector3 InitialLocalPosition => _initialLocalPosition;
        public Quaternion InitialRotation => _initialRotation;
        public Quaternion InitialLocalRotation => _initialLocalRotation;
        public Vector3 InitialScale => _initialScale;

        public MonaBodyTransformBounds PositionBounds { get => _positionBounds; set => _positionBounds = value; }

        private bool _grounded;
        public bool Grounded => _grounded;

        private bool _setActive = true;
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
        private List<IMonaBody> _childMonaBodies = new List<IMonaBody>();

        private bool _hasInput;
        private List<MonaInput> _monaInputs = new List<MonaInput>();

        private List<IMonaTagged> _monaTagged = new List<IMonaTagged>();

        public bool HasMonaTag(string tag) => MonaTags.Contains(tag) || _monaTagged.Find(x => x.HasMonaTag(tag)) != null;
        public void AddTag(string tag)
        {
            if (!HasMonaTag(tag))
                MonaTags.Add(tag);
        }
        public void RemoveTag(string tag)
        {
            if (HasMonaTag(tag))
                MonaTags.Remove(tag);
        }
        public static List<IMonaBody> FindByTag(string tag) => MonaBodies.FindAll((x) => x.HasMonaTag(tag));
        public static IMonaBody FindByLocalId(string localId) => MonaBodies.Find((x) => x.LocalId == localId);
        public IMonaBody FindChildByTag(string tag) => _childMonaBodies.Find((x) => x.HasMonaTag(tag));
        public Transform FindChildTransformByTag(string tag) => _childMonaBodies.Find((x) => x.HasMonaTag(tag))?.ActiveTransform;
        public List<IMonaBody> FindChildrenByTag(string tag) => _childMonaBodies.FindAll((x) => x.HasMonaTag(tag));
        public List<IMonaBody> Children() => _childMonaBodies;

        public Transform GetTransformParent() => ActiveTransform.parent;

        public void SetDragType(DragType dragType) => _dragType = dragType;
        public void SetOnlyApplyDragWhenGrounded(bool apply) => _onlyApplyDragWhenGrounded = apply;
        public void SetApplyPinOnGrounded(bool apply) => _applyPinOnGrounded = apply;

        public void SetDrag(float drag)
        {
            _drag = drag;
            if (ActiveRigidbody != null)
                ActiveRigidbody.drag = drag;
        }

        public void SetAngularDrag(float drag)
        {
            _angularDrag = drag;
            if (ActiveRigidbody != null)
                ActiveRigidbody.angularDrag = drag;
        }

        public void SetUseGravity(bool useGravity)
        {
            if (ActiveRigidbody != null)
                ActiveRigidbody.useGravity = useGravity;
        }

        public void SetVelocity(Vector3 velocity)
        {
            if (ActiveRigidbody != null)
                ActiveRigidbody.velocity = velocity;
        }

        public Vector3 GetVelocity()
        {
            if (ActiveRigidbody != null)
                return ActiveRigidbody.velocity;
            return _transformVelocity;
        }

        public void SetAngularVelocity(Vector3 velocity)
        {
            if (ActiveRigidbody != null)
                ActiveRigidbody.velocity = velocity;
        }

        public void SetFriction(float friction)
        {
            if (_friction != friction)
            {
                _friction = friction;
                for (var i = 0; i < _colliders.Count; i++)
                {
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
                    var material = _colliders[i].material;
                    if (material != null)
                        material.bounciness = _bounce;
                }
            }
        }

        public Action<NetworkSpawnerStartedEvent> OnNetworkSpawnerStartedEvent;
        public Action<MonaFixedTickEvent> OnFixedUpdate;

        private bool _mockNetwork;

        private Renderer[] _renderers;

        private void Awake()
        {
            _setActive = gameObject.activeInHierarchy;
            RegisterInParents();
            CacheComponents();
            InitializeTags();
            AddDelegates();
            SetInitialTransforms();
        }

        private void CacheComponents()
        {
            _renderers = GetComponentsInChildren<Renderer>(true);
            _rigidbody = GetComponent<Rigidbody>();
            if(SyncType == MonaBodyNetworkSyncType.NetworkRigidbody)
            {
                AddRigidbody();
            }
            _characterController = GetComponent<CharacterController>();
            _camera = GetComponentInChildren<Camera>();
            CacheColliders();
        }

        public void AddRigidbody()
        {
            if(SyncType != MonaBodyNetworkSyncType.NotNetworked)
                SyncType = MonaBodyNetworkSyncType.NetworkRigidbody;
            if (_rigidbody == null)
            {
                _rigidbody = GetComponent<Rigidbody>();
                if(_rigidbody == null)
                    _rigidbody = gameObject.AddComponent<Rigidbody>();
                _rigidbody.isKinematic = true;
                _rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
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
            }
        }

        public void CacheColliders()
        {
            _colliders = new List<Collider>();
            _triggers = new List<Collider>();
            var colliders = GetComponentsInChildren<Collider>();
            for (var i = 0; i < colliders.Length; i++)
            {
                var collider = colliders[i];
                if (collider != null)
                {
                    if (collider.isTrigger)
                        _triggers.Add(collider);
                    else
                        _colliders.Add(collider);
                }
            }
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
        }

        private void AddDelegates()
        {
            OnNetworkSpawnerStartedEvent = HandleNetworkSpawnerStarted;
            EventBus.Register(new EventHook(MonaCoreConstants.NETWORK_SPAWNER_STARTED_EVENT), OnNetworkSpawnerStartedEvent);
            EventBus.Register(new EventHook(MonaCoreConstants.NETWORK_SPAWNER_STARTED_EVENT, this), OnNetworkSpawnerStartedEvent);

            if (_networkBody == null)
            {
                OnFixedUpdate = HandleFixedUpdate;
                EventBus.Register(new EventHook(MonaCoreConstants.FIXED_TICK_EVENT), OnFixedUpdate);
            }
        }

        private void SetInitialTransforms()
        {
            _initialPosition = transform.position;
            _initialLocalPosition = transform.localPosition;
            _initialRotation = transform.rotation;
            _initialLocalRotation = transform.localRotation;
            _initialScale = transform.localScale;
        }

        private void RemoveDelegates()
        {
            EventBus.Unregister(new EventHook(MonaCoreConstants.NETWORK_SPAWNER_STARTED_EVENT), OnNetworkSpawnerStartedEvent);
            EventBus.Unregister(new EventHook(MonaCoreConstants.NETWORK_SPAWNER_STARTED_EVENT, this), OnNetworkSpawnerStartedEvent);
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

            if (SyncType == MonaBodyNetworkSyncType.NetworkRigidbody || ActiveRigidbody != null)
                SetLayer(MonaCoreConstants.LAYER_PHYSICS_GROUP_A, true, true);

            if (SyncType == MonaBodyNetworkSyncType.NotNetworked || _mockNetwork)
            {
                FireSpawnEvent();
                OnStarted();
                if (DisableOnLoad)
                    SetActive(false);
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

        private bool _destroyed;
        private void OnDestroy()
        {
            _destroyed = true;
            FireDespawnEvent();
            UnregisterInParents();
            RemoveDelegates();
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
        }

        public void UnregisterAsChild(IMonaBody body)
        {
            if (_childMonaBodies.Contains(body))
                _childMonaBodies.Remove(body);
        }

        public void SetNetworkMonaBody(INetworkMonaBodyClient obj)
        {
            _networkBody = obj;
            FireSpawnEvent();
            OnStarted();
            if (DisableOnLoad)
                SetActive(false);


            if (_networkBody != null)
            {
                if(_animator != null)
                    _networkBody?.SetAnimator(_animator);
                EventBus.Unregister(new EventHook(MonaCoreConstants.FIXED_TICK_EVENT), OnFixedUpdate);
            }
        }

        public void SetAnimator(Animator animator)
        {
            _animator = animator;
            _networkBody?.SetAnimator(_animator);
        }

        public void SetUpdateEnabled(bool enabled) => _updateEnabled = enabled;

        public void FixedUpdateNetwork(float deltaTime, bool hasInput, MonaInput input)
        {
            if(hasInput)
                ApplyInput(input);

            FireFixedUpdateEvent(deltaTime, hasInput);

            ApplyPositionAndRotation();
            ApplyAllForces(deltaTime);
            ApplyDrag();
            ApplySetActive();
            CalculateVelocity();
        }

        public void FixedUpdateNetwork(float deltaTime, bool hasInput, List<MonaInput> inputs)
        {
            if (hasInput && inputs != null)
            {
                ApplyInputs(inputs);
            }

            FireFixedUpdateEvent(deltaTime, hasInput);

            ApplyPositionAndRotation();
            ApplyAllForces(deltaTime);
            ApplyDrag();
            ApplySetActive();
            CalculateVelocity();
        }

        private void CalculateVelocity()
        {
            _transformVelocity = GetPosition() - _lastPosition;
            _lastPosition = GetPosition();
        }

        public void StateAuthorityChanged() => FireStateAuthorityChanged();

        public void HandleFixedUpdate(MonaFixedTickEvent evt)
        {
            if (_destroyed) return;
            if (_networkBody == null)
            {
                _hasInput = _monaInputs.Count > 0;
                if(_hasInput)
                {
                    if(_monaInputs.Count > 1) Debug.Log($"mona inputs count {_monaInputs.Count}");
                    var input = _monaInputs[0];
                    _monaInputs.RemoveAt(0);
                    ApplyInput(input);
                }

                FireFixedUpdateEvent(evt.DeltaTime, _hasInput);

                ApplyPositionAndRotation();
                ApplyAllForces(evt.DeltaTime);
                ApplyDrag();
                ApplySetActive();
                CalculateVelocity();

                //TODOif (isNetworked) _networkBody?.SetPosition(position, isKinematic);
                //_hasInput = false;
            }
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

            if (ActiveRigidbody != null)
                _applyRotation = ActiveRigidbody.rotation;
            else
                _applyRotation = ActiveTransform.rotation;

            for (var i = 0; i < _rotationDeltas.Count; i++)
            {
                var rotation = _rotationDeltas[i];
                ApplyAddRotation(rotation.Rotation);
            }
            _rotationDeltas.Clear();


            if (ActiveRigidbody != null)
                _applyPosition = ActiveRigidbody.position;
            else
                _applyPosition = ActiveTransform.position;

            for (var i = 0; i < _positionDeltas.Count; i++)
            {
                var position = _positionDeltas[i];
                ApplyAddPosition(position.Direction);
            }
            _positionDeltas.Clear();

            _applyPosition = PositionBounds.BindValue(_applyPosition);

            if(ActiveRigidbody != null)
            {
                if (ActiveRigidbody.isKinematic)
                {
                    ActiveRigidbody.Move(_applyPosition, _applyRotation);
                }
                else
                {
                    ActiveRigidbody.position = _applyPosition;
                    ActiveRigidbody.rotation = _applyRotation;
                }
            }
            else
            {
                ActiveTransform.position = _applyPosition;
                ActiveTransform.rotation = _applyRotation;
            }
        }

        private void ApplyAddRotation(Quaternion delta)
        {
            _applyRotation *= delta;
        }

        private void ApplyAllForces(float deltaTime)
        {
            if (ActiveRigidbody != null)
            {
                for (var i = 0; i < _force.Count; i++)
                {
                    var force = _force[i];
                    ActiveRigidbody.AddForce(force.Force, force.Mode);
                    //Debug.Log($"{nameof(ApplyAllForces)} {force.Force} {force.Mode}");
                }
                _force.Clear();
            }
        }

        private void ApplyInput(MonaInput input)
        {
            EventBus.Trigger(new EventHook(MonaCoreConstants.INPUT_EVENT, (IMonaBody)this), new MonaInputEvent(input));
        }    

        private void ApplyInputs(List<MonaInput> inputs)
        {
            EventBus.Trigger(new EventHook(MonaCoreConstants.INPUTS_EVENT, (IMonaBody)this), new MonaInputsEvent(inputs));
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

        public bool WithinRadius(IMonaBody body, float radius = 1f)
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
            return false;
        }

        private void ApplyDrag()
        {
            if (ActiveRigidbody != null)
            {
                if(_onlyApplyDragWhenGrounded)
                {
                    _grounded = false;
                    RaycastHit hit;
                    var layerMask = 1 << LayerMask.NameToLayer(MonaCoreConstants.LAYER_PHYSICS_GROUP_A) | 1 << LayerMask.NameToLayer(MonaCoreConstants.LAYER_LOCAL_PLAYER);
                    if (Physics.Raycast(ActiveRigidbody.position+Vector3.up*0.01f, -Vector3.up, out hit, 0.02f, ~layerMask))
                    {
                        _grounded = true;
                    }

                    if(!_grounded)
                    {
                        ActiveRigidbody.drag = 0;
                        ActiveRigidbody.angularDrag = 0;
                        //Debug.Log($"no drag");
                        return;
                    }
                }

                if (_dragType == DragType.Linear)
                {
                    //Debug.Log($"apply drag {_drag} {_angularDrag} {_dragDivisor} {_angularDragDivisor}");
                    ActiveRigidbody.drag = _drag;
                    ActiveRigidbody.angularDrag = _angularDrag;
                }
                else
                {
                    //Debug.Log($"apply drag {_drag} {_angularDrag} {_dragDivisor} {_angularDragDivisor}");
                    ActiveRigidbody.drag = _drag * ActiveRigidbody.velocity.magnitude * (ActiveRigidbody.velocity.magnitude / _dragDivisor);
                    ActiveRigidbody.angularDrag = _angularDrag * ActiveRigidbody.velocity.magnitude * (ActiveRigidbody.velocity.magnitude / _angularDragDivisor);
                }
            }
        }

        private MonaInput _lastInput;
        public void SetLocalInput(MonaInput input)
        {
            if(_monaInputs.Count == 0 || !_lastInput.Equals(input))
            {
                _lastInput = input;
                _monaInputs.Add(input);
            }
            
            if (_networkBody != null)
                _networkBody.SetLocalInput(input);
        }

        private void FireSpawnEvent()
        {
            EventBus.Trigger<MonaBodySpawnedEvent>(new EventHook(MonaCoreConstants.MONA_BODY_SPAWNED), new MonaBodySpawnedEvent((IMonaBody)this));
        }

        private void FireDespawnEvent()
        {
            EventBus.Trigger<MonaBodyDespawnedEvent>(new EventHook(MonaCoreConstants.MONA_BODY_SPAWNED), new MonaBodyDespawnedEvent((IMonaBody)this));
        }

        private void FireFixedUpdateEvent(float deltaTime, bool hasInput)
        {
            EventBus.Trigger<MonaBodyFixedTickEvent>(new EventHook(MonaCoreConstants.MONA_BODY_FIXED_TICK_EVENT, (IMonaBody)this), new MonaBodyFixedTickEvent(deltaTime, hasInput));
        }

        private void FireStateAuthorityChanged()
        {
            EventBus.Trigger<MonaStateAuthorityChangedEvent>(new EventHook(MonaCoreConstants.STATE_AUTHORITY_CHANGED_EVENT, (IMonaBody)this), new MonaStateAuthorityChangedEvent((IMonaBody)this));
        }

        public void SetTransformParent(Transform parent)
        {
            UnregisterInParents();
            ActiveTransform.SetParent(parent, true);
            RegisterInParents();
            EventBus.Trigger<MonaBodyParentChangedEvent>(new EventHook(MonaCoreConstants.MONA_BODY_PARENT_CHANGED_EVENT, (IMonaBody)this), new MonaBodyParentChangedEvent());
        }

        public void SetActive(bool active, bool isNetworked = true)
        {
            if (Transform.gameObject.activeInHierarchy != active)
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
            if (Transform != null && Transform.gameObject != null && Transform.gameObject.activeInHierarchy != _setActive)
            { 
                Transform.gameObject.SetActive(_setActive);
                if (ActiveRigidbody != null)
                {
                    ActiveRigidbody.isKinematic = true;
                    ActiveRigidbody.Sleep();
                }
                if (_setActiveIsNetworked) _networkBody?.SetActive(_setActive);
            }
        }

        public bool GetActive()
        {
            return Transform.gameObject.activeInHierarchy;
        }


        public bool HasControl()
        {
            if (SyncType == MonaBodyNetworkSyncType.NotNetworked || _mockNetwork) return true;
            if (_networkBody == null) return true;
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
            if (ActiveRigidbody != null)
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
            if (ActiveRigidbody != null && ActiveRigidbody.isKinematic != isKinematic)
            {
                ActiveRigidbody.isKinematic = isKinematic;
                if (isKinematic)
                {
                    ActiveRigidbody.velocity = Vector3.zero;
                    ActiveRigidbody.angularVelocity = Vector3.zero;
                }
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

        public void ApplyForce(Vector3 direction, ForceMode mode, bool isNetworked = true)
        {
            if(ActiveRigidbody != null)
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

        public void TeleportPosition(Vector3 position, bool isNetworked = true)
        {
            if (ActiveRigidbody != null)
            {
                ActiveRigidbody.position = position;
            }
            else
            {
                ActiveTransform.position = position;
            }
            if (isNetworked) _networkBody?.TeleportPosition(position);
        }

        public void TeleportRotation(Quaternion rotation, bool isNetworked = true)
        {
            if (ActiveRigidbody != null)
            {
                ActiveRigidbody.rotation = rotation;
            }
            else
            {
                ActiveTransform.rotation = rotation;
            }
            if (isNetworked) _networkBody?.TeleportRotation(rotation);
        }

        public void SetPosition(Vector3 position, bool isNetworked = true)
        {
            Vector3 currentPosition = ActiveTransform.position;
            if (ActiveRigidbody != null)
                currentPosition = ActiveRigidbody.position;
            AddPosition(position - currentPosition, isNetworked);
        }

        public void TeleportScale(Vector3 scale, bool isNetworked = true)
        {
            ActiveTransform.localScale = scale;
            if (SyncType == MonaBodyNetworkSyncType.NetworkRigidbody || _rigidbody != null)
            {
                ActiveRigidbody.transform.localScale = scale;
            }
            if (isNetworked) _networkBody?.TeleportScale(scale);
        }

        public void AddPosition(Vector3 dir, bool isNetworked = true)
        {
            _positionDeltas.Add(new MonaBodyDirection() { Direction = dir });
        }

        public Vector3 GetPosition()
        {
            if (ActiveRigidbody != null)
                return ActiveRigidbody.position;
            else
                return ActiveTransform.position;
        }

        public Quaternion GetRotation()
        {
            if (ActiveRigidbody != null)
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
            if (ActiveRigidbody != null)
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

    }
}
