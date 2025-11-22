using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DEV.Scripts.Zones.MoneyZone
{
    internal class MoneyZoneVisualController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Image fillImage;
        [SerializeField] private ParticleSystem moneyParticle;
        [SerializeField] private TextMeshPro costText;

        [SerializeField] private GameObject openSign;

        internal void SetFillAmount(float fillAmount) => fillImage.fillAmount = fillAmount;

        internal void SetCostText(uint cost) => costText?.SetText(cost.ToString());

        internal void SetOpenSignActive(bool active) => openSign.SetActive(active);

        internal void PlayMoneyParticle()
        {
            //if (!moneyParticle.isPlaying)
            //    moneyParticle.Play();

        }

        internal void StopMoneyParticle()
        {
            //if (moneyParticle.isPlaying)
            //    moneyParticle.Stop();
        }
    }
}