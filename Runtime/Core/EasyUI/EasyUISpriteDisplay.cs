using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mona.SDK.Core.EasyUI
{
    [System.Serializable]
    public class EasyUISpriteDisplay : IEasyUICommonElementDisplay
    {
        [SerializeField] private Sprite _elementSprite;
        public Sprite ElementSprite { get => _elementSprite; set => _elementSprite = value; }

        // vvv Implementation of IEasyUIVariableDisplayCore vvv

        [SerializeField] private bool _displayElement;
        public bool DisplayElement { get => _displayElement; set => _displayElement = value; }

        [SerializeField] private EasyUIElementDisplayType _elementType;
        public EasyUIElementDisplayType ElementType { get => _elementType; set => _elementType = value; }

        [SerializeField] private Color _elementColor;
        public Color ElementColor { get => _elementColor; set => _elementColor = value; }

        [SerializeField] private EasyUIElementDisplayType _shadowType;
        public EasyUIElementDisplayType ShadowType { get => _shadowType; set => _shadowType = value; }

        [SerializeField] private Vector2 _shadowOffset;
        public Vector2 ShadowOffset { get => _shadowOffset; set => _shadowOffset = value; }
    }
}
