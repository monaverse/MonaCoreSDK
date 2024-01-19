using UnityEngine;

namespace Mona.Core.Events
{
    public struct MonaVector2ChangedEvent
    {
        public string Name;
        public Vector2 Value;

        public MonaVector2ChangedEvent(string name, Vector2 value)
        {
            Name = name;
            Value = value;
        }
    }
}