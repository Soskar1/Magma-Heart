using UnityEngine;

namespace MagmaHeart.UIWindowPopupSystem
{
    [System.Serializable]
    public struct WindowData
    {
        [SerializeField] private string m_header;
        [SerializeField] [TextArea(3, 5)] private string m_content;

        public string Header => m_header;
        public string Content => m_content;

        public WindowData(string header, string content)
        {
            m_header = header;
            m_content = content;
        }
    }
}