namespace Mona.Core.Events
{
    public struct MonaStringChangedEvent
    {
        public string Name;
        public string Value;

        public MonaStringChangedEvent(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }
}