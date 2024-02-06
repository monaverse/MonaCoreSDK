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


        [SerializeField] private string _name;
        public string Name { get => _name; set => _name = value; }

        [SerializeField] public float _value = 1f;

        public float Value
        {
            get { return _value; }
            set
            {
                if (!_useMinMax)
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

        [SerializeField] private float _defaultValue;
        public float DefaultValue { get => _defaultValue; set => _defaultValue = value; }

        [SerializeField] private bool _useMinMax = false;
        public bool UseMinMax { get => _useMinMax; set => _useMinMax = value; }

        [SerializeField] private MinMaxConstraintType _minMaxType;
        public MinMaxConstraintType MinMaxType { get => _minMaxType; set => _minMaxType = value; }

        [SerializeField] private float _min = 0f;
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

                if (_useMinMax && _value < _min)
                    _value = _min;
            }
        }

        [SerializeField] private float _max = 10f;
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

                if (_useMinMax && _value > _max)
                    _value = _max;
            }
        }

        private float MinMaxRange => _max - _min;

        [SerializeField] private bool _allowUIDisplay = false;
        public bool AllowUIDisplay { get => _allowUIDisplay; set => _allowUIDisplay = value; }

        [SerializeField] private bool _displayInUI = false;
        public bool DisplayInUI { get => _displayInUI; set => _displayInUI = value; }

        [SerializeField] private EasyUIDisplaySpace _displaySpace = EasyUIDisplaySpace.OnObject;
        public EasyUIDisplaySpace DisplaySpace { get => _displaySpace; set => _displaySpace = value; }
        [SerializeField] private EasyUIScreenPosition _screenPosition = EasyUIScreenPosition.TopLeft;
        public EasyUIScreenPosition ScreenPosition { get => _screenPosition; set => _screenPosition = value; }
        [SerializeField] private EasyUIObjectPosition _objectPosition = EasyUIObjectPosition.Above;
        public EasyUIObjectPosition ObjectPosition { get => _objectPosition; set => _objectPosition = value; }

        [SerializeField] private int _priority = 10;
        public int Priority { get => _priority; set => _priority = value; }


        [SerializeField] private EasyUIStringDisplay _displayName;
        public EasyUIStringDisplay DisplayName { get => _displayName; set => _displayName = value; }

        [SerializeField] private EasyUIStringDisplay _tooltip;
        public EasyUIStringDisplay Tooltip { get => _tooltip; set => _tooltip = value; }

        [SerializeField] private EasyUICompoundSpriteDisplay _primaryIcon;
        public EasyUICompoundSpriteDisplay PrimaryIcon { get => _primaryIcon; set => _primaryIcon = value; }

        [SerializeField] private EasyUISpriteDisplay _uiBackground;
        public EasyUISpriteDisplay UIBackground { get => _uiBackground; set => _uiBackground = value; }

        [SerializeField] private EasyUINumericalLayoutType _valueDisplayType = EasyUINumericalLayoutType.HorizontalGauge;
        public EasyUINumericalLayoutType ValueDisplayType { get => _valueDisplayType; set => _valueDisplayType = value; }

        [SerializeField] private EasyUIFillType _fillType;
        public EasyUIFillType FillType { get => _fillType; set => _fillType = value; }

        [SerializeField] private EasyUICompoundSpriteDisplay _horizontalGaugeVisual;
        public EasyUICompoundSpriteDisplay HorizontalGaugeVisual { get => _horizontalGaugeVisual; set => _horizontalGaugeVisual = value; }

        [SerializeField] private EasyUIStringDisplay _numberDisplay;
        public EasyUIStringDisplay NumberDisplay { get => _numberDisplay; set => _numberDisplay = value; }

        [SerializeField] private string _numberPrefix;
        public string NumberPrefix { get => _numberPrefix; set => _numberPrefix = value; }

        [SerializeField] private string _numberSuffix;
        public string NumberSuffix { get => _numberSuffix; set => _numberSuffix = value; }


        [SerializeField] private EasyUINumericalBaseFormatType _numberFormatType = EasyUINumericalBaseFormatType.Default;
        public EasyUINumericalBaseFormatType NumberFormatType { get => _numberFormatType; set => _numberFormatType = value; }
        [SerializeField] private EasyUINumericalSeparatorType _thousandthPlaceSepartorType = EasyUINumericalSeparatorType.Default;
        public EasyUINumericalSeparatorType ThousandthPlaceSepartorType { get => _thousandthPlaceSepartorType; set => _thousandthPlaceSepartorType = value; }
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
        [SerializeField] private EasyUINumericalSeparatorType _decimalPlaceSepartorType = EasyUINumericalSeparatorType.Default;
        public EasyUINumericalSeparatorType DecimalPlaceSepartorType { get => _decimalPlaceSepartorType; set => _decimalPlaceSepartorType = value; }
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

        // Needs full implementation of formatting
        public string FormattedNumber { get => _numberPrefix + _value.ToString() + _numberSuffix; }

        [SerializeField] private EasyUIPulseType _pulseType = EasyUIPulseType.None;
        public EasyUIPulseType PulseType { get => _pulseType; set => _pulseType = value; }
        [SerializeField] private float _pulseStartValue = 0.25f;
        public float PulseStartValue { get => _pulseStartValue; set => _pulseStartValue = value; }
        [SerializeField] private float _pulseFrequency = 0.5f;
        public float PulseFrequency { get => _pulseFrequency; set => _pulseFrequency = value; }

        public MonaVariablesFloat() { }
    }
}