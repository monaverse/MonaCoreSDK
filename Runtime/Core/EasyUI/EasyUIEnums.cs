using System;

namespace Mona.SDK.Core.EasyUI
{
    [Serializable]
    public enum EasyUINetworkDisplayType
    {
        DisplayForAll = 0,
        DisplayForClientOnly = 10,
        HideForAll = 20
    }

    [Serializable]
    public enum EasyUIDisplaySpace
    {
        HeadsUpDisplay = 0,
        OnObject = 10,
        OnAvatar = 20
    }

    [Serializable]
    public enum EasyUIScreenPosition
    {
        TopLeft = 0,
        TopCenter = 10,
        TopRight = 20,
        BottomLeft = 30,
        BottomCenter = 40,
        BottomRight = 50
    }

    [Serializable]
    public enum EasyUIObjectPosition
    {
        Above = 0,
        Below = 10
    }

    [Serializable]
    public enum EasyUINumericalLayoutType
    {
        None = 0,
        GaugeFill = 10,
        ObjectCounter = 20
    }

    [Serializable]
    public enum EasyUIFillType
    {
        LeftToRight = 0,
        RightToLeft = 10,
        BottomUp = 20,
        TopDown = 30,
        Radial = 40
    }

    [Serializable]
    public enum EasyUIGaugeValueDisplay
    {
        None = 0,
        CurrentValueOutOfMax = 10,
        CurrentValueOnly = 20,
        AsPercentage = 30
    }

    [Serializable]
    public enum EasyUIElementDisplayType
    {
        Default = 0,
        None = 10,
        Custom = 20
    }

    [Serializable]
    public enum EasyUIElementDisplayDefaultOrCustom
    {
        Default = 0,
        Custom = 10
    }

    [Serializable]
    public enum EasyUINumericalBaseFormatType
    {
        Default = 0,
        Time = 10,
        Currency = 20,
        Percentage = 30
    }

    [Serializable]
    public enum MinMaxNumericalFormatting
    {
        None = 0,
        ShowMax = 10,
        ShowMin = 20,
        ShowMinAndMax = 30
    }

    [Serializable]
    public enum EasyUITimeFormatting
    {
        Default = 0,
        Seconds = 10,
        SecondsCentiseconds = 20,
        SecondsFrames = 30,
        MinutesSeconds = 40,
        MinutesSecondsCentiseconds = 50,
        MinutesSecondsFrames = 60,
        HoursMinutes = 70,
        HoursMinutesSeconds = 80,
        HoursMinutesSecondsCentiseconds = 90,
        HoursMinutesSecondsFrames = 100
    }

    [Serializable]
    public enum EasyUITimeFrameRates
    {
        Default = 0,
        TwelveFPS = 12,
        FifteenFPS = 15,
        TwentyFourFPS = 24,
        ThirtyFPS = 30,
        SixtyFPS = 60,
        SeventyTwoFPS = 72,
        NinetyFPS = 90
    }

    [Serializable]
    public enum EasyUINumericalSeparatorType
    {
        LocalRegionFormatting = 0,
        None = 10,
        UseSpaces = 20,
        UseCommas = 30,
        UsePeriods = 40
    }

    [Serializable]
    public enum EasyUITimeSeparatorType
    {
        Default = 0,
        UseSpaces = 10
    }

    [Serializable]
    public enum EasyUIPulseType
    {
        None = 0,
        WhenValueLessThan = 10,
        WhenValueGreaterThan = 20,
        WhenPercentageLessThan = 30,
        WhenPercentageGreaterThan = 40
    }

    [Serializable]
    public enum EasyUITextAlignment
    {
        Default = 0,
        Left = 10,
        Center = 20,
        Right = 30
    }
}
