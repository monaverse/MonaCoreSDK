namespace Mona.SDK.Core.Events
{
    public struct MonaFixedTickEvent
    {
        public float DeltaTime;

        public MonaFixedTickEvent(float deltaTime)
        {
            DeltaTime = deltaTime;
        }

    }
}