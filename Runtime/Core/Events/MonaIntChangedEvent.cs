namespace Mona.Core.Events
{
    public struct MonaIntChangedEvent
    {
        public string Name;
        public int Value;

        public MonaIntChangedEvent(string name, int value)
        {
            Name = name;
            Value = value;
        }
    }
}