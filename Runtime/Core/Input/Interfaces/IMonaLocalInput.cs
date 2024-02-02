using Mona.SDK.Core.Input.Enums;

namespace Mona.SDK.Core.Input.Interfaces
{
    public interface IMonaLocalInput
    {
        public MonaInputType Type { get; set; }
        public MonaInputState State { get; set; }
    }
}