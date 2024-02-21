using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Mona.SDK.Core.State.Structs;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using Mona.SDK.Core.EasyUI;

namespace Mona.SDK.Core.State.UIElements
{
    public class MonaVariableDefinitionNumericalDisplayFoldout : Foldout
    {

        [SerializeField] protected EasyUINumericalFormattingDisplay _numericalFormatting;
        [SerializeField] protected EnumField _minMaxFormatting;
        [SerializeField] protected TextField _numberDisplayPrefix;
        [SerializeField] protected TextField _numberDisplaySuffix;
        [SerializeField] protected EnumField _numberFormatType;
        [SerializeField] protected EnumField _timeFormatting;
        [SerializeField] protected EnumField _timeDisplayFrameRate;
        [SerializeField] protected EnumField _timeSeparatorType;
        [SerializeField] protected EnumField _thousandthPlaceSeparatorType;
        [SerializeField] protected EnumField _decimalOverrideType;
        [SerializeField] protected EnumField _decimalPlaceSeparatorType;
        [SerializeField] protected IntegerField _customDecimalPlaces;
        protected bool _useMinMax;
        protected bool _displayAdvanced;

        public void SetDisplay(EasyUINumericalFormattingDisplay numericalFormatting, string labelName, bool useMinMax, bool displayAdvanced)
        {
            _numericalFormatting = numericalFormatting;
            _useMinMax = useMinMax;
            _displayAdvanced = displayAdvanced;
            this.text = labelName;
            Refresh();
        }

        public MonaVariableDefinitionNumericalDisplayFoldout()
        {
            style.paddingLeft = style.paddingRight = style.paddingTop = style.paddingBottom = 10;
            style.marginLeft = style.marginRight = style.marginTop = style.marginBottom = 10;
            style.borderLeftWidth = style.borderRightWidth = style.borderTopWidth = style.borderBottomWidth = 1;
            style.backgroundColor = new Color(0.1f, 0.1f, 0.1f);

            _minMaxFormatting = new EnumField("Min/Max Display", MinMaxNumericalFormatting.ShowMax);
            _minMaxFormatting.RegisterValueChangedCallback((evt) =>
            {
                _numericalFormatting.MinMaxFormatting = (MinMaxNumericalFormatting)evt.newValue;
                Refresh();
            });

            _numberDisplayPrefix = new TextField("Number Prefix");
            _numberDisplayPrefix.RegisterValueChangedCallback((evt) =>
            {
                _numericalFormatting.NumberPrefix = evt.newValue;
                Refresh();
            });

            _numberDisplaySuffix = new TextField("Number Suffix");
            _numberDisplaySuffix.RegisterValueChangedCallback((evt) =>
            {
                _numericalFormatting.NumberSuffix = evt.newValue;
                Refresh();
            });

            _numberFormatType = new EnumField("Base Numerical Format", EasyUINumericalBaseFormatType.Default);
            _numberFormatType.RegisterValueChangedCallback((evt) =>
            {
                _numericalFormatting.NumberFormatType = (EasyUINumericalBaseFormatType)evt.newValue;
                Refresh();
            });

            _timeFormatting = new EnumField("Time Format", EasyUITimeFormatting.Default);
            _timeFormatting.RegisterValueChangedCallback((evt) =>
            {
                _numericalFormatting.TimeFormatting = (EasyUITimeFormatting)evt.newValue;
                Refresh();
            });

            _timeDisplayFrameRate = new EnumField("Frame Rate Units", EasyUITimeFrameRates.Default);
            _timeDisplayFrameRate.RegisterValueChangedCallback((evt) =>
            {
                _numericalFormatting.TimeDisplayFrameRate = (EasyUITimeFrameRates)evt.newValue;
                Refresh();
            });

            _timeSeparatorType = new EnumField("Time Unit Separator", EasyUITimeSeparatorType.Default);
            _timeSeparatorType.RegisterValueChangedCallback((evt) =>
            {
                _numericalFormatting.TimeSeparatorType = (EasyUITimeSeparatorType)evt.newValue;
                Refresh();
            });

            _numberFormatType = new EnumField("Base Numerical Format", EasyUINumericalBaseFormatType.Default);
            _numberFormatType.RegisterValueChangedCallback((evt) =>
            {
                _numericalFormatting.NumberFormatType = (EasyUINumericalBaseFormatType)evt.newValue;
                Refresh();
            });

            _thousandthPlaceSeparatorType = new EnumField("Thousandth Place Separator", EasyUINumericalSeparatorType.LocalRegionFormatting);
            _thousandthPlaceSeparatorType.RegisterValueChangedCallback((evt) =>
            {
                _numericalFormatting.ThousandthPlaceSeparatorType = (EasyUINumericalSeparatorType)evt.newValue;
                Refresh();
            });

            _decimalOverrideType = new EnumField("Decimal Place Display", EasyUIElementDisplayType.Default);
            _decimalOverrideType.RegisterValueChangedCallback((evt) =>
            {
                _numericalFormatting.DecimalOverrideType = (EasyUIElementDisplayType)evt.newValue;
                Refresh();
            });

            _decimalPlaceSeparatorType = new EnumField("Decimal Place Separator", EasyUINumericalSeparatorType.LocalRegionFormatting);
            _decimalPlaceSeparatorType.RegisterValueChangedCallback((evt) =>
            {
                _numericalFormatting.DecimalPlaceSeparatorType = (EasyUINumericalSeparatorType)evt.newValue;
                Refresh();
            });

            _customDecimalPlaces = new IntegerField("Custom Decimal Places");
            _customDecimalPlaces.RegisterValueChangedCallback((evt) =>
            {
                _numericalFormatting.CustomDecimalPlaces = evt.newValue;
                Refresh();
            });

            Add(_minMaxFormatting);
            Add(_numberDisplayPrefix);
            Add(_numberDisplaySuffix);
            Add(_numberFormatType);
            Add(_timeFormatting);
            Add(_timeDisplayFrameRate);
            Add(_timeSeparatorType);
            Add(_thousandthPlaceSeparatorType);
            Add(_decimalOverrideType);
            Add(_decimalPlaceSeparatorType);
            Add(_customDecimalPlaces);
            value = false;
        }

        private void Refresh()
        {
            _minMaxFormatting.value = _numericalFormatting.MinMaxFormatting;
            _numberDisplayPrefix.value = _numericalFormatting.NumberPrefix;
            _numberDisplaySuffix.value = _numericalFormatting.NumberSuffix;
            _numberFormatType.value = _numericalFormatting.NumberFormatType;
            _timeFormatting.value = _numericalFormatting.TimeFormatting;
            _timeDisplayFrameRate.value = _numericalFormatting.TimeDisplayFrameRate;
            _timeSeparatorType.value = _numericalFormatting.TimeSeparatorType;
            _thousandthPlaceSeparatorType.value = _numericalFormatting.ThousandthPlaceSeparatorType;
            _decimalOverrideType.value = _numericalFormatting.DecimalOverrideType;
            _decimalPlaceSeparatorType.value = _numericalFormatting.DecimalPlaceSeparatorType;
            _customDecimalPlaces.value = _numericalFormatting.CustomDecimalPlaces;

            _minMaxFormatting.style.display = _useMinMax ? DisplayStyle.Flex : DisplayStyle.None;
            _numberDisplayPrefix.style.display = DisplayStyle.Flex;
            _numberDisplaySuffix.style.display = DisplayStyle.Flex;
            _numberFormatType.style.display = DisplayStyle.Flex;

            _timeFormatting.style.display = _numericalFormatting.NumberFormatType == EasyUINumericalBaseFormatType.Time ?
                DisplayStyle.Flex : DisplayStyle.None;

            switch (_numericalFormatting.TimeFormatting)
            {
                case EasyUITimeFormatting.SecondsFrames:
                    _timeDisplayFrameRate.style.display = DisplayStyle.Flex;
                    break;
                case EasyUITimeFormatting.MinutesSecondsFrames:
                    _timeDisplayFrameRate.style.display = DisplayStyle.Flex;
                    break;
                case EasyUITimeFormatting.HoursMinutesSecondsFrames:
                    _timeDisplayFrameRate.style.display = DisplayStyle.Flex;
                    break;
                default:
                    _timeDisplayFrameRate.style.display = DisplayStyle.None;
                    break;
            }

            _timeSeparatorType.style.display = _numericalFormatting.NumberFormatType == EasyUINumericalBaseFormatType.Time ?
                DisplayStyle.Flex : DisplayStyle.None;

            _thousandthPlaceSeparatorType.style.display = _numericalFormatting.NumberFormatType != EasyUINumericalBaseFormatType.Percentage ?
                    DisplayStyle.Flex : DisplayStyle.None;

            if (_numericalFormatting.NumberFormatType != EasyUINumericalBaseFormatType.Time)
            {
                _decimalOverrideType.style.display = DisplayStyle.Flex;

                if (_numericalFormatting.DecimalOverrideType == EasyUIElementDisplayType.None)
                {
                    _decimalPlaceSeparatorType.style.display = DisplayStyle.None;
                    _customDecimalPlaces.style.display = DisplayStyle.None;
                }
                else
                {
                    _decimalPlaceSeparatorType.style.display = DisplayStyle.Flex;
                    _customDecimalPlaces.style.display = _numericalFormatting.DecimalOverrideType == EasyUIElementDisplayType.Custom ?
                        DisplayStyle.Flex : DisplayStyle.None;
                }
            }
            else
            {
                _decimalOverrideType.style.display = DisplayStyle.None;
                _decimalPlaceSeparatorType.style.display = DisplayStyle.None;
                _customDecimalPlaces.style.display = DisplayStyle.None;
            }
        }
    }
}
