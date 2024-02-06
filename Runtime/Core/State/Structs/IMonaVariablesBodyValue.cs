using Mona.SDK.Core.Body;
using UnityEngine;

namespace Mona.SDK.Core.State.Structs
{
    public interface IMonaVariablesBodyValue
    { 
        IMonaBody Value { get; set; }
    }
}