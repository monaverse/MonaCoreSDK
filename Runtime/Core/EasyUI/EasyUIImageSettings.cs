using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Mona.SDK.Core.EasyUI
{
    [System.Serializable]
    public class EasyUIImageSettings
    {
        [SerializeField] private GameObject[] _requiredParents;
        [SerializeField] private Image _image;
        [SerializeField] private Color _defaultColor = Color.white;
        [SerializeField] private Shadow _elementShadow;
        [SerializeField] private bool _shadowOnByDefault;
        [SerializeField] private Vector2 _defaultShadowOffset = new Vector2(-3, -3);
        [SerializeField] private bool _isObjectUI;
        private float _objectOffsetModifier = 200f;

        private bool _gaugeInitialized = false;

        private Vector2 DefaultShadowOffset => _isObjectUI ?
            _defaultShadowOffset / _objectOffsetModifier : _defaultShadowOffset;

        public void SetImage(EasyUISpriteDisplay spriteDisplay)
        {
            if (!_image || !spriteDisplay.DisplayElement)
                return;

            foreach (GameObject go in _requiredParents)
                go.SetActive(true);

            if (_elementShadow)
            {
                switch (spriteDisplay.ShadowType)
                {
                    case EasyUIElementDisplayType.None:
                        _elementShadow.enabled = false;
                        break;
                    case EasyUIElementDisplayType.Custom:
                        _elementShadow.enabled = true;
                        Vector2 customOffset = _isObjectUI ?
                            spriteDisplay.ShadowOffset / _objectOffsetModifier :
                            spriteDisplay.ShadowOffset;
                        _elementShadow.effectDistance = customOffset;
                        break;
                    default:
                        _elementShadow.enabled = _shadowOnByDefault;
                        _elementShadow.effectDistance = DefaultShadowOffset;
                        break;
                }
            }

            _image.gameObject.SetActive(true);
            _image.sprite = spriteDisplay.ElementSprite;

            switch (spriteDisplay.ElementType)
            {
                case EasyUIElementDisplayType.Custom:
                    _image.color = spriteDisplay.ElementColor;
                    break;
                default:
                    _image.color = _defaultColor;
                    break;
            }
        }

        public void SetGauge(EasyUIFillType filltype, float fillAmount)
        {
            if (!_image)
                return;

            if (_gaugeInitialized)
            {
                SetGauge(fillAmount);
                return;
            }

            _image.type = Image.Type.Filled;
            

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

            SetGauge(fillAmount);
            _gaugeInitialized = true;
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

        public void SetGauge(float fillAmount)
        {
            if (_image)
                _image.fillAmount = fillAmount;
        }
    }
}
