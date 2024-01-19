using UnityEngine;

namespace Mona.SDK.Core.Events
{
    public struct MonaVector3ChangedEvent
    {
        public string Name;
        public Vector3 Value;

        public MonaVector3ChangedEvent(string name, Vector3 value)
        {
            Name = name;
            Value = value;
        }
    }
}