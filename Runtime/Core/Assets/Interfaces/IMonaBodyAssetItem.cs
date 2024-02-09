using Mona.SDK.Core.Body;

namespace Mona.SDK.Core.Assets.Interfaces
{
    public interface IMonaBodyAssetItem : IMonaAssetItem
    {
        public IMonaBody Value { get; set; }
    }
}