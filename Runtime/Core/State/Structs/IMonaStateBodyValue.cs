using Mona.Core.Body;
using UnityEngine;

namespace Mona.Core.State.Structs
{
    public interface IMonaStateBodyValue
    { 
        IMonaBody Value { get; set; }
    }
}