using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mona.SDK.Core.EasyUI
{
    [System.Serializable]
    public enum EasyUINetworkDisplayType
    {
        DisplayForAll,
        DisplayForClientOnly,
        HideForAll
    }

    [System.Serializable]
    public enum EasyUIDisplaySpace
    {
        HeadsUpDisplay,
        OnObject,
        OnAvatar
    }

    [System.Serializable]
    public enum EasyUIScreenPosition
    {
        TopLeft,
        TopCenter,
        TopRight,
        BottomLeft,
        BottomCenter,
        BottomRight
    }

    [System.Serializable]
    public enum EasyUIObjectPosition
    {
        Above,
        Below
    }

    [System.Serializable]
    public enum EasyUINumericalLayoutType
    {
        None,
        GaugeFill,
        ObjectCounter
    }

    [System.Serializable]
    public enum EasyUIFillType
    {
        LeftToRight,
        RightToLeft,
        BottomUp,
        TopDown,
        Radial
    }

    [System.Serializable]
    public enum EasyUIGaugeValueDisplay
    {
        None,
        CurrentValueOutOfMax,
        CurrentValueOnly,
        AsPercentage
    }

    [System.Serializable]
    public enum EasyUIElementDisplayType
    {
        Default,
        None,
        Custom
    }

    [System.Serializable]
    public enum EasyUIElementDisplayDefaultOrCustom
    {
        Default,
        Custom
    }

    [System.Serializable]
    public enum EasyUINumericalBaseFormatType
    {
        Default,
        Currency,
        Percentage
    }

    [System.Serializable]
    public enum MinMaxNumericalFormatting
    {
        None,
        ShowMax,
        ShowMin,
        ShowMinAndMax
    }

    public enum EasyUINumericalSeparatorType
    {
        Default,
        None,
        UseSpaces,
        UseCommas,
        UsePeriods
    }

    public enum EasyUIPulseType
    {
        None,
        WhenValueLessThan,
        WhenValueGreaterThan,
        WhenPercentageLessThan,
        WhenPercentageGreaterThan
    }

    public enum EasyUITextAlignment
    {
        Default,
        Left,
        Center,
        Right
    }
}
