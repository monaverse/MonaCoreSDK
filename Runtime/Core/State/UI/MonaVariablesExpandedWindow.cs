using System;  
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using Mona.SDK.Core.State.Structs;
using UnityEngine.UIElements;
using Mona.SDK.Core.EasyUI;

namespace Mona.SDK.Core.State.UIElements
{
#if UNITY_EDITOR
    public class MonaVariablesExpandedWindow : EditorWindow
    {
        private IMonaVariablesValue _variable;

        public IMonaVariablesValue Variable { get => _variable; set => _variable = value; }

        protected FloatField _value;
        protected FloatField _min;
        protected FloatField _max;

        Action callback;

        public static void Open(IMonaVariablesValue newVariable, Action newCallback)
        {
            var window = GetWindow<MonaVariablesExpandedWindow>("Variable Editor");
            window.SetupVariable(newVariable);
            window.callback = newCallback;
        }

        public void SetupVariable(IMonaVariablesValue newVariable)
        {
            Variable = newVariable;
            _min.value = ((IMonaVariablesFloatValue)_variable).Min;
            _max.value = ((IMonaVariablesFloatValue)_variable).Max;
            Debug.Log("AA: Set up new variable");
        }

        private void CreateGUI()
        {
            _min = new FloatField();
            _min.RegisterValueChangedCallback((evt) =>
            {
                ((IMonaVariablesFloatValue)_variable).Min = evt.newValue;
                if (callback != null)
                    callback.Invoke();
            });

            _max = new FloatField();
            _max.RegisterValueChangedCallback((evt) =>
            {
                ((IMonaVariablesFloatValue)_variable).Max = evt.newValue;
                if (callback != null)
                    callback.Invoke();
            });

            rootVisualElement.Add(_min);
            rootVisualElement.Add(_max);
            Debug.Log("AA: Create GUI");
        }

        //private void OnGUI()
        //{
        //    _variable.Name = EditorGUILayout.TextField("Name", _variable.Name);
        //    ((IMonaVariablesFloatValue)_variable).Value = EditorGUILayout.FloatField("Value", ((IMonaVariablesFloatValue)_variable).Value);
        //    ((IMonaVariablesFloatValue)_variable).MinMaxType = (MinMaxConstraintType)EditorGUILayout.EnumPopup("Min/Max Type", ((IMonaVariablesFloatValue)_variable).MinMaxType);

        //    if (((IMonaVariablesFloatValue)_variable).UseMinMax)
        //    {
        //        ((IMonaVariablesFloatValue)_variable).Min = EditorGUILayout.FloatField("Minimum Range", ((IMonaVariablesFloatValue)_variable).Min);
        //        ((IMonaVariablesFloatValue)_variable).Max = EditorGUILayout.FloatField("Maximum Range", ((IMonaVariablesFloatValue)_variable).Max);
        //    }

        //    ((IEasyUINumericalDisplay)_variable).AllowUIDisplay = EditorGUILayout.Toggle("Allow UI Display", ((IEasyUINumericalDisplay)_variable).AllowUIDisplay);


        //}
    }
#endif
}

