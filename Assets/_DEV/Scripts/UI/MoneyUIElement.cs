using TMPro;
using UnityEngine;
using DEV.Scripts.Signals;
using DEV.Scripts.Managers;

namespace DEV.Scripts.UI
{
    public class MoneyUIElement : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI moneyText;

        private void OnEnable()
        {
            EventBus.CoreGameSignals.OnMoneyEarned += UpdateMoneyUI;
            EventBus.CoreGameSignals.OnMoneySpent += UpdateMoneyUI;
        }

        private void Start() => UpdateMoneyUI(GameManager.Instance.TotalMoney);

        private void OnDisable()
        {
            EventBus.CoreGameSignals.OnMoneyEarned -= UpdateMoneyUI;
            EventBus.CoreGameSignals.OnMoneySpent -= UpdateMoneyUI;
        }

        private void UpdateMoneyUI(uint value) => moneyText?.SetText(value.ToString());
    }
}