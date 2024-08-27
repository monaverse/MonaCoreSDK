using Mona.SDK.Core.Body.Enums;
using Mona.SDK.Core.Events;
using Unity.VisualScripting;
using UnityEngine;
using Mona.SDK.Core.Utils;

///taken from http://wiki.unity3d.com/index.php?title=DontGoThroughThings#C.23_-_DontGoThroughThings.cs

namespace Mona.SDK.Core.Body
{
    public class DontGoThroughThings : MonoBehaviour
    {
        public bool disableThisScript = false;
        // Careful when setting this to true - it might cause double
        // events to be fired - but it won't pass through the trigger
        public bool sendTriggerMessage = false;
        public LayerMask layerMask = -1; //make sure we aren't in this layer
        public float skinWidth = 0.5f; //probably doesn't need to be changed

        private float _minimumExtent;
        private float _partialExtent;
        private float _sqrMinimumExtent;
        private Vector3 _previousCenter;
        private Vector3 _previousPosition;
        private Collider _myCollider;
        private IMonaBody _body;

        public bool debug;

        private void Awake()
        {
            _body = GetComponent<IMonaBody>();
            _body.OnStarted += HandleStarted;
            CacheCollider();
        }

        private void CacheCollider()
        {
            var colliders = GetComponentsInChildren<Collider>();
            for (var i = 0; i < colliders.Length; i++)
            {
                var collider = colliders[i];
                if (collider != null && !collider.isTrigger)
                {
                    _myCollider = collider;
                    break;
                }
            }
        }

        private void OnDestroy()
        {
            _body.OnStarted -= HandleStarted;
        }

        private void HandleStarted()
        {
            _previousCenter = _body.GetCenter();
            _previousPosition = _body.GetPosition();
        }

        private void Start()
        {
            //_myCollider = _item.skin.skinCollider;
            if (_myCollider == null) return;
            _previousCenter = _body.GetPosition() + Vector3.up * .5f;
            _previousPosition = _body.GetPosition();
            _minimumExtent = Mathf.Min(Mathf.Min(_myCollider.bounds.extents.x, _myCollider.bounds.extents.y), _myCollider.bounds.extents.z);
            _partialExtent = _minimumExtent * (1.0f - skinWidth);
            _sqrMinimumExtent = (_minimumExtent * .1f) * (_minimumExtent * .1f);
        }

        private void FixedUpdate()
        {
            if (disableThisScript) return;

            if (_body.ActiveRigidbody == null) return;

            if (_body.ActiveRigidbody != null && _body.ActiveRigidbody.isKinematic)
            {
                //_previousPosition = _body.ActiveRigidbody.position;
                //return;
            }
            //have we moved more than our minimum extent?
            Vector3 movementThisStep = _body.GetPosition()+Vector3.up*.5f - _previousCenter;
            float movementSqrMagnitude = movementThisStep.sqrMagnitude;

            if(debug && movementSqrMagnitude > 0) Debug.Log($"{nameof(DontGoThroughThings)} {movementSqrMagnitude} {_sqrMinimumExtent}");
            if (movementThisStep.magnitude > Mathf.Epsilon)
            {
                float movementMagnitude = Mathf.Sqrt(movementSqrMagnitude);
                RaycastHit hitInfo;

                //check for obstructions we might have missed
                if (UnityEngine.Physics.Raycast(_previousCenter, movementThisStep.normalized, out hitInfo, _minimumExtent, ~0, QueryTriggerInteraction.Ignore))
                {
                    if (hitInfo.collider != null && hitInfo.collider.attachedRigidbody != _body.ActiveRigidbody)
                    {
                        if (hitInfo.collider.isTrigger && _myCollider != null)
                            hitInfo.collider.SendMessage("OnTriggerEnter", _myCollider, SendMessageOptions.DontRequireReceiver);

                        if (!hitInfo.collider.isTrigger)
                        {
                            var point = hitInfo.point;
                            var dir = point - _previousCenter;
                            if (Mathf.Approximately(dir.x, 0f)) dir.x = 0f;
                            if (Mathf.Approximately(dir.y, 0f)) dir.y = 0f;
                            if (Mathf.Approximately(dir.z, 0f)) dir.z = 0f;

                            //Debug.Log($"Travelled through collider {point} {dir} {movementThisStep.normalized} {Vector3.Scale(dir.normalized, _myCollider.bounds.extents)} {_previousCenter} {hitInfo.collider} {hitInfo.distance} extent {_partialExtent}");
                            MonaEventBus.Trigger(new EventHook(MonaCoreConstants.MONA_BODY_EVENT, _body), new MonaBodyEvent(MonaBodyEventType.OnStop));

                            if (_myCollider != null && movementThisStep.magnitude > Mathf.Epsilon)
                                _body.TeleportPosition(point - Vector3.Scale(dir.normalized, _myCollider.bounds.extents) - Vector3.up*0.5f); // * _partialExtent;
                        }
                    }
                }
                /*
                //check for obstructions we might have exited
                if (Physics.Raycast(_myRigidbody.position, -movementThisStep, out hitInfo, movementMagnitude, layerMask.value))
                {
                  if (!hitInfo.collider)
                    return;

                  if (sendTriggerMessage && hitInfo.collider.isTrigger)
                    hitInfo.collider.SendMessage("OnTriggerExit", _myCollider);
                }*/
            }

            _previousCenter = _body.GetPosition() + Vector3.up * .5f;
            _previousPosition = _body.GetPosition();
        }
    }
}