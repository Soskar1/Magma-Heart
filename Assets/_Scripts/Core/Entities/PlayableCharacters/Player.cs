using System;
using MagmaHeart.Core.BoardStateSystem.Actions;
using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.Input;
using UnityEngine;

namespace MagmaHeart.Core.Entities.PlayableCharacters
{
    public class Player : Entity
    {
        public PlayerController Controller { get; private set; }
        public PlayerCombatController CombatController { get; private set; }

        public event Action<Collider2D> OnTriggerEnter;
        public event Action<Collider2D> OnTriggerExit;

        public void Initialize(UserInput userInput, MouseListener mouseListener, RoomGrid grid, IActionPreviewService previewService)
        {
            base.Initialize(grid, true);

            PlayerTurnContext turnContext = new PlayerTurnContext(Model);
            TurnContext = turnContext;
            Controller = new PlayerController(this, userInput);
            CombatController = new PlayerCombatController(turnContext, mouseListener, previewService);
        }

        private void Update() => Animation.PlayAnimations();
        private void FixedUpdate() => Controller.Update();

        private void OnTriggerEnter2D(Collider2D collision) => OnTriggerEnter?.Invoke(collision);

        private void OnTriggerExit2D(Collider2D collision) => OnTriggerExit?.Invoke(collision);
    }
}