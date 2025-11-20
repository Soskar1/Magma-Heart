using MagmaHeart.Core.BoardStateSystem.Actions;
using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.Entities.CombatSystem;
using System;
using UnityEngine;

namespace MagmaHeart.Core.Entities.NonPlayableCharacters
{
    public class Enemy : Entity
    {
        public void Initialize(DungeonGrid grid, CombatAI ai)
        {
            base.Initialize(grid, false);

            CombatController = new EnemyCombatController(this, ai);

            TurnBasedMovement.OnMovementStarted += HandleOnMovementStarted;
            TurnBasedMovement.OnMovementEnded += HandleOnMovementEnded;
        }

        private void Update() => Animation.PlayAnimations();

        private void OnDisable()
        {
            TurnBasedMovement.OnMovementStarted -= HandleOnMovementStarted;
            TurnBasedMovement.OnMovementEnded -= HandleOnMovementEnded;
        }

        private void HandleOnMovementStarted(object obj, OnMovementEventArgs e)
        {
            Animation.PlayRunAnimation();

            Facing.TryUpdateFacing((e.To - e.From).x);
        }

        private void HandleOnMovementEnded(object obj, OnMovementEventArgs e)
        {
            Animation.PlayIdleAnimation();

            // TODO: Consider enemy AI for more complex behavior. Need to add ActionEndedEvent to properly handle this
            Debug.Log($"{gameObject.name} {transform.position} is ending it's move after the movement action");
            CombatController.EndTurn();
        }

        private void HandleOnAttackAnimationEnded(object obj, EventArgs e)
        {
            // TODO: Consider enemy AI for more complex behavior. Need to add ActionEndedEvent to properly handle this
            Debug.Log($"{gameObject.name} {transform.position} is ending it's move after the attack action");
            CombatController.EndTurn();
        }
    }
}