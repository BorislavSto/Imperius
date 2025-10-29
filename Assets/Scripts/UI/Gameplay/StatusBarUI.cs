using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class StatusBarUI : MonoBehaviour
    {
        [Header("UI References")] 
        [SerializeField] private Image statusBarImage;

        public int FillAmount {get ; private set;}
        public bool IsSetUp {get ; private set;}
        
        private int maxFillAmount;
        private bool doesRecharge;
        private int rechargeAmount;
        private float rechargeRate;
        private float rechargeTimer;

        private void Update()
        {
            if (doesRecharge)
            {
                if (FillAmount >= maxFillAmount)
                    FillAmount = maxFillAmount;
                
                if (rechargeRate <= 0)
                    return;
                
                rechargeTimer += Time.deltaTime;
                if (rechargeTimer >= rechargeRate)
                {
                    FillAmount += rechargeAmount;
                    rechargeTimer = 0f;
                    
                    
                    if (FillAmount > maxFillAmount)
                        FillAmount = maxFillAmount;
                }
                
                float currentFill = (float)FillAmount / maxFillAmount;
                statusBarImage.fillAmount = currentFill;
            }
        }

        public void SetFillAmount(int amount)
        {
            FillAmount = amount;
            float currentFill = (float)FillAmount / maxFillAmount;
            statusBarImage.fillAmount = currentFill;
        }

        public void SetStats(int maxFill, bool recharges = false, float rate = 0, int amount = 0)
        {
            maxFillAmount = maxFill;
            FillAmount = maxFill;
            doesRecharge = recharges;
            rechargeRate = rate;
            rechargeAmount = amount;
            IsSetUp = true;
            SetFillAmount(FillAmount);
        }
    }
}