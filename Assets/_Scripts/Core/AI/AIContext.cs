using MagmaHeart.AI.Execution;
using MagmaHeart.AI.Reasoning;

namespace MagmaHeart.Core.AI
{
    public record AIContext(AIEngine AiEngine, IStartOfTurnCommandFactory StartOfTurnCommandFactory);
}
