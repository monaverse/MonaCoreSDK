using UnityEngine;

namespace Mona.SDK.Core.EasyUI
{
    public interface IEasyUICommonElementDisplay
    {
        public bool DisplayElement { get; set; }
        public EasyUIElementDisplayType ElementType { get; set; }
        public Color ElementColor { get; set; }
        public EasyUIElementDisplayType ShadowType { get; set; }
        public Vector2 ShadowOffset { get; set; }
    }
}