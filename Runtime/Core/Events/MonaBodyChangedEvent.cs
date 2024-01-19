using Mona.SDK.Core.Body;

namespace Mona.SDK.Core.Events
{
    public struct MonaBodyChangedEvent
    {
        public string Name;
        public IMonaBody Value;

        public MonaBodyChangedEvent(string name, IMonaBody value)
        {
            Name = name;
            Value = value;
        }
    }
}