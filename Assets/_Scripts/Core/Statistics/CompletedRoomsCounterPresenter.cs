using TMPro;
using UnityEngine;

namespace MagmaHeart.Core.Statistics
{
    public class CompletedRoomsCounterPresenter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_textField;
        private CompletedRoomsCounter m_counter;
        
        public void Initialize(CompletedRoomsCounter counter)
        {
            m_counter = counter;
            m_counter.OnCompletedRoomsCounterChanged += HandleOnCompletedRoomsCounterChanged;
        }

        private void OnDisable() => m_counter.OnCompletedRoomsCounterChanged -= HandleOnCompletedRoomsCounterChanged;

        private void HandleOnCompletedRoomsCounterChanged(object _, OnCompletedRoomsCounterChangedEventArgs args) => m_textField.text = $"Room: {args.CompletedRooms}";
    }
}

