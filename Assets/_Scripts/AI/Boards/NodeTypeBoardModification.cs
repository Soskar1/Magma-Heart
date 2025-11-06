using MagmaHeart.AI.Pathifinding;
using UnityEngine;

namespace MagmaHeart.AI.Boards
{
    public record NodeTypeBoardModification(Vector2 Position, BoardNodeType Type) : BoardModification;
}
