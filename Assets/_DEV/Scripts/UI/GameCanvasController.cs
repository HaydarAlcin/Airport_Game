using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DEV.Scripts.Enums;
using DEV.Scripts.Player;
using DEV.Scripts.Signals;
using DEV.Scripts.Managers;

namespace DEV.Scripts.UI
{
    public class GameCanvasController : MonoBehaviour
    {
        [Space(20)]
        [Header("PAINT BOARD CONTROLLER")]
        [SerializeField] private PaintBoard paintBoardController;

        [Header("PAINT BOARD UI")]
        [SerializeField] private RectTransform paintPanel;
        [Space]
        [SerializeField] private Slider paintSlider;
        [Space]
        [SerializeField] private Button paintColorButton1;
        [SerializeField] private Button paintColorButton2;
        [SerializeField] private Button paintColorButton3;
        [Space]
        [SerializeField] private TextMeshProUGUI paintPercentText;

        [Space(20)]
        [Header("JOYSTICK")]
        [SerializeField] private RectTransform joystickPanel;

        [Header("MONEY")]
        [SerializeField] private RectTransform moneyUIElement;

        [SerializeField] private Button reloadSceneBttn;

        private void Start()
        {
            reloadSceneBttn.onClick.RemoveAllListeners();
            reloadSceneBttn.onClick.AddListener(() => GameManager.Instance.ReloadGameScene());
        }

        private void OnEnable() => EventBus.CoreGameSignals.OnMoneyZoneFinished += OnMoneyZoneFinished;

        private void OnDisable() => EventBus.CoreGameSignals.OnMoneyZoneFinished -= OnMoneyZoneFinished;

        private void OnMoneyZoneFinished(MoneyZoneUnlockType type)
        {
            if (!(type == MoneyZoneUnlockType.OpenPainting)) return;

            SetButtonListeners();
            SetSliderListener();
            SetPercentTxt();

            paintPanel.gameObject.SetActive(true);
            joystickPanel.gameObject.SetActive(false);
            moneyUIElement.gameObject.SetActive(true);

            PlayerManager player = EventBus.CorePlayerSignals.PlayerManager?.Invoke();
            player?.SetCanMove(false);
        }

        private void SetButtonListeners()
        {
            paintColorButton1.onClick.RemoveAllListeners();
            paintColorButton1.onClick.AddListener(() => paintBoardController.SelectColor(0));
            paintColorButton2.onClick.RemoveAllListeners();
            paintColorButton2.onClick.AddListener(() => paintBoardController.SelectColor(1));
            paintColorButton3.onClick.RemoveAllListeners();
            paintColorButton3.onClick.AddListener(() => paintBoardController.SelectColor(2));
        }

        private void SetPercentTxt() => paintBoardController.SetPercentTxt(paintPercentText);

        private void SetSliderListener()
        {
            paintSlider.onValueChanged.RemoveAllListeners();
            paintSlider.onValueChanged.AddListener((value) => paintBoardController.ChangeBrushRadius(value));
        }
    }
}