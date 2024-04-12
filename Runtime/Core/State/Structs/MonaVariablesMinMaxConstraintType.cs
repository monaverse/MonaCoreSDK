
namespace Mona.SDK.Core.State.Structs
{
    [System.Serializable]
    public enum MinMaxConstraintType
    {
        None = 0,
        ConstrainToBounds = 10,
        Loop = 20,
        Bounce = 30,
        ReturnToDefault = 40
    }


    [System.Serializable]
    public enum NumberRoundingType
    {
        None = 0,
        RoundClosest = 10,
        RoundUp = 20,
        RoundDown = 30
    }
}
