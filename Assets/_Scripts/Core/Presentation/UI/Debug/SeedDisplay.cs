using TMPro;
using UnityEngine;

namespace MagmaHeart.Core.Presentation.UI
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class SeedDisplay : MonoBehaviour
    {
        public void Initialize(int seed)
        {
            TextMeshProUGUI seedText = GetComponent<TextMeshProUGUI>();
            seedText.SetText($"Seed: {seed}");
        }
    }
}
