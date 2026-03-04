using System.Collections.Generic;
using System.Linq;
using System.Threading;
using MagmaHeart.Abilities;
using MagmaHeart.Abilities.Effects;
using MagmaHeart.AI;
using MagmaHeart.AI.Reasoning;
using MagmaHeart.Collections;
using MagmaHeart.Core.Abilities;
using MagmaHeart.Core.AI;
using MagmaHeart.Core.CombatSystem;
using UnityEditor;

namespace MagmaHeart.Core.Tests
{
    internal record AIScenario(TestGameWorld World, CircularList<AIUnitModel> TurnOrder)
    {
        public IList<AbilityPlan> RunAI(int depth, ParameterDatabase parameters, EffectDispatcher dispatcher)
        {
            AggressiveStrategy strategy = AssetDatabase.LoadAssetAtPath<AggressiveStrategy>("Assets/Data/AI/AggressiveStrategy.asset");

            IStartOfTurnEffectFactory factory = new StartOfTurnEffectFactory(parameters.Energy, 2);
            AIEngine ai = new AIEngine(strategy, depth, factory, dispatcher);

            using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            IReadOnlyList<AbilityEffect> effects = factory.CreateStartOfTurnEffects(World, TurnOrder.Head.Id);
            foreach (AbilityEffect effect in effects)
                dispatcher.Apply(World, effect);

            CircularList<int> modelTurns = new CircularList<int>();
            foreach (AIUnitModel entity in TurnOrder)
                modelTurns.Add(entity.Id);

            IEnumerable<AbilityPlan> result = ai.ChooseBestMove(modelTurns, World.Board);

            return result is null ? null : result.ToList();
        }
    }
}
