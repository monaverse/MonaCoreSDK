using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mona.SDK.Core.Body
{
    public class GroundingObject
    {
        public Transform transform = null;
        private Rigidbody _rigidbody = null;
        private IMonaBody _monaBody = null;
        private IMonaBody _riderMonaBody = null;

        private bool _initialized = false;
        private bool _onlyHasTransform = false;
        private Vector3 _previousPosition;

        public IMonaBody MonaBody => _monaBody;

        public bool TrackingObject => _initialized;

        public void Initialize(Transform objectTransform, IMonaBody riderMonaBody)
        {
            transform = objectTransform;
            _riderMonaBody = riderMonaBody;
            _previousPosition = transform.position;
            _rigidbody = transform.GetComponent<Rigidbody>();

            bool disregardRigidBody = _rigidbody == null || _rigidbody.isKinematic;

            _monaBody = disregardRigidBody ? transform.GetComponent<IMonaBody>() : null;

            _onlyHasTransform = disregardRigidBody && _monaBody == null;
            _initialized = true;
        }

        private Vector3 Velocity
        {
            get
            {
                if (!_onlyHasTransform)
                    return _monaBody != null ? _monaBody.CurrentVelocity : _rigidbody.velocity;
                
                Vector3 currentPosition = transform.position;
                Vector3 updatedVelocity = (currentPosition - _previousPosition) / Time.fixedDeltaTime;
                _previousPosition = currentPosition;

                return updatedVelocity;
            }
        }

        public Vector3 AdjustedRiderVelocity
        {
            get
            {
                Vector3 myVelocity = Velocity;
                
                if (Mathf.Approximately(myVelocity.magnitude, 0))
                    return Vector3.zero;

                Vector3 riderVelocity = _riderMonaBody.CurrentVelocity;

                float verticalVelocity = riderVelocity.y - 0.5f > myVelocity.y ?
                    myVelocity.y : myVelocity.y;

                return new Vector3(myVelocity.x, verticalVelocity, myVelocity.z);
            }
        }
    }
}
