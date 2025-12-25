using MagmaHeart.AI.Boards;
using MagmaHeart.Extensions;
using MagmaHeart.Core.Entities;
using UnityEngine;

namespace MagmaHeart.Core.Tests
{
    internal static class BoardBuilder
    {
        public static Board CreateEmptyBoard(BoardDimensions dimensions)
        {
            BoardGraph graph = new BoardGraph();
            for (int x = dimensions.StartPoint.x; x < dimensions.EndPoint.x; ++x)
            {
                for (int y = dimensions.StartPoint.y; y < dimensions.EndPoint.y; ++y)
                    graph.AddNode(new Vector2(x, y), BoardNodeType.Walkable);

                for (int y = dimensions.StartPoint.y; y < dimensions.EndPoint.y - 1; ++y)
                    graph.ConnectNodes(new Vector2(x, y), new Vector2(x, y + 1), 1);

                if (x > 0)
                    for (int y = dimensions.StartPoint.y; y < dimensions.EndPoint.y; ++y)
                        graph.ConnectNodes(new Vector2(x - 1, y), new Vector2(x, y), 1);
            }

            return new Board(graph);
        }

        public static PlayerVsEnemyBoard PlayerVsEnemy(BoardDimensions dimensions, EntityInitializationData player, EntityInitializationData enemy)
        {
            Board board = CreateEmptyBoard(dimensions);

            EntityModel playerModel = AddEntity(board, player);
            EntityModel enemyModel = AddEntity(board, enemy);

            return new PlayerVsEnemyBoard(board, playerModel, enemyModel);
        }

        public static EntityModel AddEntity(Board board, EntityInitializationData entityData)
        {
            EntityModel model = entityData.GetModel();
            board.AddUnit(entityData.Position.ToVector2(), model);
            board.ChangeNodeType(entityData.Position.ToVector2(), BoardNodeType.Obstacle);

            return model;
        }

        public static void CreateWall(Board board, Vector2 position) => board.ChangeNodeType(position, BoardNodeType.Obstacle);

        public static void SurroundWithWalls(Board board, EntityModel model)
        {
            Vector3Int position = model.GetCurrentTilePosition();

            for (int x = -1; x <= 1; ++x)
            {
                for (int y = -1; y <= 1; ++y)
                {
                    if (x == 0 && y == 0)
                        continue;

                    Vector3Int wallPosition = new Vector3Int(position.x + x, position.y + y);
                    CreateWall(board, wallPosition.ToVector2());
                }
            }
        }
    }
}