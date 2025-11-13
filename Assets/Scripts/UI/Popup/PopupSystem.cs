using System;
using UnityEngine;

namespace UI
{
    public class PopupSystem : MonoBehaviour
    {
        [SerializeField] private GameObject popupPrefab;
        [SerializeField] private Transform popupParent;

        public void ShowPopup(string message, (string, Action)[] buttons)
        {
            var popupInstance = Instantiate(popupPrefab, popupParent);
            var popup = popupInstance.GetComponent<Popup>();
            popup.SetMessage(message);
            popup.SetButtons(buttons);
        }
    }
}