namespace Mona.SDK.Core.Events
{
    public struct MonaBodyFixedTickEvent
    {
        public float DeltaTime;

        public MonaBodyFixedTickEvent(float deltaTime)
        {
            DeltaTime = deltaTime;
        }
        
    }
}