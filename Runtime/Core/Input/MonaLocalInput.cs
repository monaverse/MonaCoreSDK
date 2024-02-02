using Mona.SDK.Core.Input.Enums;
using Mona.SDK.Core.Input.Interfaces;

namespace Mona.SDK.Core.Input
{
    public struct MonaLocalInput : IMonaLocalInput
    {
        public MonaInputType Type { get; set; }
        public MonaInputState State { get; set; }
    }
}