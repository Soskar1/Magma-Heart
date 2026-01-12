using TMPro;
using UnityEngine;

namespace MagmaHeart.UIWindowPopupSystem
{
    public class WindowPresenter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_header;
        [SerializeField] private TextMeshProUGUI m_content;

        public void Initialize(WindowData windowData)
        {
            m_header.text = windowData.Header;
            m_content.text = windowData.Content;
        }
    }
}