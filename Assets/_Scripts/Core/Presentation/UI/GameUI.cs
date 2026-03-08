using MagmaHeart.Abilities;
using MagmaHeart.Core.Artifacts;
using MagmaHeart.Core.Artifacts.Presentation;
using MagmaHeart.Core.CombatSystem;
using MagmaHeart.Core.CombatSystem.Presenters;
using MagmaHeart.Core.Entities;
using MagmaHeart.Core.Entities.PlayableCharacters;
using MagmaHeart.Core.Entities.Presenters;
using MagmaHeart.Core.Statistics;
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
        [SerializeField] private TurnOrderPresenter m_battleTurnOrder;
        [SerializeField] private GameOverUI m_gaveOverUI;
        [SerializeField] private WelcomeScreen m_welcomeScreen;
        [SerializeField] private CompletedRoomsCounterPresenter m_roomCounterPresenter;
        [SerializeField] private AvailableAbilitiesWindow m_availableAbilitiesWindow;

        public RewardUI RewardUI => m_rewardUI;
        public WelcomeScreen WelcomeScreen => m_welcomeScreen;

        private Battle m_battle;

        public void Initialize(
            Entity player,
            Battle battle,
            PlayerTurnController playerTurnController,
            IGameWorld gameWorld,
            CompletedRoomsCounter roomCounter,
            Inventory inventory)
        {
            m_healthBar.Register(player.Health);
            m_endTurnButton.Initialize(playerTurnController);
            m_energyHUD.Initialize(player);
            m_entityInfoUI.Initialize(battle);
            m_battleTurnOrder.Initialize(battle);
            m_gaveOverUI.Initialize(battle, roomCounter);
            m_roomCounterPresenter.Initialize(roomCounter);
            m_rewardUI.Initialize(inventory);
            m_availableAbilitiesWindow.Initialize(inventory, playerTurnController, player.Model.Id, gameWorld);

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