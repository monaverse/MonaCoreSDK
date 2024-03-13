using Mona.SDK.Core.Input;
using System.Collections.Generic;

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