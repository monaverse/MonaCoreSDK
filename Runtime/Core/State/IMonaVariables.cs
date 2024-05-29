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

        void Set(string variableName, int value, bool isNetworked = true, bool createIfNotFound = true);
        void Set(string variableName, bool value, bool isNetworked = true, bool createIfNotFound = true);
        void Set(string variableName, string value, bool isNetworked = true, bool createIfNotFound = true);
        void Set(string variableName, float value, bool isNetworked = true, bool createIfNotFound = true);
        void Set(string variableName, IMonaBody value, bool createIfNotFound = true);
        void Set(string variableName, List<IMonaBody> value, bool createIfNotFound = true);
        void Set(string variableName, Vector2 value, bool isNetworked = true, bool createIfNotFound = true);
        void Set(string variableName, Vector3 value, bool isNetworked = true, bool createIfNotFound = true);

        List<IMonaVariablesValue> VariableList { get; set; }
        IMonaVariablesValue GetVariable(string variableName);
        IMonaVariablesValue GetVariable(string variableName, Type type, bool createIfNotFound = true);
        IMonaVariablesValue CreateVariable(string variableName, Type type, int i);
        IMonaVariablesValue GetVariableByIndex(int index);
        int GetVariableIndexByName(string name);

        int GetInt(string variableName, bool createIfNotFound = true);
        bool GetBool(string variableName, bool createIfNotFound = true);
        string GetString(string variableName, bool createIfNotFound = true);
        float GetFloat(string variableName, bool createIfNotFound = true);
        List<IMonaBody> GetBodyArray(string variableName, bool createIfNotFound = true);
        IMonaBody GetBody(string variableName, bool createIfNotFound = true);
        Vector2 GetVector2(string variableName, bool createIfNotFound = true);
        Vector3 GetVector3(string variableName, bool createIfNotFound = true);
        string GetValueAsString(string variableName);

        void CacheVariableNames();
        bool HasUI();
    }

}