using MagmaHeart.Core.CombatSystem;
using MagmaHeart.Core.Entities.PlayableCharacters;
using MagmaHeart.Core.Entities.Presenters;
using UnityEngine;

namespace MagmaHeart.Core.UI
{
    public class GameUI : MonoBehaviour, ITurnSwitchListener
    {
        [SerializeField] private HealthPresenter m_healthBar;
        [SerializeField] private EndTurnButton m_endTurnButton;
        [SerializeField] private EnergyPresenter m_energyHUD;
        [SerializeField] private RewardUI m_rewardUI;

        public HealthPresenter HealthBar => m_healthBar;
        public RewardUI RewardUI => m_rewardUI;

        public void Initialize(Player player)
        {
            m_healthBar.Initialize(player);
            m_endTurnButton.Initialize(player);
            m_energyHUD.Initialize(player);
        }

        public void HandleOnTurnSwitched(object obj, OnTurnSwitchedEventArgs args)
        {
            if (args.Entity.Model.IsPlayer)
            {
                m_energyHUD.Show();
                m_endTurnButton.Show();
            }
            else
            {
                m_energyHUD.Hide();
                m_endTurnButton.Hide();
            }
        }

        public void HandleOnBattleEnded(object obj, OnBattleEndedEventArgs args)
        {
            m_endTurnButton.Hide();
            m_energyHUD.Hide();
        }
    }
}