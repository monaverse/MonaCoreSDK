﻿using System;
using UnityEngine;
using Mona.SDK.Core.EasyUI;
using System.Globalization;
using System.Text;

namespace Mona.SDK.Core.State.Structs
{
    [Serializable]
    public class MonaVariablesFloat : IMonaVariablesValue, IMonaVariablesFloatValue, IEasyUINumericalDisplay
    {
        public event Action OnChange = delegate { };

        public void Change() => OnChange();

        // ------ Base Values ------

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = 17;

                hash = hash * 23 + (_name?.GetHashCode() ?? 0);
                hash = hash * 23 + _value.GetHashCode();

                return hash;
            }
        }

        public override bool Equals(object obj)
        {
            if (!(obj is IMonaVariablesValue other))
            {
                return false;
            }

            return Equals(other);
        }

        public bool Equals(IMonaVariablesValue other)
        {
            return GetHashCode() == other.GetHashCode();
        }

        [SerializeField] private string _name;
        [SerializeField] public float _value = 1f;
        [SerializeField] private float _defaultValue;
        [SerializeField] private NumberRoundingType _roundingType = NumberRoundingType.None;
        [SerializeField] private MinMaxConstraintType _minMaxType = MinMaxConstraintType.None;
        [SerializeField] private float _min = 0f;
        [SerializeField] private float _max = 10f;
        [SerializeField] private bool _returnRandomValueFromMinMax;
        [SerializeField] private bool _useRandomSeed;
        [SerializeField] private string _randomSeed = "Any Text";


        [SerializeField]
        private bool _isLocal;

        public bool IsLocal { get => _isLocal; set => _isLocal = value; }
        private CoreRandom _randomValue;

        public string Name { get => _name; set => _name = value; }
        public float DefaultValue { get => _defaultValue; set => _defaultValue = value; }
        public NumberRoundingType RoundingType { get => _roundingType; set => _roundingType = value; }
        public bool UseMinMax { get => _minMaxType != MinMaxConstraintType.None; }
        public MinMaxConstraintType MinMaxType { get => _minMaxType; set => _minMaxType = value; }
        public bool ReturnRandomValueFromMinMax { get => _returnRandomValueFromMinMax; set => _returnRandomValueFromMinMax = value; }
        public bool UseRandomSeed { get => _useRandomSeed; set => _useRandomSeed = value; }
        private float MinMaxRange => _max - _min;

        private float _resetValue;

        public void Reset()
        {
            _value = _resetValue;
        }

        public void SaveReset()
        {
            _resetValue = _value;
        }

        public void ResetRandom()
        {
            _randomValue.Reset(_randomSeed);
        }

        public float Value
        {
            get
            {
                if (_returnRandomValueFromMinMax && UseMinMax)
                {
                    float randomValue = _roundingType == NumberRoundingType.None ?
                        UnityEngine.Random.Range(_min, _max) : UnityEngine.Random.Range((int)_min, ((int)_max) + 1);
                    _value = randomValue;
                    UpdateUIDisplay();
                }

                return _value;
            }
            set
            {
                value = RoundValue(value);

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
                        while (value < _min)
                        {
                            value += MinMaxRange + 1;
                        }
                        _value = _min + (value - _min) % (MinMaxRange + 1);
                        break;
                    case MinMaxConstraintType.Bounce:
                        float adjustedValue = (value - _min) % (MinMaxRange * 2);
                        _value = _min + (adjustedValue <= MinMaxRange ? adjustedValue : MinMaxRange * 2 - adjustedValue);
                        break;
                    case MinMaxConstraintType.ReturnToDefault:
                        _value = value < _min || value > _max ? _defaultValue : value;
                        break;
                    default:
                        _value = value;
                        break;
                }

                UpdateUIDisplay();
            }
        }

        public float ValueToReturnFromTile
        {
            get
            {
                if (!_returnRandomValueFromMinMax && !_useRandomSeed)
                    return Value;

                float randomValue = _roundingType == NumberRoundingType.None ?
                    _randomValue.NextFloat(_min, _max) : _randomValue.Next((int)_min, (int)_max);

                _value = randomValue;
                UpdateUIDisplay();

                return _value;
            }
        }

        public float Min
        {
            get { return _min; }
            set
            {
                value = RoundValue(value);

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
                value = RoundValue(value);

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

        public void ForceMinMax(float min, float max)
        {
            float newMin = min <= max ? min : max;
            float newMax = max >= min ? max : min;

            _min = newMin;
            _max = newMax;
        }

        public string RandomSeed
        {
            get { return _randomSeed; }
            set
            {
                _randomSeed = value;
                _randomValue = new CoreRandom(_randomSeed);
            }
        }

        // ------ UI Related Values ------ //

        // General Display and UI Placement

        private EasyUIVariableDisplayElement _displayElementReference = null;
        public EasyUIVariableDisplayElement DisplayElementReference { get => _displayElementReference; set => _displayElementReference = value; }

        [SerializeField] private bool _allowUIDisplay = false;
        [SerializeField] private bool _displayInUI = true;
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
        [SerializeField] private EasyUINumericalFormattingDisplay _numericalFormatting = new EasyUINumericalFormattingDisplay();

        public EasyUIStringDisplay DisplayName { get => _displayName; set => _displayName = value; }
        public EasyUIStringDisplay Tooltip { get => _tooltip; set => _tooltip = value; }
        public EasyUICompoundSpriteDisplay PrimaryIcon { get => _primaryIcon; set => _primaryIcon = value; }
        public EasyUISpriteDisplay UIBackground { get => _uiBackground; set => _uiBackground = value; }
        public EasyUINumericalLayoutType ValueDisplayType { get => _valueDisplayType; set => _valueDisplayType = value; }
        public EasyUIFillType FillType { get => _fillType; set => _fillType = value; }
        public EasyUICompoundSpriteDisplay HorizontalGaugeVisual { get => _horizontalGaugeVisual; set => _horizontalGaugeVisual = value; }
        public EasyUIStringDisplay NumberDisplay { get => _numberDisplay; set => _numberDisplay = value; }
        public EasyUINumericalFormattingDisplay NumericalFormatting { get => _numericalFormatting; set => _numericalFormatting = value; }
        public bool DisplayAsGauge => ValueDisplayType == EasyUINumericalLayoutType.GaugeFill;
        public bool UseHorizontalGauge => DisplayAsGauge && (FillType == EasyUIFillType.LeftToRight || FillType == EasyUIFillType.RightToLeft);

        public float GaugeFillAmount
        {
            get
            {
                if (!UseMinMax || Mathf.Approximately(_max - _min, 0f))
                    return 0f;

                return (_value - _min) / (_max - _min);

            }
        }

        // Number formatting
        private StringBuilder _formattedNumber = new StringBuilder();
        private StringBuilder _minString = new StringBuilder();
        private StringBuilder _maxString = new StringBuilder();
        private const string FORWARD_SLASH_SPACES = " / ";
        private const string PERCENT = "%";

        public string FormattedNumber
        {
            get
            {
                _formattedNumber.Clear();

                float displayValue = UseMinMax && _numericalFormatting.NumberFormatType == EasyUINumericalBaseFormatType.Percentage ?
                    GaugeFillAmount * 100f : _value;

                _minString.Clear();
                _maxString.Clear();
                string percentageString = _numericalFormatting.NumberFormatType == EasyUINumericalBaseFormatType.Percentage ?
                    PERCENT : string.Empty;

                if (UseMinMax)
                {
                    float minValue = _numericalFormatting.NumberFormatType == EasyUINumericalBaseFormatType.Percentage ?
                        0f : _min;
                    float maxValue = _numericalFormatting.NumberFormatType == EasyUINumericalBaseFormatType.Percentage ?
                        100f : _max;

                    if (_numericalFormatting.MinMaxFormatting == MinMaxNumericalFormatting.ShowMin || _numericalFormatting.MinMaxFormatting == MinMaxNumericalFormatting.ShowMinAndMax)
                    {
                        _minString.Append(_numericalFormatting.NumberPrefix);
                        _minString.Append(FormatNumber(minValue));
                        _minString.Append(percentageString);
                        _minString.Append(_numericalFormatting.NumberSuffix);
                        _minString.Append(FORWARD_SLASH_SPACES);
                    }

                    if(_numericalFormatting.MinMaxFormatting == MinMaxNumericalFormatting.ShowMax || _numericalFormatting.MinMaxFormatting == MinMaxNumericalFormatting.ShowMinAndMax)
                    {
                        _maxString.Append(FORWARD_SLASH_SPACES);
                        _maxString.Append(_numericalFormatting.NumberPrefix);
                        _maxString.Append(FormatNumber(maxValue));
                        _maxString.Append(percentageString);
                        _maxString.Append(_numericalFormatting.NumberSuffix);
                    }                
                }

                _formattedNumber.Append(_minString.ToString());
                _formattedNumber.Append(_numericalFormatting.NumberPrefix);
                _formattedNumber.Append(FormatNumber(displayValue));
                _formattedNumber.Append(percentageString);
                _formattedNumber.Append(_numericalFormatting.NumberSuffix);
                _formattedNumber.Append(_maxString.ToString());

                return _formattedNumber.ToString();
            }
        }

        // Animation Settings

        [SerializeField] private EasyUIPulseType _pulseType = EasyUIPulseType.None;
        [SerializeField] private float _pulseStartValue = 0.25f;
        [SerializeField] private float _pulseFrequency = 0.5f;

        public EasyUIPulseType PulseType { get => _pulseType; set => _pulseType = value; }
        public float PulseStartValue { get => _pulseStartValue; set => _pulseStartValue = value; }
        public float PulseFrequency { get => _pulseFrequency; set => _pulseFrequency = value; }

        public MonaVariablesFloat()
        {
            _randomValue = new CoreRandom(_randomSeed);
        }

        private float RoundValue(float value)
        {
            switch (_roundingType)
            {
                case NumberRoundingType.RoundClosest: return Mathf.Round(value);
                case NumberRoundingType.RoundUp: return Mathf.Ceil(value);
                case NumberRoundingType.RoundDown: return Mathf.Floor(value);
                default: return value;
            }
        }

        public const string NUMBER_GROUP_SEPARATOR_SPACE = " ";
        public const string NUMBER_GROUP_SEPARATOR_COMMA = ",";
        public const string NUMBER_GROUP_SEPARATOR_PERIOD = ".";
        private const string CUSTOM_DECIMAL_FORMAT_PREFIX = "{0:N";
        private const string CUSTOM_DECIMAL_FORMAT_SUFFIX = "}";

        private CultureInfo _cultureInfo;
        private NumberFormatInfo _numberFormatInfo;
        private StringBuilder _customDecimalFormat = new StringBuilder();

        public string FormatNumber(float numberToFormat)
        {
            if (_cultureInfo == null)
            {
                _cultureInfo = CultureInfo.CurrentCulture;
                _numberFormatInfo = (NumberFormatInfo)_cultureInfo.NumberFormat.Clone();
            }

            switch (_numericalFormatting.ThousandthPlaceSeparatorType)
            {
                case EasyUINumericalSeparatorType.None:
                    _numberFormatInfo.NumberGroupSeparator = string.Empty;
                    break;
                case EasyUINumericalSeparatorType.UseSpaces:
                    _numberFormatInfo.NumberGroupSeparator = NUMBER_GROUP_SEPARATOR_SPACE;
                    break;
                case EasyUINumericalSeparatorType.UseCommas:
                    _numberFormatInfo.NumberGroupSeparator = NUMBER_GROUP_SEPARATOR_COMMA;
                    break;
                case EasyUINumericalSeparatorType.UsePeriods:
                    _numberFormatInfo.NumberGroupSeparator = NUMBER_GROUP_SEPARATOR_PERIOD;
                    break;
            }

            switch (_numericalFormatting.DecimalPlaceSeparatorType)
            {
                case EasyUINumericalSeparatorType.None:
                    _numberFormatInfo.NumberGroupSeparator = _numericalFormatting.NumberFormatType == EasyUINumericalBaseFormatType.Currency ?
                        "." : string.Empty;
                    break;
                case EasyUINumericalSeparatorType.UseSpaces:
                    _numberFormatInfo.NumberDecimalSeparator = NUMBER_GROUP_SEPARATOR_SPACE;
                    break;
                case EasyUINumericalSeparatorType.UseCommas:
                    _numberFormatInfo.NumberDecimalSeparator = NUMBER_GROUP_SEPARATOR_COMMA;
                    break;
                case EasyUINumericalSeparatorType.UsePeriods:
                    _numberFormatInfo.NumberDecimalSeparator = NUMBER_GROUP_SEPARATOR_PERIOD;
                    break;
            }

            switch (_numericalFormatting.NumberFormatType)
            {
                case EasyUINumericalBaseFormatType.Currency:
                    string monetaryFormat = "{0:C" + DecimalPlaceDisplay + "}";
                    return string.Format(_numberFormatInfo, monetaryFormat, numberToFormat);
                case EasyUINumericalBaseFormatType.Time:
                    return TimeFormatString(numberToFormat, _numberFormatInfo);
            }

            if (_numericalFormatting.DecimalOverrideType == EasyUIElementDisplayType.Default)
                return FormatNumber(numberToFormat, _numberFormatInfo);

            _customDecimalFormat.Clear();
            _customDecimalFormat.Append(CUSTOM_DECIMAL_FORMAT_PREFIX);
            _customDecimalFormat.Append(DecimalPlaceDisplay);
            _customDecimalFormat.Append(CUSTOM_DECIMAL_FORMAT_SUFFIX);
            string customDecimalFormat = _customDecimalFormat.ToString();
            return string.Format(_numberFormatInfo, customDecimalFormat, numberToFormat);
        }

        private string DecimalPlaceDisplay
        {
            get
            {
                switch (_numericalFormatting.DecimalOverrideType)
                {
                    case EasyUIElementDisplayType.Custom:
                        return _numericalFormatting.CustomDecimalPlaces.ToString();
                    case EasyUIElementDisplayType.None:
                        return "0";
                }

                return _numericalFormatting.NumberFormatType == EasyUINumericalBaseFormatType.Currency ?
                    "2" : string.Empty;
            }
        }

        private string FormatNumber(float numberToFormat, NumberFormatInfo numberFormatInfo)
        {
            string format = (numberToFormat % 1 == 0) ? "{0:N0}" : "{0:N}";
            return string.Format(numberFormatInfo, format, numberToFormat);
        }

        public string TimeFormatString(float timeInSeconds, NumberFormatInfo numberFormatInfo)
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(timeInSeconds);
            string primarySeparator = _numericalFormatting.TimeSeparatorType == EasyUITimeSeparatorType.Default ?
                ":" : "   ";
            string decimalSeparator = _numericalFormatting.TimeSeparatorType == EasyUITimeSeparatorType.Default ?
                " . " : "   ";

            switch (_numericalFormatting.TimeFormatting)
                {
                    case EasyUITimeFormatting.Seconds:
                        return string.Format("{0}", FormatNumber((int)timeSpan.TotalSeconds, numberFormatInfo));
                    case EasyUITimeFormatting.SecondsCentiseconds:
                        return string.Format("{0}{2}{1:D2}", FormatNumber((int)timeSpan.TotalSeconds, numberFormatInfo), timeSpan.Milliseconds / 10, decimalSeparator);
                    case EasyUITimeFormatting.SecondsFrames:
                        return string.Format("{0}{1}", FormatNumber((int)timeSpan.TotalSeconds, numberFormatInfo), decimalSeparator) + CurrentFrameString(timeInSeconds);
                    case EasyUITimeFormatting.MinutesSeconds:
                        return string.Format("{0}{2}{1:D2}", FormatNumber((int)timeSpan.TotalMinutes, numberFormatInfo), timeSpan.Seconds, primarySeparator);
                    case EasyUITimeFormatting.MinutesSecondsCentiseconds:
                        return string.Format("{0}{3}{1:D2}{4}{2:D2}", FormatNumber((int)timeSpan.TotalMinutes, numberFormatInfo), timeSpan.Seconds, timeSpan.Milliseconds / 10, primarySeparator, decimalSeparator);
                    case EasyUITimeFormatting.MinutesSecondsFrames:
                        return string.Format("{0}{2}{1:D2}{3}", FormatNumber((int)timeSpan.TotalMinutes, numberFormatInfo), timeSpan.Seconds, primarySeparator, decimalSeparator) + CurrentFrameString(timeInSeconds);
                    case EasyUITimeFormatting.HoursMinutes:
                        return string.Format("{0}{2}{1:D2}", FormatNumber((int)timeSpan.TotalHours, numberFormatInfo), timeSpan.Minutes, primarySeparator);
                    case EasyUITimeFormatting.HoursMinutesSeconds:
                        return string.Format("{0}{3}{1:D2}{3}{2:D2}", FormatNumber((int)timeSpan.TotalHours, numberFormatInfo), timeSpan.Minutes, timeSpan.Seconds, primarySeparator);
                    case EasyUITimeFormatting.HoursMinutesSecondsCentiseconds:
                        return string.Format("{0}{4}{1:D2}{4}{2:D2}{5}{3:D2}", FormatNumber((int)timeSpan.TotalHours, numberFormatInfo), timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds / 10, primarySeparator, decimalSeparator);
                    case EasyUITimeFormatting.HoursMinutesSecondsFrames:
                        return string.Format("{0}{3}{1:D2}{3}{2:D2}{4}", FormatNumber((int)timeSpan.TotalHours, numberFormatInfo), timeSpan.Minutes, timeSpan.Seconds, primarySeparator, decimalSeparator) + CurrentFrameString(timeInSeconds);
                    default:
                        return string.Format("{0}{2}{1:D2}", FormatNumber((int)timeSpan.TotalMinutes, numberFormatInfo), timeSpan.Seconds, primarySeparator);
            }
        }

        public string CurrentFrameString(float timeInSeconds)
        {
            float fps = _numericalFormatting.TimeDisplayFrameRate == EasyUITimeFrameRates.Default ?
                60f : (int)_numericalFormatting.TimeDisplayFrameRate;
            float hundredthsOfASecond = timeInSeconds - (float)Math.Truncate(timeInSeconds);

            int currentFrame = Mathf.RoundToInt(fps * hundredthsOfASecond);

            return currentFrame.ToString("D2") + "f";
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
            if (!_allowUIDisplay || _displayElementReference == null)
                return;

            _displayElementReference.UpdateDisplay(this);
        }
    }
}