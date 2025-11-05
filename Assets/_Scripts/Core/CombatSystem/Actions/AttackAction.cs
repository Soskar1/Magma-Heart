using MagmaHeart.AI.Reasoning;
using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.Entities;
using MagmaHeart.Core.Entities.Properties;
using System;
using UnityEngine;

namespace MagmaHeart.Core.CombatSystem
{
    public class AttackAction : MagmaHeart.AI.Action
    {
        public const int ENERGY_COST = 2;
        public const int ATTACK_DISTANCE = 1;
        public const int ATTACK_DAMAGE = 1;

        private readonly Energy m_energy;
        private readonly Entity m_entity;

        public Entity EntityToHit { get; set; }

        public EventHandler<OnAttackEventArgs> OnAttackTriggered;

        public AttackAction(Entity actionPossessor) : base(actionPossessor.Model)
        {
            m_entity = actionPossessor;
            m_energy = actionPossessor.Model.Energy;
        }

        public override bool CanSimulate(StateSnapshot state, AIUnit target)
        {
            EnergyProperty energy = state.GetProperty<EnergyProperty>(ActionPossessor);

            if (energy.CurrentEnergy < ENERGY_COST)
                return false;

            PositionProperty possessorPosition = state.GetProperty<PositionProperty>(ActionPossessor);
            PositionProperty targetPosition = state.GetProperty<PositionProperty>(target);

            if (possessorPosition.ManhattanDistance(targetPosition) != ATTACK_DISTANCE)
                return false;

            return true;
        }

        public override StateSnapshot Simulate(StateSnapshot state, AIUnit target)
        {
            StateSnapshot newState = base.Simulate(state, target);

            EnergyProperty currentEnergy = state.GetProperty<EnergyProperty>(ActionPossessor);
            EnergyProperty newEnergy = new EnergyProperty(currentEnergy.CurrentEnergy - ENERGY_COST);
            newState.Update(ActionPossessor, newEnergy);

            HealthProperty targetHealth = state.GetProperty<HealthProperty>(target);
            HealthProperty newHealth = new HealthProperty(targetHealth.CurrentHealth - ATTACK_DAMAGE, targetHealth.MaxHealth);
            newState.Update(target, newHealth);

            if (newHealth.CurrentHealth <= 0)
            {
                IsAliveProperty isAliveProperty = new IsAliveProperty(false);
                newState.Update(target, isAliveProperty);
            }

            return newState;
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

        public bool CanAttack(Entity entity)
        {
            if (!m_energy.HasEnough(ENERGY_COST))
            {
                Debug.Log("Not enough energy to attack");
                return false;
            }

            int distance = DungeonGrid.ManhattanDistance(m_entity.CurrentTilePosition, entity.CurrentTilePosition);
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