using System;
using System.Collections;
using MagmaHeart.Core.CombatSystem;
using MagmaHeart.Core.Dungeon;
using UnityEngine;

namespace MagmaHeart.Core.Entities.NonPlayableCharacters
{
    public class Enemy : MonoBehaviour, ICombatController
    {
        [SerializeField] private EntityData m_data;
        private Room m_currentRoom;

        private Entity m_controllingEntity;
        public Entity ControllingEntity => m_controllingEntity;

        public Action NextTurn { get; set; }
        public bool IsPlayableCharacter => false;

        private Vector3Int m_currentTilePosition; // TODO: Use MovementAction
        public Vector3Int CurrentTilePosition => m_currentTilePosition;

        public void Initialize()
        {
            m_controllingEntity = new Entity(m_data, transform);
        }

        public void StartCombat(Room room)
        {
            m_currentRoom = room;
            m_currentTilePosition = m_currentRoom.GetTilePosition(transform.position);
        }

        public void StartTurn()
        {
            Debug.Log($"{gameObject.name} ({gameObject.transform.position}) is doing a move");
            StartCoroutine(MakingThinkingMove());
        }

        public void EndTurn()
        {
            Debug.Log($"{gameObject.name} ({gameObject.transform.position}) ended his move");
            NextTurn?.Invoke();
        }

        private IEnumerator MakingThinkingMove()
        {
            yield return new WaitForSeconds(1);
            EndTurn();
        }
    }
}