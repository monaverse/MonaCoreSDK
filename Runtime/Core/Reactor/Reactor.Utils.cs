using Mona.SDK.Core.Body;
using System.Collections.Generic;
using UnityEngine;

namespace Mona
{
    public partial class MonaReactor : MonaBodyBase
    {
        private bool _hasRegisteredMonaEvents = false;

        private List<ParameterRegistryRecord> _parameterRegistry = new List<ParameterRegistryRecord>();
        public List<ParameterRegistryRecord> parameterRegistry => _parameterRegistry;

        public int GetParameterRegistryIndex(string parameter, Animator animator)
        {
            RegisterAllMonaEvents();
            return _parameterRegistry.FindIndex((x) => x.name == parameter && x.animator == animator);
        }

        public ParameterRegistryRecord GetParameterRegistryRecord(int idx)
        {
            RegisterAllMonaEvents();
            if (idx >= 0 && idx < _parameterRegistry.Count)
            {
                return _parameterRegistry[idx];
            }
            else
            {
                Debug.LogError($"{nameof(GetParameterRegistryRecord)} idx {idx} is out of bounds");
                return default;
            }
        }

        public string GetParameterRegistryName(int idx)
        {
            RegisterAllMonaEvents();
            if (idx >= 0 && idx < _parameterRegistry.Count)
            {
                return _parameterRegistry[idx].name;
            }
            else
            {
                Debug.LogError($"{nameof(GetParameterRegistryName)} idx {idx} is out of bounds");
                return null;
            }
        }

        /// <summary>
        /// Registers animator to the network and re-names the animator to a network ID 
        /// </summary>
        /// <param name="_events"></param>
        public void RegisterMonaEvents(MonaEvent[] monaEvents)
        {
            if (monaEvents == null) return;
            for (int i = 0; i < monaEvents.Length; i++)
                RegisterParameterNames(monaEvents[i]);
        }

        private void RegisterAllMonaEvents()
        {
            if (_hasRegisteredMonaEvents) return;
            _hasRegisteredMonaEvents = true;

            RegisterMonaEvents(OnEnterTrigger);
            RegisterMonaEvents(OnExitTrigger);
            RegisterMonaEvents(OnPlayerInteract);
            RegisterMonaEvents(OnPlayerLookStart);
            RegisterMonaEvents(OnPlayerLookEnd);
            RegisterMonaEvents(OnObjectEnable);
            RegisterMonaEvents(OnObjectDisable);
        }

        public void RegisterParameterNames(MonaEvent monaEvent)
        {
            //Debug.Log($"MonaReactor.NetworkRegisterAnimator - name: {monaEvent.Parameter}, target: {monaEvent.Object}");            
            var animator = monaEvent.Object.GetComponent<Animator>();
            if (_parameterRegistry.FindIndex((x) => x.name == monaEvent.Parameter && x.animator == animator) == -1)
                _parameterRegistry.Add(new ParameterRegistryRecord() { name = monaEvent.Parameter, valueType = monaEvent.ValueType, animator = animator});
        }

        /// <summary>
        /// Handles the incoming event, then executes all the registered events for that action.
        /// If the event was triggered by a collider event then pass the generators collider as the eventObject else it will be null.
        /// </summary>
        /// <param name="_eventActions"></param>
        /// <param name="_eventObject"></param>
        public void HandleMonaEvents(MonaEvent[] monaEvents, Collider eventObject = null)
        {
            foreach (string generatorTag in EventGeneratorTags)
            {
                // If trigger object has correct tag
                if (eventObject != null && !eventObject.gameObject.CompareTag(generatorTag))
                {
                    return;
                }

                // Execute all events
                foreach (var monaEvent in monaEvents)
                {
                    ExecuteEvent(monaEvent);
                }
            }
        }
    }
}