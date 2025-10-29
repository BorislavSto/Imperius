using System.Collections;
using Combat;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    // TODO: Make it into an actual button with click event and such
    public class AttackUISlot : MonoBehaviour
    {
        [Header("UI References")] 
        [SerializeField] private Image iconImage;
        [SerializeField] private Image cooldownOverlay;
        [SerializeField] private TextMeshProUGUI keybindText;
        [SerializeField] private TextMeshProUGUI cooldownText;

        [Header("UI References")] 
        [SerializeField] private Sprite defaultIcon;

        private AttackData attackData;
        private float maxCooldown;
        private float currentCooldown;
        private bool isOnCooldown;

        private void Awake()
        {
            if (cooldownOverlay != null)
                cooldownOverlay.fillAmount = 0f;

            if (cooldownText != null)
                cooldownText.gameObject.SetActive(false);
        }

        public void Initialize(AttackData data, int keybind)
        {
            attackData = data;

            if (iconImage != null)
                iconImage.sprite = data.icon;

            if (keybindText != null)
                keybindText.text = keybind.ToString();

            maxCooldown = data.cooldown;
            currentCooldown = 0f;

            UpdateVisuals();
        }

        private void UpdateVisuals()
        {
            bool onCooldown = currentCooldown > 0f;

            if (cooldownOverlay != null)
                cooldownOverlay.fillAmount = maxCooldown > 0 ? currentCooldown / maxCooldown : 0f;

            if (cooldownText != null)
            {
                cooldownText.gameObject.SetActive(onCooldown);
                if (onCooldown)
                    cooldownText.text = currentCooldown.ToString("F1");
            }
        }

        public void TriggerCooldown()
        {
            maxCooldown = attackData.cooldown;
            currentCooldown = maxCooldown;

            if (cooldownOverlay != null)
            {
                cooldownOverlay.fillAmount = 1f;
                cooldownOverlay.gameObject.SetActive(true);
            }

            if (cooldownText != null)
            {
                cooldownText.gameObject.SetActive(true);
                cooldownText.text = maxCooldown.ToString("F1");
            }

            StartCoroutine(CooldownRoutine());
        }

        private IEnumerator CooldownRoutine()
        {
            while (currentCooldown > 0f)
            {
                isOnCooldown = true;
                currentCooldown -= Time.deltaTime;

                if (currentCooldown < 0f)
                    currentCooldown = 0f;

                // Update visuals
                float progress = currentCooldown / maxCooldown;

                if (cooldownOverlay != null)
                    cooldownOverlay.fillAmount = progress;

                if (cooldownText != null)
                    cooldownText.text = Mathf.Ceil(currentCooldown).ToString();

                yield return null;
            }

            isOnCooldown = false;

            if (cooldownOverlay != null)
                cooldownOverlay.gameObject.SetActive(false);

            if (cooldownText != null)
                cooldownText.gameObject.SetActive(false);
        }

        public void Clear()
        {
            attackData = null;
            maxCooldown = 0f;
            currentCooldown = 0f;

            if (iconImage != null)
                iconImage.sprite = defaultIcon;

            if (cooldownOverlay != null)
                cooldownOverlay.fillAmount = 0f;

            if (cooldownText != null)
                cooldownText.gameObject.SetActive(false);

            if (keybindText != null)
                keybindText.text = string.Empty;
        }

        public bool IsOnCooldown()
        {
            return isOnCooldown;
        }

        public bool HasAttack(AttackData data)
        {
            return attackData == data;
        }
    }
}