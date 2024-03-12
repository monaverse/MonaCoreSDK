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

        public bool RestrictTransform => x.BindAxis || y.BindAxis || z.BindAxis;

        public Vector3 BindValue(Vector3 transformValue)
        {
            if (!RestrictTransform)
                return transformValue;

            transformValue.x = x.RestrictAxis(transformValue.x);
            transformValue.y = y.RestrictAxis(transformValue.y);
            transformValue.z = z.RestrictAxis(transformValue.z);

            return transformValue;
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

        public float RestrictAxis(float axisValue)
        {
            if (!_bindAxis)
                return axisValue;

            if (axisValue < Min)
                return Min;

            if (axisValue > Max)
                return Max;

            return axisValue;
        }
    }
}
