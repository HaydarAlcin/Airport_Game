using UnityEngine;
using System.Collections.Generic;

namespace DEV.Scripts.Passenger
{
    public class PassengerStackController : MonoBehaviour
    {
        [Header("Suitcase Stack References")]
        [SerializeField] private Transform suitcaseParent;

        private PassengerManager _manager;

        private GameObject _suitcasePrefab;
        private List<Transform> _suitcaseList = new List<Transform>();

        private uint _suitcaseCount;
        private uint _maxSuitcaseCount;

        private float _suitcaseOffsetY;

        internal void RemoveSuitcase(Transform item)
        {
            _suitcaseCount--;
            _suitcaseList.Remove(item);

            if (_suitcaseCount <= 0) _manager.SetPassengerCarryAnimation(false);
        }

        internal List<Transform> GetAllSuitcases() => _suitcaseList;

        internal void SetManager(PassengerManager passengerManager) => _manager = passengerManager;

        internal void SetSettings(GameObject suitcasePrefab,uint suitcaseCount, float offsetY)
        {
            _suitcasePrefab = suitcasePrefab;
            _maxSuitcaseCount = suitcaseCount;
            _suitcaseOffsetY = offsetY;
            _suitcaseList.Clear();
            SpawnSuitcases();
        }

        private void SpawnSuitcases()
        {
            for (int i = 0; i < _maxSuitcaseCount; i++)
            {
                _suitcaseCount++;
                Transform suitcaseTransform = Instantiate(_suitcasePrefab, suitcaseParent).transform;
                suitcaseTransform.localPosition = new Vector3(0, i * _suitcaseOffsetY, 0);
                AddSuitcase(suitcaseTransform);
            }
            _manager?.SetPassengerCarryAnimation(true);
        }

        private void AddSuitcase(Transform item)
        {
            _suitcaseCount++;
            _suitcaseList.Add(item);
        }
    }
}