using System;
using UnityEngine;

namespace Mona
{
    public partial class MonaSystem : MonoBehaviour
    {
        public IMonaSystem instance;

        public void ShowCursor(bool isVisible)
        {
            instance.ShowOlympiaUI(isVisible);
        }
    }
}
