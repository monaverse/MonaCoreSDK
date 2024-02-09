using Mona.SDK.Core.Assets;
using Mona.SDK.Core.Assets.Interfaces;
using UnityEngine.UIElements;

namespace Mona.SDK.Core.UIElements
{
    public class MonaAssetsVisualElement : VisualElement
    {
        private IMonaAssetProvider _monaAssets;
        private ListView _monaAssetListView;

        public MonaAssetsVisualElement()
        {
            _monaAssetListView = new ListView(null, 20, () => new MonaAssetEditItemVisualElement(), (elem, i) => ((MonaAssetEditItemVisualElement)elem).SetAssetItem(_monaAssets, i));
            _monaAssetListView.showFoldoutHeader = true;
            _monaAssetListView.headerTitle = GetHeader();
            _monaAssetListView.showAddRemoveFooter = true;
            _monaAssetListView.reorderMode = ListViewReorderMode.Animated;
            _monaAssetListView.reorderable = true;
            _monaAssetListView.itemsAdded += (elems) =>
            {
                foreach (var e in elems)
                {
                    _monaAssets.AllAssets[e] = CreateValue();
                }
            };
            Add(_monaAssetListView);
        }

        protected virtual string GetHeader() => "Mona Prefab Assets";

        protected virtual IMonaAssetItem CreateValue()
        {
            return (IMonaAssetItem)new MonaBodyAsset();
        }

        public void SetMonaAssets(IMonaAssetProvider monaAssets)
        {
            _monaAssets = monaAssets;
            _monaAssetListView.itemsSource = _monaAssets.AllAssets;
            _monaAssetListView.Rebuild();
        }

    }
}