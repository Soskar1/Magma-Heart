using MagmaHeart.AI.Actions;
using MagmaHeart.Core.BoardStateSystem.Actions.Args;
using MagmaHeart.Core.BoardStateSystem.Actions.Data;
using MagmaHeart.Core.BoardStateSystem.Actions.Input;

namespace MagmaHeart.Core.BoardStateSystem.Actions
{
    public record MovementActionArgs(TargetPositionActionInput TargetPositionInput, MovementActionData MovementActionData) : MagmaHeartActionArgs(TargetPositionInput);
}
