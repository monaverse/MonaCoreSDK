namespace Mona.SDK.Core.State.Structs
{
    public interface IMonaVariablesFloatValue
    {
        float Value { get; set; }
        float ValueToReturnFromTile { get; }
        float DefaultValue { get; set; }
        NumberRoundingType RoundingType { get; set; }
        MinMaxConstraintType MinMaxType { get; set; }
        bool UseMinMax { get; }
        float Min { get; set; }
        float Max { get; set; }
        bool ReturnRandomValueFromMinMax { get; set; }
        bool UseRandomSeed { get; set; }
        string RandomSeed { get; set; }

        void ForceMinMax(float min, float max);
        void ResetRandom();
    }
}