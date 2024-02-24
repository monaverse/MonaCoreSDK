using UnityEngine;
using TMPro;

namespace Mona.SDK.Core.EasyUI
{
    [System.Serializable]
    public class EasyUIStringDisplay : IEasyUICommonElementDisplay
    {
        [SerializeField] private string _elementText;
        public string ElementText { get => _elementText; set => _elementText = value; }

        [SerializeField] private EasyUIElementDisplayDefaultOrCustom _fontType = EasyUIElementDisplayDefaultOrCustom.Default;
        public EasyUIElementDisplayDefaultOrCustom FontType { get => _fontType; set => _fontType = value; }

        [SerializeField] private TMP_FontAsset _elementFont;
        public TMP_FontAsset ElementFont { get => _elementFont; set => _elementFont = value; }

        [SerializeField] EasyUITextAlignment _textAlignment = EasyUITextAlignment.Default;
        public EasyUITextAlignment TextAlignment { get => _textAlignment; set => _textAlignment = value; }

        // vvv Implementation of IEasyUIVariableDisplayCore vvv

        [SerializeField] private bool _displayElement;
        public bool DisplayElement { get => _displayElement; set => _displayElement = value; }

        [SerializeField] private EasyUIElementDisplayType _elementType = EasyUIElementDisplayType.Default;
        public EasyUIElementDisplayType ElementType { get => _elementType; set => _elementType = value; }

        [SerializeField] private Color _elementColor;
        public Color ElementColor { get => _elementColor; set => _elementColor = value; }

        [SerializeField] private EasyUIElementDisplayType _shadowType = EasyUIElementDisplayType.Default;
        public EasyUIElementDisplayType ShadowType { get => _shadowType; set => _shadowType = value; }

        [SerializeField] private Vector2 _shadowOffset;
        public Vector2 ShadowOffset { get => _shadowOffset; set => _shadowOffset = value; }
    }
}
