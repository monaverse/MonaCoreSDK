namespace Mona.SDK.Core.Events
{
    public struct MonaPlayerInputEvent
    {
        public bool IsEnabled;
        public MonaPlayerInputEvent(bool isEnabled)
        {
            IsEnabled = isEnabled;
        }
    }
}