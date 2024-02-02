using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Mona.SDK.Core.EasyUI
{
    [System.Serializable]
    public class EasyUITextSettings
    {
        public bool _useText;
        public Text _textArea;
        public bool _useShadow;
        public Shadow _elementShadow;
        public GameObject[] _requiredParents;

        private Vector2 defaultShadowOffset = new Vector2(-3, -3);

        public void InitializeText(string textString, Color color, Font font, EasyUITextAlignment alignment)
        {
            InitializeText(textString, color, font, alignment, defaultShadowOffset);
        }

        public void InitializeText(string textString, Color color, Font font, EasyUITextAlignment alignment, Vector2 shadowOffset)
        {
            if (!_useText || !_textArea)
                return;

            foreach (GameObject go in _requiredParents)
                go.SetActive(true);

            if (_useShadow && _elementShadow)
            {
                _elementShadow.gameObject.SetActive(_useShadow);
                _elementShadow.effectDistance = shadowOffset;
            }

            _textArea.gameObject.SetActive(true);
            _textArea.text = textString;
            _textArea.color = color;
            _textArea.font = font;

            switch (alignment)
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
        }

        public void SetText(string newString)
        {
            if (!_useText || !_textArea)
                return;

            _textArea.text = newString;
        }
    }
}
