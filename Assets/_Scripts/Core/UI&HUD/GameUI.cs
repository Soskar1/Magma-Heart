using MagmaHeart.Core.CombatSystem;
using MagmaHeart.Core.Entities.Presenters;
using UnityEngine;

namespace MagmaHeart.Core.UI
{
    public class GameUI : MonoBehaviour, ITurnSwitchListener
    {
        [SerializeField] private HealthPresenter m_healthBar;
        [SerializeField] private CombatUI m_combatUI;
        [SerializeField] private EnergyPresenter m_energyHUD;
        [SerializeField] private RewardUI m_rewardUI;

        public HealthPresenter HealthBar => m_healthBar;
        public CombatUI CombatUI => m_combatUI;
        public EnergyPresenter EnergyHUD => m_energyHUD;
        public RewardUI RewardUI => m_rewardUI;

        public void Initialize(Player player)
        {
            m_healthBar.Initialize(player);
            m_combatUI.Initialize(player);
            m_energyHUD.Initialize(player);
        }

        public void HandleOnTurnSwitched(object obj, OnTurnSwitchedEventArgs args)
        {
            if (args.Entity.Model.IsPlayer)
            {
                m_energyHUD.Show();
                m_combatUI.Show();
            }
            else
            {
                m_energyHUD.Hide();
                m_combatUI.Hide();
            }
        }

        public void HandleOnBattleEnded(object obj, OnBattleEndedEventArgs args)
        {
            m_combatUI.Hide();
            m_energyHUD.Hide();
        }
    }
}