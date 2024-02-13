
using Mona.SDK.Core.Assets;
using Mona.SDK.Core.Assets.Interfaces;
using Mona.SDK.Core.Body;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Mona.SDK.Core.UIElements
{
    public class MonaAssetEditItemVisualElement : VisualElement
    {
        private IMonaAssetProvider _assets;
        private int _index;

        protected DropdownField _nameDropdown;

        protected TextField _nameField;

        protected ObjectField _objectField;
        protected ObjectField _audioField;
        protected ObjectField _animationField;

        protected Toggle _toggleField;

        public MonaAssetEditItemVisualElement()
        {
            style.flexDirection = FlexDirection.Row;
            style.width = Length.Percent(100);
            style.paddingBottom = 5;

            _nameDropdown = new DropdownField();
            _nameDropdown.style.width = 150;
            _nameDropdown.style.marginRight = 5;
            _nameDropdown.RegisterValueChangedCallback((evt) =>
            {
                _nameDropdown.value = evt.newValue;
                _assets.AllAssets[_index].PrefabId = evt.newValue;
                Refresh();
            });

            _nameField = new TextField();
            _nameField.RegisterValueChangedCallback((evt) =>
            {
                if (evt.newValue == null || evt.newValue.StartsWith("_")) return;
                if (_assets.AllAssets[_index].PrefabId != evt.newValue)
                {
                    _assets.AllAssets[_index].PrefabId = evt.newValue;
                }
            });

            var regex = new Regex("\\d+");
            _nameField.RegisterCallback<BlurEvent>((evt) =>
            {
                var count = _assets.AllAssets.FindAll(x => regex.Replace(x.PrefabId, "") == _nameField.value);
                count.Remove(_assets.AllAssets[_index]);
                if (count.Count > 0)
                {
                    _assets.AllAssets[_index].PrefabId = _nameField.value + count.Count.ToString("D2");
                    _nameField.value = _assets.AllAssets[_index].PrefabId;
                }
            });
            _nameField.style.width = 100;
            _nameField.style.marginRight = 5;

            _objectField = new ObjectField();
            _objectField.style.flexGrow = 1;
            _objectField.RegisterValueChangedCallback((evt) =>
            {
                ((IMonaBodyAssetItem)_assets.AllAssets[_index]).Value = (MonaBody)evt.newValue;
            });

            _audioField = new ObjectField();
            _audioField.style.flexGrow = 1;
            _audioField.RegisterValueChangedCallback((evt) =>
            {
                if (_assets.AllAssets[_index] is IMonaAudioAssetItem)
                    ((IMonaAudioAssetItem)_assets.AllAssets[_index]).Value = (AudioClip)evt.newValue;
            });

            _animationField = new ObjectField();
            _animationField.style.flexGrow = 1;
            _animationField.RegisterValueChangedCallback((evt) =>
            {
                ((IMonaAnimationAssetItem)_assets.AllAssets[_index]).Value = (AnimationClip)evt.newValue;
            });

        }

        public void CreateValue(string value)
        {
            var name = (_assets.AllAssets[_index] != null) ? _assets.AllAssets[_index].PrefabId : "Default";
            switch (value)
            {
                case MonaCoreConstants.BODY_TYPE_LABEL:
                    _assets.CreateAsset(name, typeof(MonaBodyAsset), _index);
                    break;
                case MonaCoreConstants.AUDIO_TYPE_LABEL:
                    _assets.CreateAsset(name, typeof(MonaAudioAsset), _index);
                    break;
                case MonaCoreConstants.ANIMATION_TYPE_LABEL:
                    _assets.CreateAsset(name, typeof(MonaAnimationAsset), _index);
                    break;
            }
            Refresh();
        }

        public virtual void Refresh()
        {
            Clear();

            _nameDropdown.choices = new List<string>();
            _nameDropdown.choices.Add("Custom Name");
            _nameDropdown.choices.AddRange(_assets.DefaultNames);
            _nameDropdown.value = (_nameDropdown.choices.FindIndex(x => x == _assets.AllAssets[_index].PrefabId) == -1) ? "Custom Name" : _assets.AllAssets[_index].PrefabId;
            Add(_nameDropdown);

            var value = _assets.AllAssets[_index];

            if (_nameDropdown.value == "Custom Name")
            {
                Add(_nameField);
                _nameField.value = value.PrefabId;
            }

            if (value is IMonaBodyAssetItem)
            {
                Add(_objectField);
                _objectField.objectType = typeof(MonaBody);
                _objectField.value = (MonaBody)((IMonaBodyAssetItem)value).Value;
            }
            else if (value is IMonaAudioAssetItem)
            {            
                Add(_audioField);
                _audioField.objectType = typeof(AudioClip);
                _audioField.value = ((IMonaAudioAssetItem)value).Value;
                _audioField.SetEnabled(true);
            }
            else if (value is IMonaAnimationAssetItem)
            {
                Add(_animationField);
                _animationField.objectType = typeof(AnimationClip);
                _animationField.value = ((IMonaAnimationAssetItem)value).Value;
            }
        }

        public void SetAssetItem(IMonaAssetProvider assets, int idx)
        {
            _assets = assets;
            _index = idx;
            Refresh();
        }

        public IMonaAssetItem GetAssetItem()
        {
            return _assets.AllAssets[_index];
        }
    }
}