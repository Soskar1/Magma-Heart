using System;
using System.Collections;
using MagmaHeart.Core.CombatSystem;
using MagmaHeart.Core.Dungeon;
using UnityEngine;

namespace MagmaHeart.Core.Entities.NonPlayableCharacters
{
    public class Enemy : MonoBehaviour, IEntity, ICombatController
    {
        [SerializeField] private EntityData m_data;
        private Room m_currentRoom;

        private Entity m_controllingEntity;
        public Entity ControllingEntity => m_controllingEntity;

        public Action NextTurn { get; set; }

        public void Initialize()
        {
            m_controllingEntity = new Entity(m_data);
        }

        public void StartCombat(Room room)
        {
            m_currentRoom = room;
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
            yield return new WaitForSeconds(4);
            EndTurn();
        }
    }
}