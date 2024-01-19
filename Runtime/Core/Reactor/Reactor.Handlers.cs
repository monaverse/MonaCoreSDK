using Mona.SDK.Core.Body;
using System;
using UnityEngine;

namespace Mona
{
    public partial class MonaReactor : MonaBodyBase
    {
        private INetworkMonaReactorClient _networkReactorClient;

        // Int properties
        private void HandleIntEventType(MonaEvent monaEvent, Animator animator)
        {
            var resultInt = HandleIntOperation(monaEvent, animator.GetInteger(monaEvent.Parameter), int.Parse(monaEvent.Value));
            animator.SetInteger(monaEvent.Parameter, resultInt);

            if (monaEvent.Local) return;
            _networkReactorClient?.SetAnimationInt(GetParameterRegistryIndex(monaEvent.Parameter, animator), resultInt);
        }

        private int HandleIntOperation(MonaEvent monaEvent, int a, int b)
        {
            switch (monaEvent.Operation)
            {
                case OperationType.Set:
                    a = b;
                    break;
                case OperationType.Addition:
                    a += b;
                    break;
                case OperationType.Subtraction:
                    a -= b;
                    break;
                case OperationType.Multiplication:
                    a *= b;
                    break;
                case OperationType.Division:
                    a /= b;
                    break;
                case OperationType.Invert:
                    a = a * -1;
                    break;
            }

            return a;
        }

        // Float properties
        private void HandleFloatEventType(MonaEvent monaEvent, Animator animator)
        {
            var resultFloat = HandleFloatOperation(monaEvent, animator.GetFloat(monaEvent.Parameter), float.Parse(monaEvent.Value));

            animator.SetFloat(monaEvent.Parameter, resultFloat);

            if (monaEvent.Local) return;
            _networkReactorClient?.SetAnimationFloat(GetParameterRegistryIndex(monaEvent.Parameter, animator), resultFloat);
        }

        private float HandleFloatOperation(MonaEvent monaEvent, float a, float b)
        {

            switch (monaEvent.Operation)
            {
                case OperationType.Set:
                    a = b;
                    break;
                case OperationType.Addition:
                    a += b;
                    break;
                case OperationType.Subtraction:
                    a -= b;
                    break;
                case OperationType.Multiplication:
                    a *= b;
                    break;
                case OperationType.Division:
                    a /= b;
                    break;
                case OperationType.Invert:
                    a = a * -1;
                    break;
            }

            return a;
        }

        // Bloolean properties
        private void HandleBooleanEventType(MonaEvent monaEvent, Animator animator)
        {
            bool _boolValue = false;

            if (monaEvent.Operation == OperationType.Invert)
            {
                _boolValue = !animator.GetBool(monaEvent.Parameter);
            }
            else
            {
                _boolValue = bool.Parse(monaEvent.Value);
            }

            animator.SetBool(monaEvent.Parameter, _boolValue);

            if (monaEvent.Local) return;
            Debug.Log($"{nameof(MonaReactor)}.{nameof(HandleBooleanEventType)} {_networkReactorClient == null} {monaEvent.Parameter}, {_boolValue}");
            _networkReactorClient?.SetAnimationBool(GetParameterRegistryIndex(monaEvent.Parameter, animator), _boolValue);
        }

        private void HandleTriggerEventType(MonaEvent monaEvent, Animator animator)
        {
            animator.SetTrigger(monaEvent.Parameter);

            if (monaEvent.Local) return;
            _networkReactorClient?.SetAnimationTrigger(GetParameterRegistryIndex(monaEvent.Parameter, animator));
        }
    }
}