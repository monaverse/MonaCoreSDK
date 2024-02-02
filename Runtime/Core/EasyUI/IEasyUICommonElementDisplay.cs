using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mona.SDK.Core.EasyUI
{
    public interface IEasyUICommonElementDisplay
    {
        public bool DisplayElement { get; set; }
        public EasyUIElementDisplayType ElementType { get; set; }
        public Color ElementColor { get; set; }
        public bool UseShadow { get; set; }
        public Vector2 ShadowOffset { get; set; }
    }
}