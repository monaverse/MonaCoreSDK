namespace Mona.SDK.Core.Events
{
    public struct MonaCursorVisibilityEvent
    {
        public bool IsVisible;
        public MonaCursorVisibilityEvent(bool isVisible)
        {
            IsVisible = isVisible;
        }
    }
}