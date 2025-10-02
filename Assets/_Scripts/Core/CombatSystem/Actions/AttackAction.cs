using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.Entities;
using System;
using UnityEngine;

namespace MagmaHeart.Core.CombatSystem
{
    public class AttackAction : ICombatAction
    {
        public const int ENERGY_COST = 2;
        public const int ATTACK_DISTANCE = 1;
        public const int ATTACK_DAMAGE = 1;

        private Energy m_energy;
        private ITilePosition m_tilePosition;

        public IHittableTile EntityToHit { get; set; }

        public EventHandler<OnAttackEventArgs> OnAttackTriggered;

        public AttackAction(Energy energy, ITilePosition tilePosition)
        {
            m_tilePosition = tilePosition;
            m_energy = energy;
        }

        public void Execute()
        {
            if (CanAttack(EntityToHit))
            {
                m_energy.Spend(ENERGY_COST);

                if (OnAttackTriggered == null)
                {
                    Debug.LogWarning("No one is subscribed to the OnAttackTriggered event. Executing Hit()");
                    Hit();
                }
                else
                {
                    OnAttackEventArgs attackArgs = new OnAttackEventArgs(EntityToHit.CurrentTilePosition);
                    OnAttackTriggered?.Invoke(this, attackArgs);
                }
            }
        }

        public bool CanAttack(IHittableTile hittable)
        {
            if (!m_energy.HasEnough(ENERGY_COST))
            {
                Debug.Log("Not enough energy to attack");
                return false;
            }

            int distance = DungeonGrid.ManhattanDistance(m_tilePosition.CurrentTilePosition, hittable.CurrentTilePosition);
            if (distance > ATTACK_DISTANCE)
            {
                Debug.Log($"Distance between entities: {distance}. Can't attack");
                return false;
            }

            return true;
        }

        public void Hit() => EntityToHit.Hit(ATTACK_DAMAGE);
    }
}