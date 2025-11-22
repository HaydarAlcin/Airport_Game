using UnityEngine;
using DEV.Scripts.Signals;
using UnityEngine.SceneManagement;

namespace DEV.Scripts.Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        private uint _totalMoney = 100;

        public uint TotalMoney => _totalMoney;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
                Destroy(gameObject);

            Application.targetFrameRate = 60;

            _totalMoney = 100;
        }

        public uint Withdraw(uint want)
        {
            uint take = want > _totalMoney ? _totalMoney : want;
            _totalMoney -= take;

            EventBus.CoreGameSignals.OnMoneySpent?.Invoke(_totalMoney);
            return take;
        }

        public void AddMoney(uint v)
        {
            _totalMoney += v;
            EventBus.CoreGameSignals.OnMoneyEarned?.Invoke(_totalMoney);
        }

        public void ReloadGameScene()
        {
            _totalMoney = 100;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}