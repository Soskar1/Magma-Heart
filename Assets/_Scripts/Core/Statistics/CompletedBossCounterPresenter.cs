using TMPro;
using UnityEngine;

namespace MagmaHeart.Core.Statistics
{
    public class CompletedBossCounterPresenter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_textField;
        private CompletedBossCounter m_counter;

        public void Initialize(CompletedBossCounter counter)
        {
            m_counter = counter;
            m_counter.OnCompleteBossCounterChanged += HandleOnCompletedRoomsCounterChanged;
        }

        private void OnDisable() => m_counter.OnCompleteBossCounterChanged -= HandleOnCompletedRoomsCounterChanged;

        private void HandleOnCompletedRoomsCounterChanged(object _, int args) => m_textField.text = args.ToString();
    }
}
