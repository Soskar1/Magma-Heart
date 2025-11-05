using MagmaHeart.Collections;
using MagmaHeart.Core.Entities;
using System;
using System.Collections.Generic;

namespace MagmaHeart.Core.CombatSystem
{
    public class TurnSwitcher
    {
        public CircularList<Entity> TurnOrder { get; init; }

        private Entity m_currentEntity;

        public event EventHandler<OnTurnSwitchedEventArgs> OnTurnSwitched;

        public TurnSwitcher() => TurnOrder = new CircularList<Entity>();

        public void Start(IEnumerable<Entity> entities)
        {
            TurnOrder.Clear();
            TurnOrder.AddRange(entities);

            m_currentEntity = TurnOrder.Head;
            m_currentEntity.CombatController.NextTurn += NextTurn;
            StartTurn();
        }

        private void NextTurn(object obj, EventArgs e)
        {
            m_currentEntity.CombatController.NextTurn -= NextTurn;
            m_currentEntity = TurnOrder.Next();
            m_currentEntity.CombatController.NextTurn += NextTurn;
            StartTurn();
        }

        private void StartTurn()
        {
            OnTurnSwitchedEventArgs args = new OnTurnSwitchedEventArgs(m_currentEntity);
            OnTurnSwitched?.Invoke(this, args);

            m_currentEntity.CombatController.StartTurn();
        }

        public void Clear()
        {
            foreach (Entity entity in TurnOrder)
            {
                entity.CombatController.NextTurn -= NextTurn;
                entity.CombatController.EndTurn();
            }

            TurnOrder.Clear();
        }
    }
}