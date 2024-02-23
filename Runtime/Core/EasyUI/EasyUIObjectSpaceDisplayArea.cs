using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mona.SDK.Core.EasyUI
{
    [System.Serializable]
    public class EasyUIObjectSpaceDisplayArea
    {
        [SerializeField] private string areaName;
        [SerializeField] private EasyUIObjectPosition _uiPosition;
        [SerializeField] private RectTransform _positionalRect;
        [SerializeField] private RectTransform _scaleRect;
        [SerializeField] private EasyUIScreenZone _screenZone;

        public EasyUIObjectPosition UIPosition => _uiPosition;
        public EasyUIScreenZone ScreenZone => _screenZone;

        public void SetOffset(float offset)
        {
            if (!_positionalRect)
                return;

            float placementModifier = _uiPosition == EasyUIObjectPosition.Above ? 1f : -1f;
            offset *= placementModifier;
            _positionalRect.localPosition = new Vector3(0, offset, 0);
        }

        public void SetScale(float scaleModifier)
        {
            if (!_scaleRect)
                return;

            _scaleRect.localScale = Vector3.one * scaleModifier;
        }
    }
}
