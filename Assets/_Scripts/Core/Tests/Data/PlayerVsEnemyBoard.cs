using MagmaHeart.AI.Boards;
using MagmaHeart.Core.Entities;

namespace MagmaHeart.Core.Tests
{
    internal record PlayerVsEnemyBoard(Board Board, EntityModel Player, EntityModel Enemy);
}