using MagmaHeart.Core.Artifacts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MagmaHeart.Core.UI
{
    public class RewardCard : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_nameText;
        [SerializeField] private TextMeshProUGUI m_rarityText;
        [SerializeField] private TextMeshProUGUI m_descriptionText;
        [SerializeField] private Image m_image;

        public void Display(ArtifactData data)
        {
            m_nameText.text = data.Name;
            m_rarityText.text = data.Rarity.ToString();
            m_descriptionText.text = data.Description;
            m_image.sprite = data.Sprite;
        }
    }
}

