using System;
using MagmaHeart.Core.Artifacts;
using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.Input;
using MagmaHeart.Core.StateMachines;
using MagmaHeart.Core.UI;
using UnityEngine;

namespace MagmaHeart.Core.Entities.PlayableCharacters
{
    public class Player : Entity, IActionStateListener, ICombatStateListener, IRewardStateListener
    {
        private Inventory m_inventory;
        private RewardUI m_rewardUI;

        public event Action<Collider2D> OnTriggerEnter;
        public event Action<Collider2D> OnTriggerExit;

        private IPlayerBehaviour m_currentBehaviour;
        private ActionPlayerBehaviour m_actionBehaviour;
        private CombatPlayerBehaviour m_combatBehaviour;
        private RewardPlayerBehaviour m_rewardPlayerBehaviour;

        public void Initialize(ActionUserInput actionUserInput, CombatUserInput turnBasedUserInput, GameUI gameUI, DungeonGrid grid)
        {
            base.Initialize(grid, true);

            CombatController = new PlayerCombatController(Model, gameUI, turnBasedUserInput);

            m_inventory = new Inventory(Model);
            m_rewardUI = gameUI.RewardUI;
            m_rewardUI.OnRewardPicked += HandleOnRewardPicked;

            m_actionBehaviour = new ActionPlayerBehaviour(this, actionUserInput);
            m_combatBehaviour = new CombatPlayerBehaviour(this, turnBasedUserInput);
            m_rewardPlayerBehaviour = new RewardPlayerBehaviour(actionUserInput.UserInput, Animation);
            m_currentBehaviour = m_actionBehaviour;
        }

        public void OnDisable() => m_rewardUI.OnRewardPicked -= HandleOnRewardPicked;

        public void Enable() => m_currentBehaviour.Enable();
        public void Disable() => m_currentBehaviour.Disable();

        public void EnterActionState() => SwitchState(m_actionBehaviour);
        public void ExitActionState() { }

        public void EnterCombatState() => SwitchState(m_combatBehaviour);
        public void ExitCombatState() { }

        public void EnterRewardState() => SwitchState(m_rewardPlayerBehaviour);
        public void ExitRewardState() { }

        private void SwitchState(IPlayerBehaviour newState)
        {
            m_currentBehaviour.Disable();
            m_currentBehaviour = newState;
            m_currentBehaviour.Enable();
        }

        private void Update() => Animation.PlayAnimations();
        private void FixedUpdate() => m_currentBehaviour.Update();

        private void OnTriggerEnter2D(Collider2D collision) => OnTriggerEnter?.Invoke(collision);

        private void OnTriggerExit2D(Collider2D collision) => OnTriggerExit?.Invoke(collision);

        private void HandleOnRewardPicked(object obj, OnRewardPickedArgs args) => m_inventory.Pick(args.ArtifactData);
    }
}