namespace Mona.Core.Events
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