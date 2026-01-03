using MagmaHeart.AI.Actions;
using MagmaHeart.AI.Reasoning;
using MagmaHeart.Core.Entities;

namespace MagmaHeart.Core.AI
{
    public record AIContext(ActionDatabase ActionDatabase, AIEngine aiEngine, MagmaHeartTurnContext TurnContext);
}
