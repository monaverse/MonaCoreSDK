namespace Mona.SDK.Core.State.Structs
{
    public interface IMonaVariablesFloatValue
    { 
        float Value { get; set; }
        float DefaultValue { get; set; }
        MinMaxConstraintType MinMaxType { get; set; }
        bool UseMinMax { get; }
        float Min { get; set; }
        float Max { get; set; }
        bool ReturnRandomValueFromMinMax { get; set; }
    }
}