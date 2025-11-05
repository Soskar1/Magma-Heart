using System;
using System.Collections;
using MagmaHeart.Core.Dungeon;
using UnityEngine;

namespace MagmaHeart.Core.Entities.NonPlayableCharacters
{
    public class Enemy : Entity
    {
        private Room m_currentRoom;

        private Vector3Int m_currentTilePosition; // TODO: Use MovementAction

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

        public void StartCombat(Room room)
        {
            m_currentRoom = room;
            m_currentTilePosition = m_currentRoom.GetRoomTile(transform.position).Position;
        }

        private IEnumerator MakingThinkingMove()
        {
            yield return new WaitForSeconds(1);
            CombatController.EndTurn();
        }
    }
}