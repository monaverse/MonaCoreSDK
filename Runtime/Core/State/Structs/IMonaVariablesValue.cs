using System;

namespace Mona.SDK.Core.State.Structs
{
    public interface IMonaVariablesValue
    {
        event Action OnChange;
        void Change();
        string Name { get; set; }
    }
}