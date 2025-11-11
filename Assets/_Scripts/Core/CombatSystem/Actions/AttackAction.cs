using MagmaHeart.AI;
using MagmaHeart.AI.Actions;
using MagmaHeart.AI.Boards;
using MagmaHeart.AI.States;
using MagmaHeart.Core.Dungeon;
using MagmaHeart.Core.Entities;
using MagmaHeart.Core.Entities.Properties;
using System;
using UnityEngine;

namespace MagmaHeart.Core.CombatSystem
{
    public class AttackAction : MagmaHeart.AI.Actions.Action<AttackActionArgs>
    {
        public const int ENERGY_COST = 2;
        public const int ATTACK_DISTANCE = 1;
        public const int ATTACK_DAMAGE = 1;

        private readonly Energy m_energy;
        private readonly Entity m_entity;

        public event EventHandler<OnAttackEventArgs> OnAttackTriggered;

        public AttackAction(Entity actionPossessor) : base(actionPossessor.Model)
        {
            m_entity = actionPossessor;
            m_energy = actionPossessor.Model.Energy;
        }

        public override ActionArgs CreateActionArgs(StateSnapshot state, AIUnit unit) => new AttackActionArgs(unit);

        public override bool CanSimulate(StateSnapshot state, SimulatedBoard board, AttackActionArgs args)
        {
            EnergyPropertySnapshot energy = state.GetProperty<EnergyPropertySnapshot>(ActionPossessor);

            if (energy.CurrentEnergy < ENERGY_COST)
                return false;

            PositionPropertySnapshot possessorPosition = state.GetProperty<PositionPropertySnapshot>(ActionPossessor);
            PositionPropertySnapshot targetPosition = state.GetProperty<PositionPropertySnapshot>(args.Target);

            if (possessorPosition.ManhattanDistance(targetPosition) != ATTACK_DISTANCE)
                return false;

            return true;
        }

        public override StateSnapshot Simulate(StateSnapshot state, SimulatedBoard board, AttackActionArgs args)
        {
            StateSnapshot newState = base.Simulate(state, board, args);

            EnergyPropertySnapshot currentEnergy = state.GetProperty<EnergyPropertySnapshot>(ActionPossessor);
            EnergyPropertySnapshot newEnergy = new EnergyPropertySnapshot(currentEnergy.CurrentEnergy - ENERGY_COST);
            newState.Update(ActionPossessor, newEnergy);

            HealthPropertySnapshot targetHealth = state.GetProperty<HealthPropertySnapshot>(args.Target);
            HealthPropertySnapshot newHealth = new HealthPropertySnapshot(targetHealth.CurrentHealth - ATTACK_DAMAGE, targetHealth.MaxHealth);
            newState.Update(args.Target, newHealth);

            if (newHealth.CurrentHealth <= 0)
            {
                IsAlivePropertySnapshot isAliveProperty = new IsAlivePropertySnapshot(false);
                newState.Update(args.Target, isAliveProperty);
            }

            return newState;
        }

        public override void Execute(AttackActionArgs args)
        {
            EntityModel model = (EntityModel)args.Target;

            if (CanAttack(model))
            {
                m_energy.Spend(ENERGY_COST);

                OnAttackEventArgs attackArgs = new OnAttackEventArgs(model);
                OnAttackTriggered?.Invoke(this, attackArgs);
                if (OnAttackTriggered == null)
                {
                    Debug.LogWarning("No one is subscribed to the OnAttackTriggered event. Executing Hit()");
                    Hit(model);
                }
            }
        }

        public bool CanAttack(EntityModel model)
        {
            if (!m_energy.HasEnough(ENERGY_COST))
            {
                Debug.Log("Not enough energy to attack");
                return false;
            }

            int distance = DungeonGrid.ManhattanDistance(m_entity.Model.GetCurrentTilePosition(), model.GetCurrentTilePosition());
            if (distance > ATTACK_DISTANCE)
            {
                Debug.Log($"Distance between entities: {distance}. Can't attack");
                return false;
            }

            return true;
        }

        public void Hit(EntityModel entity) => entity.Health.TakeDamage(ATTACK_DAMAGE);

        public override bool CanSimulate(StateSnapshot state, SimulatedBoard board, ActionArgs args) => CanSimulate(state, board, (AttackActionArgs)args);
        public override void Execute(ActionArgs args) => Execute((AttackActionArgs)args);
    }
}