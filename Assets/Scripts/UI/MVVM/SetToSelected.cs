using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SetToSelected : ViewUIElement
    {
        [SerializeField] private Selectable buttonToSetSelected;

        public override void UpdateUIElement()
        {
            UIManager.Instance.SetSelectedUIElement(buttonToSetSelected.gameObject);
        }

        public override void CleanupUIElement()
        {
            UIManager.Instance.ShowPreviouslySelectedUIElement();
        }
    }
}