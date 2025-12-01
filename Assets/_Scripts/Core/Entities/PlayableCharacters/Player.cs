using System;
using MagmaHeart.Core.Artifacts;
using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.Input;
using MagmaHeart.Core.StateMachines;
using MagmaHeart.Core.Presentation.UI;
using UnityEngine;

namespace MagmaHeart.Core.Entities.PlayableCharacters
{
    public class Player : Entity, IActionStateListener
    {
        private Inventory m_inventory;
        private RewardUI m_rewardUI;
        private PlayerController m_playerController;

        public event Action<Collider2D> OnTriggerEnter;
        public event Action<Collider2D> OnTriggerExit;

        public void Initialize(UserInput userInput, GameUI gameUI, DungeonGrid grid)
        {
            base.Initialize(grid, true);

            TurnContext = new PlayerTurnContext(Model, userInput);
            m_playerController = new PlayerController(this, userInput);

            m_inventory = new Inventory(Model);
            m_rewardUI = gameUI.RewardUI;
            m_rewardUI.OnRewardPicked += HandleOnRewardPicked;
        }

        public void OnDisable() => m_rewardUI.OnRewardPicked -= HandleOnRewardPicked;

        public void EnterActionState() => m_playerController.Enable();
        public void ExitActionState() => m_playerController.Disable();

        private void Update() => Animation.PlayAnimations();
        private void FixedUpdate() => m_playerController.Update();

        private void OnTriggerEnter2D(Collider2D collision) => OnTriggerEnter?.Invoke(collision);

        private void OnTriggerExit2D(Collider2D collision) => OnTriggerExit?.Invoke(collision);

        private void HandleOnRewardPicked(object obj, OnRewardPickedArgs args) => m_inventory.Pick(args.ArtifactData);
    }
}