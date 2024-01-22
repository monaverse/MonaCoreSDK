using Mona.SDK.Core.State.Structs;

namespace Mona.SDK.Core.Events
{
    public struct MonaValueChangedEvent
    {
        public string Name;
        public IMonaStateValue Value;

        public MonaValueChangedEvent(string name, IMonaStateValue value)
        {
            Name = name;
            Value = value;
        }
    }
}