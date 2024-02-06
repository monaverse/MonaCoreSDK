using Mona.SDK.Core.State.Structs;

namespace Mona.SDK.Core.Events
{
    public struct MonaValueChangedEvent
    {
        public string Name;
        public IMonaVariablesValue Value;

        public MonaValueChangedEvent(string name, IMonaVariablesValue value)
        {
            Name = name;
            Value = value;
        }
    }
}