using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mona.SDK.Core.EasyUI
{
    public interface IEasyUINumericalDisplay
    {
        /// <summary>
        /// Can this Value be displayed as a UI element? This can not be set at run-time.
        /// </summary>
        bool AllowUIDisplay { get; set; }

        /// <summary>
        /// Display the UI element?
        /// </summary>
        bool DisplayInUI { get; set; }

        /// <summary>
        /// Defines whether to display this UI on the screen or on an in-world object.
        /// </summary>
        EasyUIDisplaySpace DisplaySpace { get; set; }

        /// <summary>
        /// The UI display position on the player's screen.
        /// </summary>
        EasyUIScreenPosition ScreenPosition { get; set; }

        /// <summary>
        /// The UI display position relative to an object or the local avatar.
        /// </summary>
        EasyUIObjectPosition ObjectPosition { get; set; }

        /// <summary>
        /// The higher the number, the higher priority of the UI element which dictates display position.
        /// </summary>
        int Priority { get; set; }

        /// <summary>
        /// The display name for the variable
        /// </summary>
        EasyUIStringDisplay DisplayName { get; set; }

        /// <summary>
        /// A tooltip to be displayed on-hover for the variable
        /// </summary>
        EasyUIStringDisplay Tooltip { get; set; }

        /// <summary>
        /// The primary icon for the variable. Is used as main element for vertical and radial gauges.
        /// </summary>
        EasyUICompoundSpriteDisplay PrimaryIcon { get; set; }

        /// <summary>
        /// The background graphics for this UI element.
        /// </summary>
        EasyUISpriteDisplay UIBackground { get; set; }

        /// <summary>
        /// The primary display visuals for the UI element.
        /// Note: This setting is only available if Min/Max is enabled for the value.
        /// </summary>
        EasyUINumericalLayoutType ValueDisplayType { get; set; }

        /// <summary>
        /// The fill direction for a gauge.
        /// </summary>
        EasyUIFillType FillType { get; set; }

        /// <summary>
        /// The display settings for the horizontal gauge
        /// </summary>
        EasyUICompoundSpriteDisplay HorizontalGaugeVisual { get; set; }

        EasyUIStringDisplay NumberDisplay { get; set; }
        string NumberPrefix { get; set; }
        string NumberSuffix { get; set; }
        EasyUINumericalBaseFormatType NumberFormatType { get; set; }
        EasyUINumericalSeparatorType ThousandthPlaceSepartorType { get; set; }
        string ThousandthPlaceSeparator { get; }
        EasyUINumericalSeparatorType DecimalPlaceSepartorType { get; set; }
        string DecimalPlaceSeparator { get; }
        string FormattedNumber { get; }

        /// <summary>
        /// The animated pulse type for UI
        /// </summary>
        EasyUIPulseType PulseType { get; set; }
        float PulseStartValue { get; set; }
        float PulseFrequency { get; set; }
    }
}
