using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
namespace DEV.Scripts.UI
{
    public class PaintBoard : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        [Header("UI")]
        [SerializeField] private RawImage canvasImage;

        [Header("Canvas")]
        [SerializeField] private int width = 512;
        [SerializeField] private int height = 512;
        [SerializeField] private Color32 background = new Color32(255, 255, 255, 255);

        [SerializeField]
        private Color32[] palette = {
        new Color32(255, 80, 80, 255),
        new Color32( 80,200,120, 255),
        new Color32( 80,140,255, 255)
        };


        private TextMeshProUGUI _percentText;
        
        private int _brushRadius = 16;

        private Texture2D _tex;
        private Color32[] _pixels;
        private RectTransform _rt;
        private bool _dirty, _painting;
        private Vector2 _lastUV;

        private int _currentColorIndex;
        private int _paintedCount;
        private int _totalPixels;

        private void Awake()
        {
            _rt = canvasImage.rectTransform;

            _tex = new Texture2D(width, height, TextureFormat.RGBA32, false, false);
            _tex.wrapMode = TextureWrapMode.Clamp;
            _tex.filterMode = FilterMode.Bilinear;

            _totalPixels = width * height;
            _pixels = new Color32[_totalPixels];
            for (int i = 0; i < _totalPixels; i++) _pixels[i] = background;

            _paintedCount = 0;
            _tex.SetPixels32(_pixels);
            _tex.Apply(false, false);
            canvasImage.texture = _tex;

            _currentColorIndex = 0;
            UpdatePercentLabel();
        }

        public void SelectColor(int index)
        {
            if (index < 0 || index >= palette.Length) return;
            _currentColorIndex = index;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (TryGetUV(eventData, out var uv))
            {
                Stamp(uv);
                _lastUV = uv;
                _painting = true;
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!_painting) return;
            if (TryGetUV(eventData, out var uv))
            {
                DrawLine(_lastUV, uv);
                _lastUV = uv;
            }
        }

        public void OnPointerUp(PointerEventData eventData) => _painting = false;

        private void LateUpdate()
        {
            if (_dirty)
            {
                _tex.SetPixels32(_pixels);
                _tex.Apply(false, false);
                _dirty = false;
            }
        }

        private bool TryGetUV(PointerEventData evt, out Vector2 uv)
        {
            uv = default;
            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    _rt, evt.position, evt.pressEventCamera, out var local)) return false;

            var rect = _rt.rect;
            float u = (local.x - rect.x) / rect.width;
            float v = (local.y - rect.y) / rect.height;
            if (u < 0 || u > 1 || v < 0 || v > 1) return false;
            uv = new Vector2(u, v);
            return true;
        }

        private void DrawLine(Vector2 a, Vector2 b)
        {
            int x0 = Mathf.RoundToInt(a.x * (width - 1));
            int y0 = Mathf.RoundToInt(a.y * (height - 1));
            int x1 = Mathf.RoundToInt(b.x * (width - 1));
            int y1 = Mathf.RoundToInt(b.y * (height - 1));

            int dx = Mathf.Abs(x1 - x0), dy = Mathf.Abs(y1 - y0);
            int sx = x0 < x1 ? 1 : -1, sy = y0 < y1 ? 1 : -1;
            int err = dx - dy;

            while (true)
            {
                FillCircle(x0, y0, _brushRadius);
                if (x0 == x1 && y0 == y1) break;
                int e2 = 2 * err;
                if (e2 > -dy) { err -= dy; x0 += sx; }
                if (e2 < dx) { err += dx; y0 += sy; }
            }
            _dirty = true;
            UpdatePercentLabel();
        }

        private void Stamp(Vector2 uv)
        {
            int px = Mathf.RoundToInt(uv.x * (width - 1));
            int py = Mathf.RoundToInt(uv.y * (height - 1));
            FillCircle(px, py, _brushRadius);
            _dirty = true;
            UpdatePercentLabel();
        }

        private void FillCircle(int cx, int cy, int r)
        {
            int r2 = r * r;
            int xMin = Mathf.Max(cx - r, 0);
            int xMax = Mathf.Min(cx + r, width - 1);
            int yMin = Mathf.Max(cy - r, 0);
            int yMax = Mathf.Min(cy + r, height - 1);

            var col = palette[_currentColorIndex];

            for (int y = yMin; y <= yMax; y++)
            {
                int dy = y - cy;
                int dy2 = dy * dy;
                int row = y * width;

                for (int x = xMin; x <= xMax; x++)
                {
                    int dx = x - cx;
                    if (dx * dx + dy2 > r2) continue;

                    int idx = row + x;
                    if (ApproximatelyEqual(_pixels[idx], background) && !ApproximatelyEqual(col, background))
                        _paintedCount++;

                    _pixels[idx] = col;
                }
            }
        }

        private void UpdatePercentLabel()
        {
            if (!_percentText) return;
            float pct = (_paintedCount / (float)_totalPixels) * 100f;
            _percentText.SetText(Mathf.RoundToInt(pct) + "%");
        }

        private bool ApproximatelyEqual(Color32 a, Color32 b)
            => a.r == b.r && a.g == b.g && a.b == b.b && a.a == b.a;


        internal void ChangeBrushRadius(float newRadius) => _brushRadius = Mathf.RoundToInt(newRadius);

        internal void SetPercentTxt(TextMeshProUGUI paintPercentText) => _percentText = paintPercentText;
    }
}