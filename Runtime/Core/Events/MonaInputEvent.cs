using Mona.SDK.Core.Input.Interfaces;
using System.Collections.Generic;

namespace Mona.SDK.Core.Events
{
    public struct MonaInputEvent
    {
        public List<IMonaLocalInput> Inputs;
        public MonaInputEvent(List<IMonaLocalInput> inputs)
        {
            Inputs = inputs;
        }
    }
}