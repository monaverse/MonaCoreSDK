using System;
using UnityEngine;
using Mona.SDK.Core.EasyUI;

namespace Mona.SDK.Core.State.Structs
{
    [Serializable]
    public class MonaVariablesFloat : IMonaVariablesValue, IMonaVariablesFloatValue, IEasyUINumericalDisplay
    {
        public event Action OnChange = delegate { };

        public void Change() => OnChange();

        // ------ Base Values ------

        [SerializeField] private string _name;
        [SerializeField] public float _value = 1f;
        [SerializeField] private float _defaultValue;
        [SerializeField] private MinMaxConstraintType _minMaxType = MinMaxConstraintType.None;
        [SerializeField] private float _min = 0f;
        [SerializeField] private float _max = 10f;

        public string Name { get => _name; set => _name = value; }
        public float DefaultValue { get => _defaultValue; set => _defaultValue = value; }
        public bool UseMinMax { get => _minMaxType != MinMaxConstraintType.None; }
        public MinMaxConstraintType MinMaxType { get => _minMaxType; set => _minMaxType = value; }
        private float MinMaxRange => _max - _min;

        public float Value
        {
            get { return _value; }
            set
            {
                if (!UseMinMax)
                {
                    _value = value;
                    return;
                }

                switch (_minMaxType)
                {
                    case MinMaxConstraintType.ConstrainToBounds:
                        _value = Mathf.Clamp(value, _min, _max);
                        break;
                    case MinMaxConstraintType.Loop:
                        _value = _min + (value - _min) % MinMaxRange;
                        break;
                    case MinMaxConstraintType.Bounce:
                        float adjustedValue = (value - _min) % (MinMaxRange * 2);
                        _value = _min + (adjustedValue <= MinMaxRange ? adjustedValue : MinMaxRange * 2 - adjustedValue);
                        break;
                    case MinMaxConstraintType.ReturnToDefault:
                        _value = _defaultValue;
                        break;
                    default:
                        _value = value;
                        break;
                }
            }
        }

        public float Min
        {
            get { return _min; }
            set
            {
                if (value <= _max)
                {
                    _min = value;
                }
                else
                {
                    Debug.LogWarning(string.Format("WARNING: Setting minimum range for value '{0}' failed as it exceeds the maximum range of '{1}'!", _name, _max.ToString()));
                }

                if (UseMinMax && _value < _min)
                    _value = _min;
            }
        }

        public float Max
        {
            get { return _max; }
            set
            {
                if (value >= _min)
                {
                    _max = value;
                }
                else
                {
                    Debug.LogWarning(string.Format("WARNING: Setting maximum range for value '{0}' failed as it is less than the minimum range of '{1}'!", _name, _max.ToString()));
                }

                if (UseMinMax && _value > _max)
                    _value = _max;
            }
        }

        // ------ UI Related Values ------ //

        // General Display and UI Placement

        [SerializeField] private bool _allowUIDisplay = false;
        [SerializeField] private bool _displayInUI = false;
        [SerializeField] private EasyUIDisplaySpace _displaySpace = EasyUIDisplaySpace.OnObject;
        [SerializeField] private EasyUIScreenPosition _screenPosition = EasyUIScreenPosition.TopLeft;
        [SerializeField] private EasyUIObjectPosition _objectPosition = EasyUIObjectPosition.Above;
        [SerializeField] private int _priority = 10;

        public bool AllowUIDisplay { get => _allowUIDisplay; set => _allowUIDisplay = value; }
        public bool DisplayInUI { get => _displayInUI; set => _displayInUI = value; }
        public EasyUIDisplaySpace DisplaySpace { get => _displaySpace; set => _displaySpace = value; }
        public EasyUIScreenPosition ScreenPosition { get => _screenPosition; set => _screenPosition = value; }
        public EasyUIObjectPosition ObjectPosition { get => _objectPosition; set => _objectPosition = value; }
        public int Priority { get => _priority; set => _priority = value; }

        // Display Element definitions

        [SerializeField] private EasyUIStringDisplay _displayName = new EasyUIStringDisplay();
        [SerializeField] private EasyUIStringDisplay _tooltip = new EasyUIStringDisplay();
        [SerializeField] private EasyUICompoundSpriteDisplay _primaryIcon = new EasyUICompoundSpriteDisplay();
        [SerializeField] private EasyUISpriteDisplay _uiBackground = new EasyUISpriteDisplay();
        [SerializeField] private EasyUINumericalLayoutType _valueDisplayType = EasyUINumericalLayoutType.GaugeFill;
        [SerializeField] private EasyUIFillType _fillType = EasyUIFillType.LeftToRight;
        [SerializeField] private EasyUICompoundSpriteDisplay _horizontalGaugeVisual = new EasyUICompoundSpriteDisplay();
        [SerializeField] private EasyUIStringDisplay _numberDisplay = new EasyUIStringDisplay();
        [SerializeField] private string _numberPrefix;
        [SerializeField] private string _numberSuffix;

        public EasyUIStringDisplay DisplayName { get => _displayName; set => _displayName = value; }
        public EasyUIStringDisplay Tooltip { get => _tooltip; set => _tooltip = value; }
        public EasyUICompoundSpriteDisplay PrimaryIcon { get => _primaryIcon; set => _primaryIcon = value; }
        public EasyUISpriteDisplay UIBackground { get => _uiBackground; set => _uiBackground = value; }
        public EasyUINumericalLayoutType ValueDisplayType { get => _valueDisplayType; set => _valueDisplayType = value; }
        public EasyUIFillType FillType { get => _fillType; set => _fillType = value; }
        public EasyUICompoundSpriteDisplay HorizontalGaugeVisual { get => _horizontalGaugeVisual; set => _horizontalGaugeVisual = value; }
        public EasyUIStringDisplay NumberDisplay { get => _numberDisplay; set => _numberDisplay = value; }
        public string NumberPrefix { get => _numberPrefix; set => _numberPrefix = value; }
        public string NumberSuffix { get => _numberSuffix; set => _numberSuffix = value; }
        public bool DisplayAsGauge => ValueDisplayType == EasyUINumericalLayoutType.GaugeFill;
        public bool UseHorizontalGauge => DisplayAsGauge && (FillType == EasyUIFillType.LeftToRight || FillType == EasyUIFillType.RightToLeft);

        // Number formatting

        [SerializeField] private EasyUINumericalBaseFormatType _numberFormatType = EasyUINumericalBaseFormatType.Default;
        [SerializeField] private EasyUINumericalSeparatorType _thousandthPlaceSepartorType = EasyUINumericalSeparatorType.Default;
        [SerializeField] private EasyUINumericalSeparatorType _decimalPlaceSepartorType = EasyUINumericalSeparatorType.Default;

        public EasyUINumericalBaseFormatType NumberFormatType { get => _numberFormatType; set => _numberFormatType = value; }
        public EasyUINumericalSeparatorType ThousandthPlaceSepartorType { get => _thousandthPlaceSepartorType; set => _thousandthPlaceSepartorType = value; }
        public EasyUINumericalSeparatorType DecimalPlaceSepartorType { get => _decimalPlaceSepartorType; set => _decimalPlaceSepartorType = value; }
        // Needs full implementation of formatting
        public string FormattedNumber { get => _numberPrefix + _value.ToString() + _numberSuffix; }

        public string ThousandthPlaceSeparator
        {
            get
            {
                switch (_thousandthPlaceSepartorType) // Need to implement default (regional detection)
                {
                    case EasyUINumericalSeparatorType.None:
                        return string.Empty;
                    case EasyUINumericalSeparatorType.UseSpaces:
                        return " ";
                    case EasyUINumericalSeparatorType.UsePeriods:
                        return ".";
                    default:
                        return ",";
                }
            }
        }
        
        public string DecimalPlaceSeparator
        {
            get
            {
                switch (_decimalPlaceSepartorType) // Need to implement default (regional detection)
                {
                    case EasyUINumericalSeparatorType.None:
                        return string.Empty;
                    case EasyUINumericalSeparatorType.UseSpaces:
                        return " ";
                    case EasyUINumericalSeparatorType.UsePeriods:
                        return ".";
                    default:
                        return ",";
                }
            }
        }

        // Animation Settings

        [SerializeField] private EasyUIPulseType _pulseType = EasyUIPulseType.None;
        [SerializeField] private float _pulseStartValue = 0.25f;
        [SerializeField] private float _pulseFrequency = 0.5f;

        public EasyUIPulseType PulseType { get => _pulseType; set => _pulseType = value; }
        public float PulseStartValue { get => _pulseStartValue; set => _pulseStartValue = value; }
        public float PulseFrequency { get => _pulseFrequency; set => _pulseFrequency = value; }

        public MonaVariablesFloat() { }
    }
}