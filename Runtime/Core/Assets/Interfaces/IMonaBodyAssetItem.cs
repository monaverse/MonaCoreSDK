using Mona.SDK.Core.Body;

namespace Mona.SDK.Core.Assets.Interfaces
{
    public interface IMonaBodyAssetItem : IMonaAssetItem
    {
        public MonaBody Value { get; set; }
        public string Url { get; set; }
    }
}