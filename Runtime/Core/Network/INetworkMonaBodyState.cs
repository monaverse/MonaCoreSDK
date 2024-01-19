namespace Mona
{
    public interface INetworkMonaBodyState
    {
        void SetInt(string variableName, int value);
        void SetFloat(string variableName, float value);
        void SetBool(string variableName, bool value);
    }
}