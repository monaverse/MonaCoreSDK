namespace Mona.SDK.Core.Events
{
    public struct MonaKeyBindingsEvent
    {
        public bool Enable;
        public MonaKeyBindingsEvent(bool enable)
        {
            Enable = enable;
        }
    }
}