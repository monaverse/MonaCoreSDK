namespace Mona.SDK.Core.Events
{
    public struct MonaFloatChangedEvent
    {
        public string Name;
        public float Value;

        public MonaFloatChangedEvent(string name, float value)
        {
            Name = name;
            Value = value;
        }
    }
}