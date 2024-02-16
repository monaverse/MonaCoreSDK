using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mona.SDK.Core.EasyUI
{
    [System.Serializable]
    public class EasyUICompoundSpriteDisplay
    {
        [SerializeField] private EasyUISpriteDisplay _primarySprite = new EasyUISpriteDisplay();
        [SerializeField] private EasyUISpriteDisplay _backgroundSprite = new EasyUISpriteDisplay();
        [SerializeField] private EasyUISpriteDisplay _foregroundSprite = new EasyUISpriteDisplay();
        [SerializeField] private EasyUIStringDisplay _textDisplay = new EasyUIStringDisplay();

        public EasyUISpriteDisplay PrimarySprite { get => _primarySprite; set => _primarySprite = value; }
        public EasyUISpriteDisplay BackgroundSprite { get => _backgroundSprite; set => _backgroundSprite = value; }
        public EasyUISpriteDisplay ForegroundSprite { get => _foregroundSprite; set => _foregroundSprite = value; }
        public EasyUIStringDisplay TextDisplay { get => _textDisplay; set => _textDisplay = value; }
    }
}
