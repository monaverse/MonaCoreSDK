using Mona.SDK.Core.Input;
using System.Collections.Generic;

namespace Mona.SDK.Core.Events
{
    public struct MonaInputsEvent
    {
        public MonaInput Input;
        public MonaInputsEvent(MonaInput input)
        {
            Input = input;
        }
    }
}