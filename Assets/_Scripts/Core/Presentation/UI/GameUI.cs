using MagmaHeart.Core.CombatSystem;
using MagmaHeart.Core.Entities.PlayableCharacters;
using MagmaHeart.Core.Entities.Presenters;
using UnityEngine;

namespace MagmaHeart.Core.Presentation.UI
{
    public class GameUI : MonoBehaviour
    {
        [SerializeField] private HealthPresenter m_healthBar;
        [SerializeField] private EndTurnButton m_endTurnButton;
        [SerializeField] private EnergyPresenter m_energyHUD;
        [SerializeField] private RewardUI m_rewardUI;
        [SerializeField] private EntityInfoUI m_entityInfoUI;

        public RewardUI RewardUI => m_rewardUI;

        private Battle m_battle;

        public void Initialize(Player player, Battle battle, MouseHover mouseHover)
        {
            m_healthBar.Register(player.Health);
            m_endTurnButton.Initialize(player);
            m_energyHUD.Initialize(player);
            m_entityInfoUI.Initialize(mouseHover);

            m_battle = battle;
            m_battle.OnTurnSwitched += HandleOnTurnSwitched;
            m_battle.OnBattleEnded += HandleOnBattleEnded;
        }

        private void OnDisable()
        {
            m_battle.OnTurnSwitched -= HandleOnTurnSwitched;
            m_battle.OnBattleEnded -= HandleOnBattleEnded;
        }

        private void HandleOnTurnSwitched(object obj, OnTurnSwitchedEventArgs args)
        {
            if (args.CurrentEntity.Model.IsPlayer)
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

        private void HandleOnBattleEnded(object obj, OnBattleEndedEventArgs args)
        {
            m_endTurnButton.Hide();
            m_energyHUD.Hide();
        }
    }
}