using System.Collections.Generic;
using Mona.SDK.Core.Body;
using UnityEngine;

namespace Mona.SDK.Core.State.Structs
{
    public interface IMonaVariablesBodyArrayValue
    { 
        List<IMonaBody> Value { get; set; }
    }
}