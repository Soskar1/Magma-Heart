using MagmaHeart.AI.Boards;
using MagmaHeart.AI.States;
using System.Collections.Generic;
using System.Linq;

namespace MagmaHeart.AI.Actions
{
    public abstract class Action
    {
        public AIUnit ActionPossessor { get; }

        public Action(AIUnit actionPossessor) => ActionPossessor = actionPossessor;

        public abstract void Execute(ActionArgs args, GameState gameState);
        public abstract bool CanExecute(ActionArgs args, GameState gameState);
    }

    public abstract class Action<T> : Action where T : ActionArgs
    {
        public Action(AIUnit actionPossessor) : base(actionPossessor) { }

        public abstract void Execute(T args, GameState gameState);
        public override void Execute(ActionArgs args, GameState gameState) => Execute((T)args, gameState);

        public abstract void CanExecute(T args, GameState gameState);
        public override void CanExecute(ActionArgs args, GameState gameState) => CanExecute((T)args, gameState);
    }
}
