using System;
using System.Collections;
using MagmaHeart.Core.Dungeon;
using UnityEngine;

namespace MagmaHeart.Core.Entities.NonPlayableCharacters
{
    public class Enemy : Entity
    {
        public void Initialize(DungeonGrid grid)
        {
            base.Initialize(grid, false);
            CombatController.OnTurnStarted += HandleOnTurnStarted;
            CombatController.OnTurnEnded += HandleOnTurnEnded;
        }

        public void OnDisable()
        {
            CombatController.OnTurnStarted -= HandleOnTurnStarted;
            CombatController.OnTurnEnded -= HandleOnTurnEnded;
        }

        public void HandleOnTurnStarted(object obj, EventArgs args)
        {
            Debug.Log($"{gameObject.name} ({gameObject.transform.position}) is doing a move");
            StartCoroutine(MakingThinkingMove());
        }

        public void HandleOnTurnEnded(object obj, EventArgs args)
        {
            Debug.Log($"{gameObject.name} ({gameObject.transform.position}) ended his move");
        }

        private IEnumerator MakingThinkingMove()
        {
            yield return new WaitForSeconds(1);
            CombatController.EndTurn();
        }
    }
}