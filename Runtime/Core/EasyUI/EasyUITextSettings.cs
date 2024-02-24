using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Mona.SDK.Core.EasyUI
{
    [System.Serializable]
    public class EasyUITextSettings
    {
        [SerializeField] private GameObject[] _requiredParents;
        [SerializeField] private Color _defaultColor = Color.white;
        [SerializeField] private TMP_Text _tmpText;
        [SerializeField] private TMP_Text _tmpShadowText;
        [SerializeField] private bool _shadowOnByDefault;
        [SerializeField] private Vector2 _defaultShadowOffset = new Vector2(-3, -3);
        [SerializeField] private bool _isObjectUI;
        private float _objectOffsetModifier = 200f;

        private Vector2 DefaultShadowOffset => _isObjectUI ?
            _defaultShadowOffset / _objectOffsetModifier : _defaultShadowOffset;

        private bool _initialized = false;

        public void SetText(EasyUIStringDisplay stringDisplay)
        {
            SetText(stringDisplay, stringDisplay.ElementText);
        }

        public void SetText(EasyUIStringDisplay stringDisplay, string newString)
        {
            if (!stringDisplay.DisplayElement || !_tmpText)
                return;

            if (_initialized)
            {
                SetText(_tmpText, newString);
                SetText(_tmpShadowText, newString);
                return;
            }

            foreach (GameObject go in _requiredParents)
                go.SetActive(true);

            if (_tmpShadowText)
            {
                switch (stringDisplay.ShadowType)
                {
                    case EasyUIElementDisplayType.None:
                        _tmpShadowText.gameObject.SetActive(false);
                        break;
                    case EasyUIElementDisplayType.Custom:
                        _tmpShadowText.gameObject.SetActive(true);
                        Vector2 customOffset =_isObjectUI ?
                            stringDisplay.ShadowOffset / _objectOffsetModifier :
                            stringDisplay.ShadowOffset;

                        _tmpShadowText.rectTransform.offsetMin = customOffset;
                        _tmpShadowText.rectTransform.offsetMax = customOffset;
                        break;
                    default:
                        _tmpShadowText.gameObject.SetActive(_shadowOnByDefault);
                        _tmpShadowText.rectTransform.offsetMin = DefaultShadowOffset;
                        _tmpShadowText.rectTransform.offsetMax = DefaultShadowOffset;
                        break;
                }
            }

            _tmpText.gameObject.SetActive(true);

            SetText(_tmpText, newString);
            SetText(_tmpShadowText, newString);

            switch (stringDisplay.ElementType)
            {
                case EasyUIElementDisplayType.Custom:
                    _tmpText.color = stringDisplay.ElementColor;
                    break;
                default:
                    _tmpText.color = _defaultColor;
                    break;
            }

            if (stringDisplay.FontType == EasyUIElementDisplayDefaultOrCustom.Custom)
            {
                SetFont(_tmpText, stringDisplay.ElementFont);
                SetFont(_tmpShadowText, stringDisplay.ElementFont);
            }

            AlignText(_tmpText, stringDisplay.TextAlignment);
            AlignText(_tmpShadowText, stringDisplay.TextAlignment);

            _initialized = true;
        }

        private void SetFont(TMP_Text tmpElement, TMP_FontAsset font)
        {
            if (!tmpElement)
                return;

            tmpElement.font = font;
        }

        private void AlignText(TMP_Text tmpElement, EasyUITextAlignment alignment)
        {
            if (!tmpElement)
                return;

            switch (alignment)
            {
                case EasyUITextAlignment.Left:
                    tmpElement.alignment = TextAlignmentOptions.Left;
                    break;
                case EasyUITextAlignment.Center:
                    tmpElement.alignment = TextAlignmentOptions.Center;
                    break;
                case EasyUITextAlignment.Right:
                    tmpElement.alignment = TextAlignmentOptions.Right;
                    break;
            }
        }

        private void SetText(TMP_Text tmpElement, string newString)
        {
            if (!tmpElement)
                return;

            tmpElement.text = newString;
        }
    }
}
