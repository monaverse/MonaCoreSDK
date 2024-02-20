using System;
using UnityEngine;
using Mona.SDK.Core.EasyUI;
using System.Globalization;

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
                    UpdateUIDisplay();
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

                UpdateUIDisplay();
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

                UpdateUIDisplay();
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

                UpdateUIDisplay();
            }
        }

        // ------ UI Related Values ------ //

        // General Display and UI Placement

        private EasyUIVariableDisplayElement _displayElementReference = null;
        public EasyUIVariableDisplayElement DisplayElementReference { get => _displayElementReference; set => _displayElementReference = value; }

        [SerializeField] private bool _allowUIDisplay = false;
        [SerializeField] private bool _displayInUI = false;
        [SerializeField] private EasyUIDisplaySpace _displaySpace = EasyUIDisplaySpace.OnObject;
        [SerializeField] private EasyUIScreenPosition _screenPosition = EasyUIScreenPosition.TopLeft;
        [SerializeField] private EasyUIObjectPosition _objectPosition = EasyUIObjectPosition.Above;
        [SerializeField] private int _priority = 10;

        public bool AllowUIDisplay { get => _allowUIDisplay; set => _allowUIDisplay = value; }
        public bool DisplayInUI
        {
            get => _displayInUI;
            set
            {
                _displayInUI = value;
                UpdateUIDisplay();
            }
        }
        public EasyUIDisplaySpace DisplaySpace { get => _displaySpace; set => _displaySpace = value; }
        public EasyUIScreenPosition ScreenPosition { get => _screenPosition; set => _screenPosition = value; }
        public EasyUIObjectPosition ObjectPosition { get => _objectPosition; set => _objectPosition = value; }
        public int Priority
        {
            get => _priority;
            set
            {
                _priority = value;
                UpdateUIDisplay();
            }
        }

        // Display Element definitions

        [SerializeField] private EasyUIStringDisplay _displayName = new EasyUIStringDisplay();
        [SerializeField] private EasyUIStringDisplay _tooltip = new EasyUIStringDisplay();
        [SerializeField] private EasyUICompoundSpriteDisplay _primaryIcon = new EasyUICompoundSpriteDisplay();
        [SerializeField] private EasyUISpriteDisplay _uiBackground = new EasyUISpriteDisplay();
        [SerializeField] private EasyUINumericalLayoutType _valueDisplayType = EasyUINumericalLayoutType.GaugeFill;
        [SerializeField] private EasyUIFillType _fillType = EasyUIFillType.LeftToRight;
        [SerializeField] private EasyUICompoundSpriteDisplay _horizontalGaugeVisual = new EasyUICompoundSpriteDisplay();
        [SerializeField] private EasyUIStringDisplay _numberDisplay = new EasyUIStringDisplay();
        [SerializeField] private MinMaxNumericalFormatting _minMaxFormatting = MinMaxNumericalFormatting.None;
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
        public MinMaxNumericalFormatting MinMaxFormatting { get => _minMaxFormatting; set => _minMaxFormatting = value; }
        public string NumberPrefix { get => _numberPrefix; set => _numberPrefix = value; }
        public string NumberSuffix { get => _numberSuffix; set => _numberSuffix = value; }
        public bool DisplayAsGauge => ValueDisplayType == EasyUINumericalLayoutType.GaugeFill;
        public bool UseHorizontalGauge => DisplayAsGauge && (FillType == EasyUIFillType.LeftToRight || FillType == EasyUIFillType.RightToLeft);

        public float GaugeFillAmount
        {
            get
            {
                if (!UseMinMax || _max - _min == 0f)
                    return 0f;

                return (_value - _min) / (_max - _min);

            }
        }

        // Number formatting

        [SerializeField] private EasyUINumericalBaseFormatType _numberFormatType = EasyUINumericalBaseFormatType.Default;
        [SerializeField] private EasyUINumericalSeparatorType _thousandthPlaceSepartorType = EasyUINumericalSeparatorType.Default;
        [SerializeField] private EasyUINumericalSeparatorType _decimalPlaceSepartorType = EasyUINumericalSeparatorType.Default;

        public EasyUINumericalBaseFormatType NumberFormatType { get => _numberFormatType; set => _numberFormatType = value; }
        public EasyUINumericalSeparatorType ThousandthPlaceSepartorType { get => _thousandthPlaceSepartorType; set => _thousandthPlaceSepartorType = value; }
        public EasyUINumericalSeparatorType DecimalPlaceSepartorType { get => _decimalPlaceSepartorType; set => _decimalPlaceSepartorType = value; }
        // Needs full implementation of formatting
        public string FormattedNumber
        {
            get
            {
                string min = string.Empty;
                string max = string.Empty;

                if (UseMinMax)
                {
                    min = _minMaxFormatting == MinMaxNumericalFormatting.ShowMin || _minMaxFormatting == MinMaxNumericalFormatting.ShowMinAndMax ?
                        _numberPrefix + FormatNumber(_min) + _numberSuffix + " / " : string.Empty;
                    max = _minMaxFormatting == MinMaxNumericalFormatting.ShowMax || _minMaxFormatting == MinMaxNumericalFormatting.ShowMinAndMax ?
                        " / " + _numberPrefix + FormatNumber(_max) + NumberSuffix : string.Empty;                    
                }

                return min + _numberPrefix + FormatNumber(_value) + _numberSuffix + max;
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

        public string FormatNumber(float numberToFormat)
        {
            CultureInfo cultureInfo = CultureInfo.CurrentCulture;
            NumberFormatInfo numberFormatInfo = (NumberFormatInfo)cultureInfo.NumberFormat.Clone();

            switch (_thousandthPlaceSepartorType)
            {
                case EasyUINumericalSeparatorType.None:
                    numberFormatInfo.NumberGroupSeparator = string.Empty;
                    break;
                case EasyUINumericalSeparatorType.UseSpaces:
                    numberFormatInfo.NumberGroupSeparator = " ";
                    break;
                case EasyUINumericalSeparatorType.UseCommas:
                    numberFormatInfo.NumberGroupSeparator = ",";
                    break;
                case EasyUINumericalSeparatorType.UsePeriods:
                    numberFormatInfo.NumberGroupSeparator = ".";
                    break;
            }

            switch (_decimalPlaceSepartorType)
            {
                case EasyUINumericalSeparatorType.None:
                    numberFormatInfo.NumberGroupSeparator = _numberFormatType == EasyUINumericalBaseFormatType.Currency ?
                        "." : string.Empty;
                    break;
                case EasyUINumericalSeparatorType.UseSpaces:
                    numberFormatInfo.NumberDecimalSeparator = " ";
                    break;
                case EasyUINumericalSeparatorType.UseCommas:
                    numberFormatInfo.NumberDecimalSeparator = ",";
                    break;
                case EasyUINumericalSeparatorType.UsePeriods:
                    numberFormatInfo.NumberDecimalSeparator = ".";
                    break;
            }

            if (_numberFormatType == EasyUINumericalBaseFormatType.Currency)
                return string.Format(numberFormatInfo, "{0:C2}", numberToFormat);

            string format = (Value % 1 == 0) ? "{0:N0}" : "{0:N}";
            return string.Format(numberFormatInfo, format, numberToFormat);
        }

        public void ChangeIconSprite(Sprite newSprite)
        {
            _primaryIcon.PrimarySprite.ElementSprite = newSprite;
            UpdateUIDisplay();
        }

        public void ChangeIconColor(Color newColor)
        {
            _primaryIcon.PrimarySprite.ElementColor = newColor;
            UpdateUIDisplay();
        }

        public void ChangeGaugeColor(Color newColor)
        {
            switch (_fillType)
            {
                case EasyUIFillType.LeftToRight:
                    _horizontalGaugeVisual.PrimarySprite.ElementColor = newColor;
                    break;
                case EasyUIFillType.RightToLeft:
                    _horizontalGaugeVisual.PrimarySprite.ElementColor = newColor;
                    break;
                default:
                    _primaryIcon.PrimarySprite.ElementColor = newColor;
                    break;
            }

            UpdateUIDisplay();
        }

        public void UpdateUIDisplay()
        {
            if (_displayElementReference != null)
                _displayElementReference.UpdateDisplay(this);
        }
    }
}