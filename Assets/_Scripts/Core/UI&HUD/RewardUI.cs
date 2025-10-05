using MagmaHeart.Core.StateMachines;
using UnityEngine;

namespace MagmaHeart.Core.UI
{
    public class RewardUI : MonoBehaviour, IDisplayable, ICombatStateListener
    {
        [SerializeField] private GameObject m_visual;
        
        public void Show() => m_visual.SetActive(true);
        public void Hide() => m_visual.SetActive(false);

        public void EnterCombatState() { }
        public void ExitCombatState() => Show();
    }
}