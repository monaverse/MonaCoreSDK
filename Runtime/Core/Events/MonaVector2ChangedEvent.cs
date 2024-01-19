using UnityEngine;

namespace Mona.SDK.Core.Events
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