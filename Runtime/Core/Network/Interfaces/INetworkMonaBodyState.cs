namespace Mona.SDK.Core.Network.Interfaces
{
    public interface INetworkMonaBodyState
    {
        void SetInt(string variableName, int value);
        void SetFloat(string variableName, float value);
        void SetBool(string variableName, bool value);
    }
}