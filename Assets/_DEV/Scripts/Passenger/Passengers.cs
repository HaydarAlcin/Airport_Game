using UnityEngine;
using System.Collections.Generic;
using DEV.Scripts.SO.Passenger;
using DEV.Scripts.Signals;
using DEV.Scripts.Enums;

namespace DEV.Scripts.Passenger
{
    internal class Passengers : MonoBehaviour
    {
        public static Passengers Instance { get; private set; }

        [Header("Passenger Data")]
        [SerializeField] private PassengerConfig_SO configData;

        private List<PassengerManager> passengers = new List<PassengerManager>();

        public uint PassengerCount => configData.PassengerCount;
        public uint PricePerSuitcase => configData.PricePerSuitcase;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        private void OnEnable()
        {
            EventBus.CoreGameSignals.OnMoneyZoneFinished += ZoneFinished;
            passengers.Clear();
        }

        private void OnDisable() => EventBus.CoreGameSignals.OnMoneyZoneFinished -= ZoneFinished;

        private void SpawnPassengers()
        {
            uint count = configData.PassengerCount;
            for (int i = 0; i < count; i++)
            {
                GameObject passengerObj = Instantiate(configData.Passennger, transform.position, Quaternion.identity, transform);
                passengerObj.name = $"Passenger_{i}";
                PassengerManager passengerManager = passengerObj.GetComponent<PassengerManager>();
                if (passengerManager != null)
                {
                    passengerManager.SetSettings(configData);
                    passengers.Add(passengerManager);

                    passengerManager.SwitchState(configData.FirstState);
                }
            }
        }

        private void ZoneFinished(MoneyZoneUnlockType type)
        {
            if (type == MoneyZoneUnlockType.OpenAirplane)
            {
                passengers.Clear();
                SpawnPassengers();

                EventBus.CoreGameSignals.OnMoneyZoneFinished -= ZoneFinished;
            }
        }
    }
}