using System;
using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using DEV.Scripts.Enums;
using DEV.Scripts.Signals;

namespace DEV.Scripts.Listener
{
    internal class MoneyZoneUnlockVisualListener : MonoBehaviour
    {
        [Header("Filter")]
        [SerializeField] private MoneyZoneUnlockType targetType;

        [Header("Items")]
        [SerializeField] private List<RevealItem> items = new();

        [Header("Animation")]
        [SerializeField, Min(0.01f)] private float scaleDuration = 0.25f;
        [SerializeField] private Ease scaleEase = Ease.OutBack;
        [SerializeField] private bool resetScaleOnEnable = true;

        private Dictionary<Transform,Vector3> defaultScales = new Dictionary<Transform,Vector3>();

        private void OnEnable() => EventBus.CoreGameSignals.OnMoneyZoneFinished += OnMoneyZoneFinished;

        private void Awake()
        {
            if (resetScaleOnEnable)
            {
                foreach (var item in items)
                {
                    if (!item.target) continue;
                    item.target.SetActive(false);
                    defaultScales[item.target.transform] = item.target.transform.localScale;
                    item.target.transform.localScale = Vector3.zero;
                }
            }
        }

        private void OnDisable() => EventBus.CoreGameSignals.OnMoneyZoneFinished -= OnMoneyZoneFinished;

        private void OnMoneyZoneFinished(MoneyZoneUnlockType type)
        {
            if (type != targetType) return;

            foreach (var entry in items)
            {
                if (!entry.target) continue;

                var t = entry.target.transform;

                DOVirtual.DelayedCall(entry.delay, () =>
                {
                    if (!t) return;

                    entry.target.SetActive(true);
                    t.localScale = Vector3.zero;

                    Vector3 targetScale = defaultScales.ContainsKey(t) ? defaultScales[t] : Vector3.one;

                    t.DOScale(targetScale, scaleDuration)
                     .SetEase(scaleEase);
                }, ignoreTimeScale: true);
            }
        }

        [Serializable]
        private struct RevealItem
        {
            public GameObject target;
            [Min(0f)] public float delay;
        }
    }
}