
using Mona.SDK.Core.Assets;
using Mona.SDK.Core.Assets.Interfaces;
using Mona.SDK.Core.Body;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
#if UNITY_EDITOR
using UnityEditor.UIElements;
#endif
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
#if UNITY_EDITOR
        protected ObjectField _bodyField;
        protected ObjectField _wearableField;
        protected ObjectField _avatarField;
        protected ObjectField _audioField;
        protected ObjectField _textureField;
        protected ObjectField _materialField;
        protected ObjectField _animationField;
#endif
        private Toggle _bodyUseUrlField;
        private TextField _bodyUrlField;
        private Toggle _avatarUseUrlField;
        private TextField _avatarUrlField;
        private IntegerField _animationLayer;
        private FloatField _animationLayerWeight;

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

#if UNITY_EDITOR
            _bodyField = new ObjectField();
            _bodyField.style.flexGrow = 1;
            _bodyField.objectType = typeof(MonaBody);
            _bodyField.RegisterValueChangedCallback((evt) =>
            {
                ((IMonaBodyAssetItem)_assets.AllAssets[_index]).Value = (MonaBody)evt.newValue;
            });

            _bodyUrlField = new TextField();
            _bodyUrlField.style.flexGrow = 1;
            _bodyUrlField.RegisterValueChangedCallback((evt) =>
            {
                ((IMonaBodyAssetItem)_assets.AllAssets[_index]).Url = (string)evt.newValue;
            });

            _bodyUseUrlField = new Toggle();
            _bodyUseUrlField.RegisterValueChangedCallback((evt) =>
            {
                DisplayBodyFields(evt.newValue);
            });

            _avatarField = new ObjectField();
            _avatarField.style.flexGrow = 1;
            _avatarField.objectType = typeof(GameObject);
            _avatarField.RegisterValueChangedCallback((evt) =>
            {
                ((IMonaAvatarAssetItem)_assets.AllAssets[_index]).Value = (GameObject)evt.newValue;
            });

            _avatarUrlField = new TextField();
            _avatarUrlField.style.flexGrow = 1;
            _avatarUrlField.RegisterValueChangedCallback((evt) =>
            {
                ((IMonaAvatarAssetItem)_assets.AllAssets[_index]).Url = (string)evt.newValue;
            });

            _avatarUseUrlField = new Toggle();
            _avatarUseUrlField.RegisterValueChangedCallback((evt) =>
            {
                DisplayAvatarFields(evt.newValue);
            });

            _wearableField = new ObjectField();
            _wearableField.style.flexGrow = 1;
            _wearableField.RegisterValueChangedCallback((evt) =>
            {
                ((IMonaWearableAssetItem)_assets.AllAssets[_index]).Value = (GameObject)evt.newValue;
            });

            _audioField = new ObjectField();
            _audioField.style.flexGrow = 1;
            _audioField.RegisterValueChangedCallback((evt) =>
            {
                if (_assets.AllAssets[_index] is IMonaAudioAssetItem)
                    ((IMonaAudioAssetItem)_assets.AllAssets[_index]).Value = (AudioClip)evt.newValue;
            });

            _materialField = new ObjectField();
            _materialField.style.flexGrow = 1;
            _materialField.RegisterValueChangedCallback((evt) =>
            {
                if (_assets.AllAssets[_index] is IMonaMaterialAssetItem)
                    ((IMonaMaterialAssetItem)_assets.AllAssets[_index]).Value = (Material)evt.newValue;
            });

            _textureField = new ObjectField();
            _textureField.style.flexGrow = 1;
            _textureField.RegisterValueChangedCallback((evt) =>
            {
                if (_assets.AllAssets[_index] is IMonaTextureAssetItem)
                    ((IMonaTextureAssetItem)_assets.AllAssets[_index]).Value = (Texture)evt.newValue;
            });

            _animationField = new ObjectField();
            _animationField.style.flexGrow = 1;
            _animationField.RegisterValueChangedCallback((evt) =>
            {
                ((IMonaAnimationAssetItem)_assets.AllAssets[_index]).Value = (AnimationClip)evt.newValue;
            });

            _animationLayer = new IntegerField();
            _animationLayer.style.width = 30;
            _animationLayer.tooltip = "Layer";
            _animationLayer.RegisterValueChangedCallback((evt) =>
            {
                ((IMonaAnimationAssetItem)_assets.AllAssets[_index]).Layer = (int)evt.newValue;
            });

            _animationLayerWeight = new FloatField();
            _animationLayerWeight.style.width = 30;
            _animationLayerWeight.tooltip = "Layer Weight";
            _animationLayerWeight.RegisterValueChangedCallback((evt) =>
            {
                ((IMonaAnimationAssetItem)_assets.AllAssets[_index]).LayerWeight = (float)evt.newValue;
            });
#endif

        }

        private void DisplayAvatarFields(bool useUrl)
        {
#if UNITY_EDITOR
            if(_avatarField.parent == this) Remove(_avatarField);
            if (_avatarUrlField.parent == this) Remove(_avatarUrlField);
            if(useUrl)
            {
                Add(_avatarUrlField);
            }
            else
            {
                Add(_avatarField);
            }
#endif
        }

        private void DisplayBodyFields(bool useUrl)
        {
#if UNITY_EDITOR
            if (_bodyField.parent == this) Remove(_bodyField);
            if (_bodyUrlField.parent == this) Remove(_bodyUrlField);
            if (useUrl)
            {
                Add(_bodyUrlField);
            }
            else
            {
                Add(_bodyField);
            }
#endif
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

#if UNITY_EDITOR
            if (value is IMonaBodyAssetItem)
            {
                Add(_bodyUseUrlField);
                DisplayBodyFields(_bodyUseUrlField.value);
                _bodyUseUrlField.value = !string.IsNullOrEmpty(((IMonaBodyAssetItem)value).Url);
                _bodyField.value = ((IMonaBodyAssetItem)value).Value;
                _bodyUrlField.value = ((IMonaBodyAssetItem)value).Url;
            }
            else if (value is IMonaAvatarAssetItem)
            {
                Add(_avatarUseUrlField);
                DisplayAvatarFields(_avatarUseUrlField.value);
                _avatarUseUrlField.value = !string.IsNullOrEmpty(((IMonaAvatarAssetItem)value).Url);
                _avatarField.value = ((IMonaAvatarAssetItem)value).Value;
                _avatarUrlField.value = ((IMonaAvatarAssetItem)value).Url;
            }
            else if (value is IMonaWearableAssetItem)
            {
                Add(_wearableField);
                _wearableField.objectType = typeof(GameObject);
                _wearableField.value = (GameObject)((IMonaWearableAssetItem)value).Value;
            }
            else if (value is IMonaAudioAssetItem)
            {            
                Add(_audioField);
                _audioField.objectType = typeof(AudioClip);
                _audioField.value = ((IMonaAudioAssetItem)value).Value;
                _audioField.SetEnabled(true);
            }
            else if (value is IMonaMaterialAssetItem)
            {
                Add(_materialField);
                _materialField.objectType = typeof(Material);
                _materialField.value = ((IMonaMaterialAssetItem)value).Value;
                _materialField.SetEnabled(true);
            }
            else if (value is IMonaTextureAssetItem)
            {
                Add(_textureField);
                _textureField.objectType = typeof(Texture);
                _textureField.value = ((IMonaTextureAssetItem)value).Value;
                _textureField.SetEnabled(true);
            }
            else if (value is IMonaAnimationAssetItem)
            {
                Add(_animationField);
                _animationField.objectType = typeof(AnimationClip);
                _animationField.value = ((IMonaAnimationAssetItem)value).Value;

                Add(_animationLayer);
                _animationLayer.value = ((IMonaAnimationAssetItem)value).Layer;

                Add(_animationLayerWeight);
                _animationLayerWeight.value = ((IMonaAnimationAssetItem)value).LayerWeight;
            }
#endif
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