using MagmaHeart.AI.Actions;
using MagmaHeart.Core.BoardStateSystem.Actions.Input;

namespace MagmaHeart.Core.BoardStateSystem.Actions.Args
{
    public record MagmaHeartActionArgs(MagmaHeartActionInput TypedInput) : ActionArgs(TypedInput);
}
