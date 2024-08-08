
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Mona.SDK.Core.Body
{
    public static class MonaBodyFactory
    {
        public static List<IMonaBody> Empty = new List<IMonaBody>();
        public static List<IMonaBody> MonaBodies = new List<IMonaBody>();
        public static Dictionary<string, List<IMonaBody>> MonaBodiesByTag = new Dictionary<string, List<IMonaBody>>();

        public static IMonaBody[] FindAllMonaBodies() => Array.ConvertAll(GameObject.FindObjectsByType<MonaBody>(FindObjectsSortMode.None), x => (IMonaBody)x);

        public static List<IMonaBody> FindByTag(string tag)
        {
            if (MonaBodiesByTag.ContainsKey(tag))
                return MonaBodiesByTag[tag];
            return Empty;
        }

        public static IMonaBody FindByLocalId(string localId)
        {
            for (var i = 0; i < MonaBodies.Count; i++)
            {
                if (MonaBodies[i].LocalId == localId)
                    return MonaBodies[i];
            }
            return null;
        }

        public static Func<GameObject, IMonaBody> CreateDelegate;
        public static Func<Transform, IMonaBody> CreateTransformDelegate;

        public static IMonaBody Create(GameObject gameObject)
        {
            if (CreateDelegate != null) return CreateDelegate.Invoke(gameObject);
            return gameObject.AddComponent<MonaBody>();
        }

        public static IMonaBody Create(Transform transform)
        {
            if (CreateTransformDelegate != null) return CreateTransformDelegate.Invoke(transform);
            return transform.AddComponent<MonaBody>();
        }

    }
}