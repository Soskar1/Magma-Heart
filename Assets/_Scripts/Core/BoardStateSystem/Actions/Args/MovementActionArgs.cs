using MagmaHeart.Core.BoardStateSystem.Actions.Args;
using MagmaHeart.Core.BoardStateSystem.Actions.Input;

namespace MagmaHeart.Core.BoardStateSystem.Actions
{
    public record MovementActionArgs(TargetPositionActionInput TargetPositionInput, int Speed) : MagmaHeartActionArgs(TargetPositionInput);
}
