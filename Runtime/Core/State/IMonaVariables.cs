using Mona.SDK.Core.Body;
using Mona.SDK.Core.State.Structs;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Mona.SDK.Core.State
{
    public interface IMonaVariables
    {
        void SetGameObject(GameObject gameObject);

        void Set(string variableName, int value, bool isNetworked = true);
        void Set(string variableName, bool value, bool isNetworked = true);
        void Set(string variableName, string value, bool isNetworked = true);
        void Set(string variableName, float value, bool isNetworked = true);
        void Set(string variableName, IMonaBody value);
        void Set(string variableName, Vector2 value, bool isNetworked = true);
        void Set(string variableName, Vector3 value, bool isNetworked = true);

        List<IMonaVariablesValue> VariableList { get; set; }
        IMonaVariablesValue GetVariable(string variableName);
        IMonaVariablesValue GetVariable(string variableName, Type type);
        IMonaVariablesValue CreateVariable(string variableName, Type type, int i);

        int GetInt(string variableName);
        bool GetBool(string variableName);
        string GetString(string variableName);
        float GetFloat(string variableName);
        IMonaBody GetBody(string variableName);
        Vector2 GetVector2(string variableName);
        Vector3 GetVector3(string variableName);
    }

}