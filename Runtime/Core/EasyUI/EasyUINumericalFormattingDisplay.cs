using System;
using UnityEngine;

namespace Mona.SDK.Core.EasyUI
{
    [Serializable]
    public class EasyUINumericalFormattingDisplay
    {
        [SerializeField] private MinMaxNumericalFormatting _minMaxFormatting = MinMaxNumericalFormatting.None;
        [SerializeField] private string _numberPrefix;
        [SerializeField] private string _numberSuffix;
        [SerializeField] private EasyUINumericalBaseFormatType _numberFormatType = EasyUINumericalBaseFormatType.Default;
        [SerializeField] private EasyUINumericalSeparatorType _thousandthPlaceSeparatorType = EasyUINumericalSeparatorType.Default;
        [SerializeField] private EasyUINumericalSeparatorType _decimalPlaceSeparatorType = EasyUINumericalSeparatorType.Default;
        [SerializeField] private EasyUIElementDisplayType _decimalOverrideType = EasyUIElementDisplayType.Default;
        [SerializeField] private int _customDecimalPlaces = 2;

        public MinMaxNumericalFormatting MinMaxFormatting { get => _minMaxFormatting; set => _minMaxFormatting = value; }
        public string NumberPrefix { get => _numberPrefix; set => _numberPrefix = value; }
        public string NumberSuffix { get => _numberSuffix; set => _numberSuffix = value; }
        public EasyUINumericalBaseFormatType NumberFormatType { get => _numberFormatType; set => _numberFormatType = value; }
        public EasyUINumericalSeparatorType ThousandthPlaceSeparatorType { get => _thousandthPlaceSeparatorType; set => _thousandthPlaceSeparatorType = value; }
        public EasyUINumericalSeparatorType DecimalPlaceSeparatorType { get => _decimalPlaceSeparatorType; set => _decimalPlaceSeparatorType = value; }
        public EasyUIElementDisplayType DecimalOverrideType { get => _decimalOverrideType; set => _decimalOverrideType = value; }
        public int CustomDecimalPlaces
        {
            get => _customDecimalPlaces;
            set
            {
                int clampedValue = Math.Clamp(value, 0, 10);
                _customDecimalPlaces = clampedValue;
            }
        }
    }
}

