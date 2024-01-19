using UnityEngine;

namespace Mona.Core.Events
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