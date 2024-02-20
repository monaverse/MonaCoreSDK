using UnityEngine;

namespace Mona.SDK.Core.EasyUI
{
    public class EasyUIScreenDefinitions : MonoBehaviour
    {

        [SerializeField] private EasyUIScreenZone[] _screenZones;

        public void PlaceElementInHUD(IEasyUINumericalDisplay variable)
        {
            foreach (EasyUIScreenZone screenzone in _screenZones)
            {
                if (screenzone.Placement == variable.ScreenPosition)
                {
                    screenzone.AddVariable(variable);
                }
            }
        }
    }
}
