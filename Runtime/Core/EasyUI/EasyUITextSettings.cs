using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Mona.SDK.Core.EasyUI
{
    [System.Serializable]
    public class EasyUITextSettings
    {
        [SerializeField] private GameObject[] _requiredParents;
        [SerializeField] private Text _textArea;
        [SerializeField] private Color _defaultColor = Color.white;
        [SerializeField] private Shadow _elementShadow;
        [SerializeField] private bool _shadowOnByDefault;
        [SerializeField] private Vector2 _defaultShadowOffset = new Vector2(-3, -3);

        private bool _initialized = false;

        public void SetText(EasyUIStringDisplay stringDisplay)
        {
            SetText(stringDisplay, stringDisplay.ElementText);
        }

        public void SetText(EasyUIStringDisplay stringDisplay, string newString)
        {
            if (!stringDisplay.DisplayElement || !_textArea)
                return;

            if (_initialized)
            {
                SetText(newString);
                return;
            }

            foreach (GameObject go in _requiredParents)
                go.SetActive(true);

            if (_elementShadow)
            {
                switch (stringDisplay.ShadowType)
                {
                    case EasyUIElementDisplayType.None:
                        _elementShadow.enabled = false;
                        break;
                    case EasyUIElementDisplayType.Custom:
                        _elementShadow.enabled = true;
                        _elementShadow.effectDistance = stringDisplay.ShadowOffset;
                        break;
                    default:
                        _elementShadow.enabled = _shadowOnByDefault;
                        _elementShadow.effectDistance = _defaultShadowOffset;
                        break;
                }
            }

            _textArea.gameObject.SetActive(true);
            SetText(newString);

            switch (stringDisplay.ElementType)
            {
                case EasyUIElementDisplayType.Custom:
                    _textArea.color = stringDisplay.ElementColor;
                    break;
                default:
                    _textArea.color = _defaultColor;
                    break;
            }

            switch (stringDisplay.FontType)
            {
                case EasyUIElementDisplayDefaultOrCustom.Custom:
                    _textArea.font = stringDisplay.ElementFont;
                    break;
            }

            switch (stringDisplay.TextAlignment)
            {
                case EasyUITextAlignment.Left:
                    _textArea.alignment = TextAnchor.MiddleLeft;
                    break;
                case EasyUITextAlignment.Center:
                    _textArea.alignment = TextAnchor.MiddleCenter;
                    break;
                case EasyUITextAlignment.Right:
                    _textArea.alignment = TextAnchor.MiddleRight;
                    break;
            }

            _initialized = true;
        }

        private void SetText(string newString)
        {
            _textArea.text = newString;
        }
    }
}
