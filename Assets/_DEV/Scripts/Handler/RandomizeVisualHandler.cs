using UnityEngine;

namespace DEV.Scripts.Handler
{
    public class RandomizeVisualHandler : MonoBehaviour
    {
        [SerializeField] private Renderer visualMeshRenderer;

        private void Start() => UpdateVisual();

        private void UpdateVisual()
        {
            if (!visualMeshRenderer) return;

            float tileX = Mathf.Round(Random.Range(0f, 1f) * 10f) / 10f;
            float tileY = Mathf.Round(Random.Range(0f, 1f) * 10f) / 10f;

            var materials = visualMeshRenderer.materials;

            foreach (var mat in materials)
                mat.mainTextureScale = new Vector2(tileX, tileY);
        }
    }
}