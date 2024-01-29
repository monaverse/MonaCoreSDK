using UnityEngine;

///taken from http://wiki.unity3d.com/index.php?title=DontGoThroughThings#C.23_-_DontGoThroughThings.cs

namespace Mona.SDK.Core.Body
{
    public class DontGoThroughThings : MonoBehaviour
    {
        // Careful when setting this to true - it might cause double
        // events to be fired - but it won't pass through the trigger
        public bool sendTriggerMessage = false;
        public LayerMask layerMask = -1; //make sure we aren't in this layer
        public float skinWidth = 0.5f; //probably doesn't need to be changed

        private float _minimumExtent;
        private float _partialExtent;
        private float _sqrMinimumExtent;
        private Vector3 _previousPosition;
        private Collider _myCollider;
        private IMonaBody _body;

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
            _previousPosition = transform.position;
        }

        private void Start()
        {
            //_myCollider = _item.skin.skinCollider;
            if (_myCollider == null) return;
            _previousPosition = _body.ActiveRigidbody.position;
            _minimumExtent = Mathf.Min(Mathf.Min(_myCollider.bounds.extents.x, _myCollider.bounds.extents.y), _myCollider.bounds.extents.z);
            _partialExtent = _minimumExtent * (1.0f - skinWidth);
            _sqrMinimumExtent = _minimumExtent * _minimumExtent;
        }

        private void FixedUpdate()
        {
            if (_body.ActiveRigidbody.isKinematic)
            {
                _previousPosition = _body.ActiveRigidbody.position;
                return;
            }
            //have we moved more than our minimum extent?
            Vector3 movementThisStep = _body.ActiveRigidbody.position - _previousPosition;
            float movementSqrMagnitude = movementThisStep.sqrMagnitude;

            if (movementSqrMagnitude > _sqrMinimumExtent)
            {
                float movementMagnitude = Mathf.Sqrt(movementSqrMagnitude);
                RaycastHit hitInfo;

                //check for obstructions we might have missed
                if (UnityEngine.Physics.Raycast(_previousPosition, movementThisStep, out hitInfo, movementMagnitude, layerMask.value))
                {
                    if (hitInfo.collider != null && hitInfo.collider.attachedRigidbody != _body.ActiveRigidbody)
                    {
                        if (hitInfo.collider.isTrigger && _myCollider != null)
                            hitInfo.collider.SendMessage("OnTriggerEnter", _myCollider, SendMessageOptions.DontRequireReceiver);

                        if (!hitInfo.collider.isTrigger)
                        {
                            //Debug.Log($"Travelled through collider {hitInfo.collider} {hitInfo.distance} extent {_partialExtent}");
                            _body.ActiveRigidbody.position = hitInfo.point - (movementThisStep / movementMagnitude) * _partialExtent;
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

            _previousPosition = _body.ActiveRigidbody.position;
        }
    }
}