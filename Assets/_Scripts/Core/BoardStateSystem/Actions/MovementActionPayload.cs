using MagmaHeart.AI.Actions;

namespace MagmaHeart.BoardStateSystem.Actions
{
    public record MovementActionPayload(int MovementDistanceInTilesForOneEnergy) : ActionPayload;
}
