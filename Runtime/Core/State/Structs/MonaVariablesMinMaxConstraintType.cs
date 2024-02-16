using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mona.SDK.Core.State.Structs
{
    public enum MinMaxConstraintType
    {
        None,
        ConstrainToBounds,
        Loop,
        Bounce,
        ReturnToDefault
    }
}
