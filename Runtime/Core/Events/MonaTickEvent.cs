namespace Mona.SDK.Core.Events
{
    public struct MonaTickEvent
    {
        public float DeltaTime;

        public MonaTickEvent(float deltaTime)
        {
            DeltaTime = deltaTime;
        }

    }
}