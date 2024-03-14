using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mona.SDK.Core.Body
{
    [System.Serializable]
    public class MonaBodyTransformBounds
    {
        public TransformBoundsAxis x = new TransformBoundsAxis();
        public TransformBoundsAxis y = new TransformBoundsAxis();
        public TransformBoundsAxis z = new TransformBoundsAxis();
        public TransformBoundsRadius radius = new TransformBoundsRadius();

        public bool RestrictTransform => x.BindAxis || y.BindAxis || z.BindAxis || radius.BindRadius;

        public Vector3 BindValue(Vector3 transformValue)
        {
            if (!RestrictTransform)
                return transformValue;

            transformValue = radius.RestrictPosition(transformValue);
            transformValue.x = x.RestrictAxis(transformValue.x, false);
            transformValue.y = y.RestrictAxis(transformValue.y, false);
            transformValue.z = z.RestrictAxis(transformValue.z, false);

            return transformValue;
        }

        public Quaternion BindValue(Quaternion transformValue, Transform transform)
        {
            if (!RestrictTransform)
                return transformValue;

            Quaternion localRotation = transform.parent == null ?
                transformValue :
                Quaternion.Inverse(transform.parent.rotation) * transformValue;

            Vector3 localRotationEulers = localRotation.eulerAngles;
            localRotationEulers.x = x.RestrictAxis(localRotationEulers.x, true);
            localRotationEulers.y = y.RestrictAxis(localRotationEulers.y, true);
            localRotationEulers.z = z.RestrictAxis(localRotationEulers.z, true);

            Quaternion boundLocalRotation = Quaternion.Euler(localRotationEulers);

            Quaternion boundGlobalRotation = transform.parent == null ?
                boundLocalRotation : transform.parent.rotation * boundLocalRotation;


            return boundGlobalRotation;
        }
    }

    [System.Serializable]
    public class TransformBoundsRadius
    {
        [SerializeField] private bool _bindRadius;
        [SerializeField] private Vector3 _originPosition;
        [SerializeField] private float _radius;

        public bool BindRadius { get => _bindRadius; set => _bindRadius = value; }
        public Vector3 OriginPosition { get => _originPosition; set => _originPosition = value; }
        public float Radius { get => _radius; set => _radius = value; }

        public void Bind(float radius, Vector3 origin)
        {
            _bindRadius = true;
            _radius = radius;
            _originPosition = origin;
        }

        public void UnBind()
        {
            _bindRadius = false;
        }

        public Vector3 RestrictPosition(Vector3 newPosition)
        {
            if (!_bindRadius)
                return newPosition;

            Vector3 direction = newPosition - _originPosition;

            if (direction.magnitude > _radius)
            {
                direction = direction.normalized * _radius;
                newPosition = _originPosition + direction;
            }

            return newPosition;
        }
    }

    [System.Serializable]
    public class TransformBoundsAxis
    {
        [SerializeField] private bool _bindAxis;
        [SerializeField] private float _min = -10f;
        [SerializeField] private float _max = 10f;

        public bool BindAxis { get => _bindAxis; set => _bindAxis = value; }

        public float Min
        {
            get => _min < _max ? _min : _max;
            set => _min = value;
        }

        public float Max
        {
            get => _max > _min ? _max : _min;
            set => _max = value;
        }

        public void Bind(float min, float max)
        {
            _bindAxis = true;
            _min = min;
            _max = max;
        }

        public void UnBind()
        {
            _bindAxis = false;
        }

        public float RestrictAxis(float axisValue, bool isAngle)
        {
            if (isAngle)
                return _bindAxis ? Mathf.Clamp(GetAdjustedAngle(axisValue), Min, Max) : axisValue;

            return _bindAxis ? Mathf.Clamp(axisValue, Min, Max) : axisValue;
        }

        private float GetAdjustedAngle(float angle)
        {
            return angle % 360f;
            //angle = angle % 360;
            //
            //if (angle < -180)
            //{
            //    angle += 360;
            //}
            //if (angle > 180)
            //{
            //    angle -= 360;
            //}
            //return angle;
        }
    }
}
