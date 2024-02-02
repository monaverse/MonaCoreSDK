using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Mona.SDK.Core.EasyUI
{
    [System.Serializable]
    public class EasyUIImageSettings
    {
        public bool _useImage;
        public Image _image;
        public bool _useShadow;
        public Shadow _elementShadow;
        public GameObject[] _requiredParents;

        private Vector2 defaultShadowOffset = new Vector2(-3, -3);

        public void InitializeImage(Sprite sprite, Color color)
        {
            InitializeImage(sprite, color, defaultShadowOffset);
        }

        public void InitializeImage(Sprite sprite, Color color, Vector2 shadowOffset)
        {
            if (!_useImage || !_image)
                return;

            foreach (GameObject go in _requiredParents)
                go.SetActive(true);

            if (_useShadow && _elementShadow)
            {
                _elementShadow.gameObject.SetActive(_useShadow);
                _elementShadow.effectDistance = shadowOffset;
            }

            _image.gameObject.SetActive(true);
            SetSprite(sprite);
            SetColor(color);
        }

        public void InitializeGauge(EasyUIFillType filltype, float fillAmount)
        {
            if (!_useImage || !_image)
                return;

            _image.type = Image.Type.Filled;
            SetGaugeFill(fillAmount);

            switch (filltype)
            {
                case EasyUIFillType.LeftToRight:
                    _image.fillMethod = Image.FillMethod.Horizontal;
                    _image.fillOrigin = (int)Image.OriginHorizontal.Left;
                    break;
                case EasyUIFillType.RightToLeft:
                    _image.fillMethod = Image.FillMethod.Horizontal;
                    _image.fillOrigin = (int)Image.OriginHorizontal.Right;
                    break;
                case EasyUIFillType.BottomUp:
                    _image.fillMethod = Image.FillMethod.Vertical;
                    _image.fillOrigin = (int)Image.OriginVertical.Bottom;
                    break;
                case EasyUIFillType.TopDown:
                    _image.fillMethod = Image.FillMethod.Vertical;
                    _image.fillOrigin = (int)Image.OriginVertical.Top;
                    break;
                case EasyUIFillType.Radial:
                    _image.fillMethod = Image.FillMethod.Radial360;
                    _image.fillOrigin = (int)Image.Origin360.Top;
                    break;
            }
        }

        public void SetSprite(Sprite sprite)
        {
            if (_image)
                _image.sprite = sprite;
        }

        public void SetColor(Color color)
        {
            if (_image)
                _image.color = color;
        }

        public void SetGaugeFill(float fillAmount)
        {
            if (_image)
                _image.fillAmount = fillAmount;
        }
    }
}
