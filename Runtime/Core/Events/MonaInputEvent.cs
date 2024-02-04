using Mona.SDK.Core.Input;

namespace Mona.SDK.Core.Events
{
    public struct MonaInputEvent
    {
        public MonaInput Input;
        public MonaInputEvent(MonaInput input)
        {
            Input = input;
        }
    }
}