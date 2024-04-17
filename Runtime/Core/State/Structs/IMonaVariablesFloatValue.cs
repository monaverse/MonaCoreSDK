namespace Mona.SDK.Core.State.Structs
{
    public interface IMonaVariablesFloatValue
    {
        float Value { get; set; }
        float DefaultValue { get; set; }
        NumberRoundingType RoundingType { get; set; }
        MinMaxConstraintType MinMaxType { get; set; }
        bool UseMinMax { get; }
        float Min { get; set; }
        float Max { get; set; }
        bool ReturnRandomValueFromMinMax { get; set; }

        void ForceMinMax(float min, float max);
    }
}