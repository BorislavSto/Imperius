using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class Popup : MonoBehaviour
    {
        [SerializeField] private TMP_Text messageText;
        [SerializeField] private Transform buttonContainer;
        [SerializeField] private GameObject buttonPrefab;

        private readonly Action escapeAction;

        private Popup()
        {
            escapeAction = ClosePopup;
        }

        private void Awake()
        {
            UIManager.Instance.PushEscapeAction(escapeAction);
        }

        private void OnDestroy()
        {
            UIManager.Instance.PopEscapeAction(escapeAction);
        }

        public void SetMessage(string message)
        {
            messageText.text = message;
        }

        public void SetButtons((string, Action)[] buttons)
        {
            foreach (Transform child in buttonContainer)
                Destroy(child.gameObject);

            foreach (var (label, action) in buttons)
            {
                var btnObj = Instantiate(buttonPrefab, buttonContainer);
                var btn = btnObj.GetComponent<Button>();
                btnObj.GetComponentInChildren<TMP_Text>().text = label;
                btn.onClick.AddListener(() => action?.Invoke());
                btn.onClick.AddListener(ClosePopup);

                // TODO: Doesn't work as intended for now as the buttons are made in runtime so the navigation has to be automatic,
                // which then goes to buttons it shouldn't go to, for now the first button is selected and will stay on it with no navigation possible
                if (buttonContainer.childCount == 1)
                {
                    UIManager.Instance.SetSelectedUIElement(btnObj);
                    Debug.Log("Set selected to popup button" + label);
                }
            }
        }
        
        private void ClosePopup()
        {
            UIManager.Instance.ShowPreviouslySelectedUIElement();
            Destroy(gameObject);
        }
    }
}