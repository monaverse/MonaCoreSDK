using UnityEngine;

namespace Mona.SDK.Core.EasyUI
{
    [System.Serializable]
    public class EasyUIVisualElement
    {
        public GameObject[] _prerequisitParents;
        public EasyUIImageSettings _primaryImageSettings;
        public EasyUIImageSettings _backgroundImageSettings;
        public EasyUIImageSettings _foregroundImageSettings;
        public EasyUITextSettings _textSettings;

        public void SetElements(EasyUICompoundSpriteDisplay compoundSpriteDisplay)
        {
            foreach (GameObject go in _prerequisitParents)
                go.SetActive(true);

            _primaryImageSettings.SetImage(compoundSpriteDisplay.PrimarySprite);
            _backgroundImageSettings.SetImage(compoundSpriteDisplay.BackgroundSprite);
            _foregroundImageSettings.SetImage(compoundSpriteDisplay.ForegroundSprite);
            _textSettings.SetText(compoundSpriteDisplay.TextDisplay);
        }

        public void SetGauge(EasyUIFillType fillType, float fillAmount)
        {
            _primaryImageSettings.SetGauge(fillType, fillAmount);
        }
    }

    [System.Serializable]
    public class EasyUIMultiIconCounter
    {
        public GameObject[] _requiredParents;
        [SerializeField] private GameObject _iconCounter;
    }

    public class EasyUIVariableDisplayElement : MonoBehaviour
    {
        [SerializeField] private bool _isPlaceholder;
        [SerializeField] private GameObject _iconCounterElementPrefab;
        [SerializeField] private GameObject _contentContainer;
        [SerializeField] private float _iconAnchorOffset = 0.2f;
        [SerializeField] private RectTransform _informationAreaRect;
        [SerializeField] private EasyUIImageSettings _elementBackground;
        [SerializeField] private EasyUIVisualElement _primaryIcon;
        [SerializeField] private EasyUITextSettings _nameDisplay;
        [SerializeField] private EasyUIVisualElement _horizontalGauge;
        [SerializeField] private EasyUIVisualElement _highVolumeCounter;
        [SerializeField] private EasyUITextSettings _numberDisplay;
        [SerializeField] private EasyUIMultiIconCounter _iconCounter;

        private IEasyUINumericalDisplay _variableReference;
        private int _priority;

        public bool IsPlaceholder => _isPlaceholder;
        public int Priority => _isPlaceholder ? 0 : _priority;
        public IEasyUINumericalDisplay VariableReference { get => _variableReference; set => _variableReference = value; }

        public void UpdateDisplay(IEasyUINumericalDisplay variable)
        {
            UpdateVisibilityDisplay(variable);

            if (_informationAreaRect)
            {
                float minX = variable.PrimaryIcon.PrimarySprite.DisplayElement ? _iconAnchorOffset : 0f;
                _informationAreaRect.anchorMin = new Vector2(minX, 0);
            }

            _elementBackground.SetImage(variable.UIBackground);
            _nameDisplay.SetText(variable.DisplayName);
            _primaryIcon.SetElements(variable.PrimaryIcon);
            _priority = variable.Priority;
            _numberDisplay.SetText(variable.NumberDisplay, variable.FormattedNumber);

            switch (variable.ValueDisplayType)
            {
                case EasyUINumericalLayoutType.GaugeFill:
                    if (variable.DisplayAsGauge)
                        UpdateGauge(variable);
                    break;
                case EasyUINumericalLayoutType.ObjectCounter:
                    break;
            }
        }

        private void UpdateGauge(IEasyUINumericalDisplay variable)
        {
            switch (variable.FillType)
            {
                case EasyUIFillType.LeftToRight:
                    _horizontalGauge.SetElements(variable.HorizontalGaugeVisual);
                    _horizontalGauge.SetGauge(variable.FillType, variable.GaugeFillAmount);
                    break;
                case EasyUIFillType.RightToLeft:
                    _horizontalGauge.SetElements(variable.HorizontalGaugeVisual);
                    _horizontalGauge.SetGauge(variable.FillType, variable.GaugeFillAmount);
                    break;
                default:
                    _primaryIcon.SetGauge(variable.FillType, variable.GaugeFillAmount);
                    break;
            }
        }

        private void UpdateVisibilityDisplay(IEasyUINumericalDisplay variable)
        {
            if (_contentContainer != null && _contentContainer.activeInHierarchy != variable.DisplayInUI)
                _contentContainer.SetActive(variable.DisplayInUI);
        }
    }
}
