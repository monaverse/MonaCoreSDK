using Mona.SDK.Core.Input;
using System.Collections.Generic;

namespace Mona.SDK.Core.Events
{
    public struct MonaInputsEvent
    {
        public List<MonaInput> Inputs;
        public MonaInputsEvent(List<MonaInput> inputs)
        {
            Inputs = inputs;
        }
    }
}