using MagmaHeart.AI;
using MagmaHeart.AI.Reasoning;
using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.Entities;
using System;
using UnityEngine;

namespace MagmaHeart.Core.CombatSystem
{
    public class AttackAction : AI.Action
    {
        public const int ENERGY_COST = 2;
        public const int ATTACK_DISTANCE = 1;
        public const int ATTACK_DAMAGE = 1;

        private Energy m_energy;
        private ITilePosition m_tilePosition;

        public IHittableTile EntityToHit { get; set; }

        public EventHandler<OnAttackEventArgs> OnAttackTriggered;

        public AttackAction(Entity actionPossessor, ITilePosition tilePosition) : base(actionPossessor)
        {
            m_tilePosition = tilePosition;
            m_energy = actionPossessor.Energy;
        }

        public override bool CanSimulate(StateSnapshot state, AIUnit target)
        {
            throw new NotImplementedException();
        }

        public override StateSnapshot Simulate(StateSnapshot state, AIUnit target)
        {
            return base.Simulate(state, target);
        }

        public override void Execute()
        {
            if (CanAttack(EntityToHit))
            {
                m_energy.Spend(ENERGY_COST);

                OnAttackEventArgs attackArgs = new OnAttackEventArgs(EntityToHit.CurrentTilePosition);
                OnAttackTriggered?.Invoke(this, attackArgs);
                if (OnAttackTriggered == null)
                {
                    Debug.LogWarning("No one is subscribed to the OnAttackTriggered event. Executing Hit()");
                    Hit();
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