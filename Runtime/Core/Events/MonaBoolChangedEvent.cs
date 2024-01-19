namespace Mona.Core.Events
{
    public struct MonaBoolChangedEvent
    {
        public string Name;
        public bool Value;

        public MonaBoolChangedEvent(string name, bool value)
        {
            Name = name;
            Value = value;
        }
    }
}