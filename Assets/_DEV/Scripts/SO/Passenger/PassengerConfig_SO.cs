using UnityEngine;
using DEV.Scripts.Enums;
using DG.Tweening;

namespace DEV.Scripts.SO.Passenger
{
    [CreateAssetMenu(fileName = "PassengerConfig_SO", menuName = "ScriptableObjects/Passenger/PassengerConfig_SO")]
    public class PassengerConfig_SO : ScriptableObject
    {
        [Header("Passenger")]
        [SerializeField] private GameObject passengerPrefab;

        [Header("Passenger First State")]
        [SerializeField] private PassengerStatus firstState = PassengerStatus.Entering;

        [Header("Passenger Count")]
        [SerializeField] private uint passengerCount = 6;

        [Space]
        [Header("Path")]
        [SerializeField] private Ease moveEase = Ease.Linear;
        [SerializeField] private float rotateEaseSeconds = 0.3f;
        [SerializeField] private float walkSpeed = 2f;
        [SerializeField] private float runSpeed = 4f;

        [Space]
        [Header("Suitcase")]
        [SerializeField] private GameObject suitcasePrefab;
        [SerializeField] private uint pricePerSuitcase = 50;
        [SerializeField] private uint maxSuitcaseCount = 3;
        [SerializeField] private uint minSuitcaseCount = 1;
        [SerializeField] private float suitcaseOffsetY = 0.3f;


        public GameObject Passennger => passengerPrefab;
        public PassengerStatus FirstState => firstState;

        public uint PassengerCount => passengerCount;

        public Ease MoveEase => moveEase;
        public float RotateEaseSeconds => rotateEaseSeconds;
        public float WalkSpeed => walkSpeed;
        public float RunSpeed => runSpeed;

        public uint PricePerSuitcase => pricePerSuitcase;
        public uint MaxSuitcaseCount => maxSuitcaseCount;
        public uint MinSuitcaseCount => minSuitcaseCount;
        public float SuitcaseOffsetY => suitcaseOffsetY;
        public GameObject SuitcasePrefab => suitcasePrefab;

        public uint GetRandomSuitcaseCount()
        {
            return (uint)Random.Range((int)minSuitcaseCount, (int)maxSuitcaseCount + 1);
        }
    }
}