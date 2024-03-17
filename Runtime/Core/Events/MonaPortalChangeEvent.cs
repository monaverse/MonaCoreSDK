namespace Mona.SDK.Brains.Core.Events
{
    public struct MonaPortalChangeEvent
    {
        public string PortalId;
        public MonaPortalChangeEvent(string portalId)
        {
            PortalId = portalId;
        }
    }
}