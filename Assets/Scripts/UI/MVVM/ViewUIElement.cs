using UnityEngine;

namespace UI
{
    public abstract class ViewUIElement : MonoBehaviour
    {
        public abstract void UpdateUIElement();
        public abstract void CleanupUIElement();
    }
}