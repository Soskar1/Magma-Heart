using MagmaHeart.AI.Actions;
using MagmaHeart.AI.Execution;
using MagmaHeart.AI.Reasoning;

namespace MagmaHeart.Core.AI
{
    public record AIContext(ActionDatabase ActionDatabase, AIEngine AiEngine, IStartOfTurnCommandFactory StartOfTurnCommandFactory);
}
