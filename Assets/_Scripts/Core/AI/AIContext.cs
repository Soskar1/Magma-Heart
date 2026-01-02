using MagmaHeart.AI.Actions;
using MagmaHeart.Core.Entities.NonPlayableCharacters;

namespace MagmaHeart.Core.AI
{
    public record AIContext(ActionDatabase ActionDatabase, CombatAI CombatAI);
}
