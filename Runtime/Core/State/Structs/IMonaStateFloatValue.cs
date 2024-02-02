namespace Mona.SDK.Core.State.Structs
{
    public interface IMonaStateFloatValue
    { 
        float Value { get; set; }
        float DefaultValue { get; set; }
        bool UseMinMax { get; set; }
        MinMaxConstraintType MinMaxType { get; set; }
        float Min { get; set; }
        float Max { get; set; }
    }
}