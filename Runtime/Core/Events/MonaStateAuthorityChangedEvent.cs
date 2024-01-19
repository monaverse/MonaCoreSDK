namespace Mona.Core.Events
{
    public struct MonaStateAuthorityChangedEvent
    {
        public bool HasControl;
        public MonaStateAuthorityChangedEvent(bool hasControl)
        {
            HasControl = hasControl;
        }
    }
}