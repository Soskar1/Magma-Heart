using System;
using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.Input;
using UnityEngine;

namespace MagmaHeart.Core.Entities.PlayableCharacters
{
    public class Player : Entity
    {
        public PlayerController Controller { get; private set; }

        public event Action<Collider2D> OnTriggerEnter;
        public event Action<Collider2D> OnTriggerExit;

        public void Initialize(UserInput userInput, MouseListener mouseListener, DungeonGrid grid)
        {
            base.Initialize(grid, true);

            TurnContext = new PlayerTurnContext(Model, mouseListener);
            Controller = new PlayerController(this, userInput);
        }

        private void Update() => Animation.PlayAnimations();
        private void FixedUpdate() => Controller.Update();

        private void OnTriggerEnter2D(Collider2D collision) => OnTriggerEnter?.Invoke(collision);

        private void OnTriggerExit2D(Collider2D collision) => OnTriggerExit?.Invoke(collision);
    }
}